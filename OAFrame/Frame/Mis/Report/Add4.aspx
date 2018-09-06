<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add4.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Report.Add4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>报表信息(交叉统计表)</title>
</head>
<body>
    <form id="form1" runat="server">
    <script>

    </script>
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" EnableAjax="true">
    </f:PageManager>
    <f:Panel ID="Panel1" runat="server" ShowBorder="False" ShowHeader="false" BodyPadding="5px"
        AutoScroll="true">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnNew" Text="保存" runat="server" Icon="Disk" ValidateForms="Form3"
                        OnClick="btnNew_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form Title="基本信息" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false"
                runat="server" LabelWidth="100" EnableCollapse="true">
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_Title" Label="标题" Required="true" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:DropDownList runat="server" ID="ddl_ReportType" Label="报表类型" Required="true" ShowRedStar="true"
                                Readonly="true">
                                <f:ListItem Text="简易报表" Value="1" />
                                <f:ListItem Text="固定报表" Value="2" />
                                <f:ListItem Text="分组报表" Value="3" />
                                <f:ListItem Text="交叉统计表" Value="4" Selected="true" />
                            </f:DropDownList>
                            <f:DropDownList runat="server" ID="ddl_ListType" Label="内容类型" Required="true"
                                ShowRedStar="true">
                                <f:ListItem Text="文本" Value="s" />
                                <f:ListItem Selected="true" Text="数字(0位)" Value="0" />
                                <f:ListItem Text="数字(1位)" Value="1" />
                                <f:ListItem Text="数字(2位)" Value="2" />
                                <f:ListItem Text="数字(3位)" Value="3" />
                                <f:ListItem Text="数字(4位)" Value="4" />
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea runat="server" ID="txt_FilterSql" Label="X轴sql语句" Required="true" ShowRedStar="true"
                                Text="" Height="70">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea runat="server" ID="txt_OrderSql" Label="Y轴sql语句" Height="70">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_XSql" Label="X轴名称" Required="true" ShowRedStar="true">
                            </f:TextBox>
                            <f:TextBox runat="server" ID="txt_YSql" Label="Y轴名称" Required="true" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:DropDownList runat="server" ID="ddl_ListPosition" Label="显示位置" Required="true"
                                ShowRedStar="true">
                                <f:ListItem Text="居左" Value="1" />
                                <f:ListItem Text="居中" Value="2" Selected="true" />
                                <f:ListItem Text="居右" Value="3" />
                            </f:DropDownList>
                            <f:DropDownList runat="server" ID="ddl_ShowSummary" Label="显示合计" Required="true"
                                ShowRedStar="true">
                                <f:ListItem Text="是" Value="1" />
                                <f:ListItem Selected="true" Text="否" Value="0" />
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea runat="server" ID="txt_SourceSql" Label="主查询sql语句" Required="true" ShowRedStar="true"
                                Height="70">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow Hidden="true">
                        <Items>
                            <f:DropDownList runat="server" ID="ddl_EnableNumber" Label="显示序号" Required="true"
                                ShowRedStar="true" Hidden="true">
                                <f:ListItem Text="是" Value="1" />
                                <f:ListItem Selected="true" Text="否" Value="0" />
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
