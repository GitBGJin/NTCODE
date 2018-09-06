<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Chart.Add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>报表信息</title>
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
                    <f:Button ID="btnNew" Text="保存" runat="server" Icon="Disk" ValidateForms="Form3" OnClick="btnNew_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form Title="基本信息" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false" runat="server" LabelWidth="80" EnableCollapse="true">
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_Title" Label="标题" Required="true" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_TitleColor" Label="标题颜色" MaxLength="7" Text="#000000" Required="true" ShowRedStar="true">
                            </f:TextBox>
                            <f:NumberBox runat="server" ID="txt_TitleSize" Label="标题大小" DecimalPrecision="0" Text="4" MinValue="1" MaxValue="7" Required="true" ShowRedStar="true">
                            </f:NumberBox>
                            <f:DropDownList runat="server" ID="ddl_TitleIsBold" Label="是否加粗" Required="true" ShowRedStar="true">
                                <f:ListItem Text="是" Value="1"></f:ListItem>
                                <f:ListItem Text="否" Value="0" Selected="true"></f:ListItem>
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_SubTitle" Label="副标题">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_YTitle" Label="Y轴标题">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:DropDownList runat="server" ID="ddl_ChartType" Label="报表类型">
                                <f:ListItem Text="柱形图" Value="1"></f:ListItem>
                                <f:ListItem Text="圆形图" Value="2"></f:ListItem>
                                <f:ListItem Text="折线图" Value="3" Selected="true"></f:ListItem>
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea runat="server" ID="txt_SourceSql" Label="数据源sql">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea runat="server" ID="txt_XNameSql" Label="X轴数据sql">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea runat="server" ID="txt_XGroupSql" Label="X轴分组sql">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_XName" Label="X轴分组名">
                            </f:TextBox>
                            <f:TextBox runat="server" ID="txt_XValue" Label="X轴值">
                            </f:TextBox>
                            <f:TextBox runat="server" ID="txt_YValue" Label="Y轴值">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:ContentPanel runat="server" ID="cp1" ShowBorder="false" ShowHeader="false">
                                <table>
                                    <tr>
                                        <td width="80" rowspan="2">
                                        </td>
                                        <td>
                                            标题颜色示例：#FF0000,标题大小为1-7之间的数字（越大则标题越大）
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            动态参数请加“@”,示例：@SaleDate。X轴值、Y轴值用sql数据库中的字段（非显示字段）
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
    </form>
</body>
</html>
