<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListQueryAdd.aspx.cs" Inherits="TK.Mis.Web.TableInfoSet.ListQueryAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <script src="../../WorkFlow/JavaScript/Common.js" type="text/javascript"></script>
    <script>

        function downloadview(rowguid, type)
        {
            if (type == "xls" || type == "doc" || type == "ppt" || type == "xlsx" || type == "docx" || type == "pptx")
            {
                OpenWindow("../../AttachSet/View.aspx?AttachGuid=" + rowguid + "&Type=" + type, window.screen.availWidth, window.screen.availHeight);
            }
            else
            {
                OpenWindow("../../AttachSet/AttachView.aspx?RowGuid=" + rowguid + "&Type=" + type);
            }
        }
    </script>
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Form4" EnableAjax="true">
    </f:PageManager>
    <f:Panel ID="Panel1" runat="server" ShowBorder="False" ShowHeader="false" BodyPadding="5px" AutoScroll="true">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server" Hidden="true">
                <Items>
                    <f:Button ID="btnNew" Text="保存" runat="server" Icon="Disk" ValidateForms="Form3" OnClick="btnNew_Click" Hidden="true">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form Title="基本信息" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false" runat="server" EnableLightBackgroundColor="true" LabelWidth="60" EnableCollapse="true">
            </f:Form>
            <f:Form runat="server" ID="Form4" ShowBorder="false" ShowHeader="false" Hidden="true">
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:Panel ID="Panel2" runat="server" Layout="Fit" Title="附件" ShowBorder="true" ShowHeader="true" EnableCollapse="false" EnableAjax="true" Hidden="true">
                                <Toolbars>
                                    <f:Toolbar ID="Toolbar2" runat="server">
                                        <Items>
                                            <f:Button ID="Button1" Text="上传附件" runat="server" Icon="Add" EnablePostBack="false">
                                            </f:Button>
                                            <f:Button ID="Button2" Text="删除附件" runat="server" Icon="Delete" ConfirmText="您确定删除选中的附件?" EnableAjax="true" OnClick="delAT">
                                            </f:Button>
                                        </Items>
                                    </f:Toolbar>
                                </Toolbars>
                                <Items>
                                    <f:Grid ID="Grid1" ShowBorder="false" Icon="Eye" AllowPaging="false" EnableCheckBoxSelect="True" Height="500" ShowHeader="false" runat="server" DataKeyNames="RowGuid" RowVerticalAlign="Middle" AutoScroll="true">
                                        <Columns><f:RowNumberField></f:RowNumberField>
                                            <f:BoundField DataToolTipField="FileName" Width="150px" DataField="FileName" DataFormatString="{0}" HeaderText="附件名称" ExpandUnusedSpace="True"></f:BoundField>
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
    <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Top" Width="400px" Height="100px" OnClose="Window1_Close" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
