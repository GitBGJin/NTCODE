<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MethodRegister.aspx.cs"
    Inherits="Com.Sinoyd.Mis.WebUI.Report.MethodRegister" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mis表管理</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region2" Split="false" Width="200px"
                Margins="3 3 3 3" ShowHeader="false" EnableCollapse="true" ShowBorder="false"
                Layout="Fit" Position="Left" runat="server">
                <Items>
                    <f:Tree runat="server" EnableArrows="true" ShowBorder="true" BodyPadding="5px" ShowHeader="false"
                        AutoScroll="true" ID="treeDll" Expanded="true" OnNodeCommand="Tree1_NodeCommand">
                        <Nodes>
                        </Nodes>
                    </f:Tree>
                </Items>
            </f:Region>
            <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="3 3 3 3" Position="Center"
                ShowBorder="false" runat="server">
                <Items>
                    <f:Panel ID="Panel2" runat="server" ShowBorder="true" ShowHeader="false" Layout="Fit"
                        >
                        <Items>
                            <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server" AnchorValue="100% -36"
                                Layout="Fit">
                                <Toolbars>
                                    <f:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                            <f:Button ID="btnAdd" Text="新增" runat="server" Icon="Add" OnClick="btnAdd_Click">
                                            </f:Button>
                                        </Items>
                                    </f:Toolbar>
                                </Toolbars>
                                <Items>
                                    <f:Form Title="AddForm" Width="750px" LabelWidth="100px" 
                                        BodyPadding="5px" ID="Form2" runat="server" ShowBorder="false" ShowHeader="false">
                                        <Rows>
                                            <f:FormRow ColumnWidths="50%  50%">
                                                <Items>
                                                    <f:Label ID="txt_MethodName" Label="方法名称" runat="Server">
                                                    </f:Label>
                                                    <f:Label ID="txt_ReturnValueType" Label="返回类型" runat="Server">
                                                    </f:Label>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:TextBox ID="txt_Note" Label="备注" runat="server">
                                                    </f:TextBox>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:DropDownList runat="Server" ID="ddl_param" Label="参数名称" AutoPostBack="true">
                                                    </f:DropDownList>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:TriggerBox ID="tb_param" EnableEdit="true" Text="" EnablePostBack="false" TriggerIcon="Search"
                                                        Label="参数值" runat="server">
                                                    </f:TriggerBox>
                                                    <f:HiddenField ID="hide_pv" runat="server">
                                                    </f:HiddenField>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:HiddenField ID="hide_TypeName" runat="server">
                                                    </f:HiddenField>
                                                    <f:HiddenField ID="hide_DllName" runat="server">
                                                    </f:HiddenField>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:Button ID="brn_SParam" Text="保存参数" runat="server" Icon="Add" OnClick="btn_SaveParam">
                                                    </f:Button>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:Grid ID="Grid1" Title="Grid1" ShowBorder="false" ShowHeader="false"
                                                        runat="server" EnableCheckBoxSelect="false" DataKeyNames="MethodGuid" EnableHeaderMenu=false BodyPadding="3">
                                                        <Columns>
                                                            <f:BoundField Width="150px" DataField="ParameterName" HeaderText="参数名称" />
                                                            <f:BoundField Width="150px" DataField="ParameterType" HeaderText="类型" />
                                                            <f:BoundField Width="150px" DataField="ParameterValue" HeaderText="参数值" />
                                                        </Columns>
                                                    </f:Grid>
                                                </Items>
                                            </f:FormRow>
                                        </Rows>
                                    </f:Form>
                                </Items>
                            </f:Panel>
                        </Items>
                    </f:Panel>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    <f:Window ID="WinAdd" Title=""  EnableIFrame="true" runat="server" EnableConfirmOnClose="false"
        IFrameUrl="about:blank" Target="Parent" IsModal="True" Width="550px" Height="350px" Hidden=true>
    </f:Window>
    <f:Window ID="TableStruct" Title="" EnableIFrame="true" runat="server"
        EnableConfirmOnClose="false" IFrameUrl="TableStructList.aspx" Target="Top" IsModal="True"
        Width="950px" Height="550px" Hidden=true>
    </f:Window>
    <f:Window ID="WinParam" Title="Edit" EnableIFrame="true" runat="server"
        EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Parent" IsModal="True"
        Width="450px" Height="450px" Hidden=true>
    </f:Window>
    </form>
</body>
</html>
