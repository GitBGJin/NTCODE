<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="TK.Mis.Web.TableInfoSet.Circle.Add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>数据表信息</title>
</head>
<body>
    <form id="form1" runat="server">
    <script>

    </script>
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" EnableAjax="true"></f:PageManager>
    <f:Panel ID="Panel1" runat="server" ShowBorder="False" ShowHeader="false" BodyPadding="5px" AutoScroll="true">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnNew" Text="新建" runat="server" Icon="Disk" ValidateForms="Form3" OnClick="btnNew_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form Title="基本信息" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false" runat="server" EnableLightBackgroundColor="true" LabelWidth="90" EnableCollapse="true">
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:RadioButtonList runat="server" ID="rbl_Type" Label="周期类型">
                                <f:RadioItem Text="日报" Value="日报"></f:RadioItem>
                                <f:RadioItem Text="周报" Value="周报"></f:RadioItem>
                                <f:RadioItem Selected="true" Text="月报" Value="月报"></f:RadioItem>
                                <f:RadioItem Text="两月报" Value="两月报"></f:RadioItem>
                                <f:RadioItem Text="季报" Value="季报"></f:RadioItem>
                            </f:RadioButtonList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ColumnWidths="50% 50%">
                        <Items>
                            <f:DatePicker Label="开始日期" runat="server" ID="txt_FromDate" DateFormatString="yyyy-MM-dd" ShowRedStar="true" Width="120" Required="true">
                            </f:DatePicker>
                            <f:DatePicker Label="结束日期" runat="server" ID="txt_EndDate" DateFormatString="yyyy-MM-dd" ShowRedStar="true" Width="120" Required="true">
                            </f:DatePicker>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:ContentPanel runat="server" ID="cp1" ShowHeader="false" ShowBorder="false" Title="填报说明" Hidden="true">
                                <table>
                                    <tr height="20">
                                        <td style="color: Red;">
                                            填写说明：
                                        </td>
                                    </tr>
                                    <tr height="20">
                                        <td style="padding-left: 25px;">
                                            1、此模块中的添加为批量添加
                                        </td>
                                    </tr>
                                    <tr height="20">
                                        <td style="padding-left: 25px;">
                                            2、报名名称中动态部分请用##代替
                                        </td>
                                    </tr>
                                    <tr height="20">
                                        <td style="padding-left: 25px;">
                                            3、如类型为月报，则##替换为****年度**月份
                                        </td>
                                    </tr>
                                    <tr height="20">
                                        <td style="padding-left: 25px;">
                                            4、如类型为月报，则##替换为****年度**季度
                                        </td>
                                    </tr>
                                    <tr height="20">
                                        <td style="padding-left: 25px;">
                                            5、如类型为年报，则##替换为****年度
                                        </td>
                                    </tr>
                                </table>
                            </f:ContentPanel>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>
    <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Self" Width="550px" Height="450px" Hidden="true">
    </f:Window>
    <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Self" Width="550px" Height="150px" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
