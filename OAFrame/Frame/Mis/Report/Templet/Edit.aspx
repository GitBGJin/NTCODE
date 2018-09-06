<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Report.Templet.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>模板</title>
</head>
<body>
    <form id="form1" runat="server">
    <script>
        function DocEdit()
        {
            var RowGuid = "<%=Request.QueryString["RowGuid"] %>";
            parent.window.showwindows(RowGuid+'zwbj', 'OA/Fawen/Zwbj.aspx?RowGuid='+RowGuid+'&id='+Math.random(), '正文编辑');
        }

        function submit(FwlbModuleGuid)
        {
            var RowGuid = "<%=Request.QueryString["RowGuid"] %>";
            parent.window.delrefreshTab(FwlbModuleGuid,RowGuid);
        }

        function refreshFwlb(FwlbModuleGuid)
        {
            parent.window.refreshTab(FwlbModuleGuid);
        }
    </script>
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" EnableAjax="false" />
    <f:Panel ID="Panel1" runat="server" ShowBorder="False" ShowHeader="false" BodyPadding="5px"
        AutoScroll="true" Width="800">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnSave" Text="保存" runat="server" Icon="Disk" OnClick="btnSave_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form Title="基本信息" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false"
                runat="server" LabelWidth="80" EnableCollapse="true">
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_Title" Label="模板名称" Required="true" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:ContentPanel runat="server" ID="AttachPanel" ShowBorder="false" ShowHeader="false">
                                <table>
                                    <tr>
                                        <td width="80">
                                            上传文件：
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fuFile" runat="server" Width="283px" Height="22"></asp:FileUpload>
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
    <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" Width="650px"
        Height="450px" IFrameUrl="about:blank" Target="Self" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
