﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddApply_OneTime.aspx.cs"
    Inherits="TK.Mis.Web.TableInfoSet.AddApply_OneTime" %>

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
                    <f:Button ID="btnNew" Text="上报" runat="server" Icon="DriveAdd" ValidateForms="Form3" OnClick="btnNew_Click" ConfirmText="您确定上报吗？上报后数据不允许修改!">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form Title="基本信息" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false" runat="server" EnableLightBackgroundColor="true" LabelWidth="60" EnableCollapse="true">
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_TableName" Label="报表名称" Readonly="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea runat="server" ID="txt_Note" Label="填报说明" Height="150" Readonly="true">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ColumnWidths="50% 50%" Hidden="true">
                        <Items>
                            <f:RadioButtonList runat="server" ID="rbl_CollectType" Label="汇总方式" Required="true" ShowRedStar="true" Hidden="true">
                                <f:RadioItem Value="1" Text="横向累加"></f:RadioItem>
                                <f:RadioItem Value="2" Text="纵向累加" Selected="true"></f:RadioItem>
                            </f:RadioButtonList>
                            <f:TextBox runat="server" ID="txt_SheetName" Label="Sheet名" Required="true" ShowRedStar="true" Text="" Hidden="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ColumnWidths="25% 25% 25% 25%" Hidden="true">
                        <Items>
                            <f:TextBox runat="server" ID="txt_FromCelColumn" Label="开始列" Required="true" ShowRedStar="true" Hidden="true">
                            </f:TextBox>
                            <f:TextBox runat="server" ID="txt_FromCellRow" Label="开始行" Required="true" ShowRedStar="true" Hidden="true">
                            </f:TextBox>
                            <f:TextBox runat="server" ID="txt_EndCellColumn" Label="结束列" Required="true" ShowRedStar="true" Hidden="true">
                            </f:TextBox>
                            <f:TextBox runat="server" ID="txt_EndCellRow" Label="结束行" Required="true" ShowRedStar="true" Hidden="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow Hidden="true">
                        <Items>
                            <f:TriggerBox ID="CopyNameStr" EnableEdit="false" Text="" EnablePostBack="false" TriggerIcon="Search" Label="填报人员" runat="server" Required="true" ShowRedStar="true" Hidden="true">
                            </f:TriggerBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow Hidden="true">
                        <Items>
                            <f:TextBox runat="server" ID="CopyGuidStr" Hidden="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:Panel ID="Panel2" runat="server" Layout="Fit" Title="报表模板" ShowBorder="true" ShowHeader="true" EnableCollapse="false" EnableAjax="true">
                                <Items>
                                    <f:Grid ID="Grid1" ShowBorder="false" Icon="Eye" AllowPaging="false" EnableCheckBoxSelect="false" Height="60" ShowHeader="false" runat="server" DataKeyNames="RowGuid" RowVerticalAlign="Middle" AutoScroll="true">
                                        <Columns><f:RowNumberField></f:RowNumberField>
                                            <f:BoundField DataToolTipField="FileName" Width="150px" DataField="FileName" DataFormatString="{0}" HeaderText="报表名称" ExpandUnusedSpace="True"></f:BoundField>
                                            <f:TemplateField HeaderText="预览" Width="40" TextAlign="Center">
                                                <ItemTemplate>
                                                    <a href='../../<%# DataBinder.Eval(Container.DataItem,"[Src]")%>' target="_blank">
                                                        <img src="../../images/icons/zoom.png"></a>
                                                </ItemTemplate>
                                            </f:TemplateField>
                                        </Columns>
                                    </f:Grid>
                                </Items>
                            </f:Panel>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:ContentPanel runat="server" ID="cp_001" ShowBorder="false" ShowHeader="false">
                                <div style="height: 5px;">
                                </div>
                            </f:ContentPanel>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:Panel ID="Panel3" runat="server" Layout="Fit" Title="报表" ShowBorder="true" ShowHeader="true" EnableCollapse="false" EnableAjax="true">
                                <Toolbars>
                                    <f:Toolbar ID="Toolbar2" runat="server">
                                        <Items>
                                            <f:Button ID="Button1" Text="上传报表" runat="server" Icon="Add" EnablePostBack="false">
                                            </f:Button>
                                            <f:Button ID="Button2" Text="删除报表" runat="server" Icon="Delete" ConfirmText="您确定删除选中的报表吗?" EnableAjax="true" OnClick="delAT">
                                            </f:Button>
                                        </Items>
                                    </f:Toolbar>
                                </Toolbars>
                                <Items>
                                    <f:Grid ID="Grid2" ShowBorder="false" Icon="Eye" AllowPaging="false" EnableCheckBoxSelect="True" Height="80" ShowHeader="false" runat="server" DataKeyNames="RowGuid" RowVerticalAlign="Middle" AutoScroll="true">
                                        <Columns><f:RowNumberField></f:RowNumberField>
                                            <f:BoundField DataToolTipField="FileName" Width="150px" DataField="FileName" DataFormatString="{0}" HeaderText="报表名称" ExpandUnusedSpace="True"></f:BoundField>
                                            <f:TemplateField HeaderText="预览" Width="40" TextAlign="Center">
                                                <ItemTemplate>
                                                    <a href='../../<%# DataBinder.Eval(Container.DataItem,"[Src]")%>' target="_blank">
                                                        <img src="../../images/icons/zoom.png"></a>
                                                </ItemTemplate>
                                            </f:TemplateField>
                                        </Columns>
                                    </f:Grid>
                                </Items>
                            </f:Panel>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>
    <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Self" Width="550px" Height="450px" Hidden="true">
    </f:Window>
    <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Self" Width="550px" Height="150px" OnClose="Window1_Close" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
