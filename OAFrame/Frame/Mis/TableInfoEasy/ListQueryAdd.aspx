﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListQueryAdd.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfoEasy.ListQueryAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" EnableAjax="true"></f:PageManager>
        <f:Panel ID="Panel1" runat="server" ShowBorder="False" ShowHeader="false" BodyPadding="5px" AutoScroll="true" Layout="Fit" Height="1000">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnNew" Text="保存" runat="server" Icon="Disk" ValidateForms="Form3" OnClick="btnNew_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Panel ID="Panel3"  runat="server" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <f:Form Title="基本信息" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false" runat="server" LabelWidth="60" EnableCollapse="true">
                        </f:Form>
                        <f:Form runat="server" ID="Form4" ShowBorder="false" ShowHeader="false">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:Panel ID="Panel2" runat="server" Layout="Fit" Title="附件" ShowBorder="true" ShowHeader="true" EnableCollapse="false" EnableAjax="true">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar2" runat="server">
                                                    <Items>
                                                        <f:Button ID="Button1" Text="上传附件" runat="server" Icon="Add" EnablePostBack="true">
                                                        </f:Button>
                                                        <f:Button ID="Button2" Text="删除附件" runat="server" Icon="Delete" ConfirmText="您确定删除选中的附件?" EnableAjax="true" OnClick="delAT">
                                                        </f:Button>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Items>
                                                <f:Grid ID="Grid1" ShowBorder="false" Icon="Eye" AllowPaging="false"
                                                    EnableCheckBoxSelect="True" Height="200" ShowHeader="false" runat="server"
                                                    DataKeyNames="RowGuid" RowVerticalAlign="Middle" AutoScroll="true" EnableHeaderMenu="false">
                                                    <Columns>
                                                        <f:RowNumberField></f:RowNumberField>
                                                        <f:TemplateField HeaderText="附件名称" ExpandUnusedSpace="true" TextAlign="Center">
                                                            <ItemTemplate>
                                                                <div style="text-align: left;">
                                                                    <%#DataBinder.Eval(Container.DataItem,"[FileName]")%>
                                                                </div>
                                                            </ItemTemplate>
                                                        </f:TemplateField>
                                                        <f:TemplateField HeaderText="预览" Width="60" TextAlign="Center">
                                                            <ItemTemplate>
                                                                <a target="_blank" href='../<%#DataBinder.Eval(Container.DataItem,"[Src]")%>'>
                                                                    <img src="../images/icons/zoom.png"></a>
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
            </Items>
        </f:Panel>
        <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Self" Width="400px" Height="100px" OnClose="Window1_Close" Hidden="true">
        </f:Window>
    </form>
</body>
</html>
