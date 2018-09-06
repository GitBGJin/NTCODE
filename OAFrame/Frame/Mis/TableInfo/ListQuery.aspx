<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListQuery.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfo.ListQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <script language="C#" runat="server"> 
        //页面加载代码
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //针对Test1表定制
                if (Request["TableRowGuid"] == "c7c2156f-5dca-4a06-bc1c-dc7f6b962243")
                {
                    FineUI.DatePicker txt_Dp = (FineUI.DatePicker)table1.FindControl("txt★T3From");
                    if (txt_Dp != null)
                    {
                        txt_Dp.Text = "2005-01-01";
                    }

                    FineUI.DatePicker txt_DpTo = (FineUI.DatePicker)table1.FindControl("txt★T3To");
                    if (txt_DpTo != null)
                    {
                        txt_DpTo.Text = "2005-01-01";
                    }
                }
                
                ViewState["PageNow"] = "1";
                ViewState["SortField"] = "";
                ViewState["SortDirection"] = "";
                InitRowStatus();
                loaddata();
                btnNew.OnClientClick = Window1.GetShowReference(txt_EditUrl.Text + "?TableRowGuid=" + Request["TableRowGuid"]
                    + "&ParentRowGuid=" + Request["ParentRowGuid"] + "&Guid=" + Guid.NewGuid().ToString(), "添加");

                //信访数据查询
                if (Request["TableRowGuid"] == "a125aefa-a88e-47dc-87f1-b28757b52f87")
                {
                    Grid1.ShowBorder = true;
                    if (!string.IsNullOrEmpty(Request["st"]))
                    {
                        FineUI.DatePicker txt_St = (FineUI.DatePicker)table1.FindControl("txt★petitionDateFrom");
                        if (txt_St != null)
                        {
                            txt_St.Text = Request["st"];
                        }
                    }
                    if (!string.IsNullOrEmpty(Request["et"]))
                    {
                        FineUI.DatePicker txt_Et = (FineUI.DatePicker)table1.FindControl("txt★petitionDateTo");
                        txt_Et.Text = Request["et"];
                    }
                }
            }
        }

        //搜索代码
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FineUI.NumberBox txt_T5From = (FineUI.NumberBox)table1.FindControl("txt★T5From");
            if (txt_T5From != null)
            {
                if (txt_T5From.Text != "")
                {
                    if (Convert.ToDecimal(txt_T5From.Text) > 100)
                    {
                        FineUI.Alert.Show("数字超出了100！");
                        return;
                    }
                }
            }
            
            ViewState["PageNow"] = "1";
            loaddata();
        }

        //删除代码
        protected void btnDel_OnClick(object sender, EventArgs e)
        {
            if (Grid1.SelectedRowIndexArray != null && Grid1.SelectedRowIndexArray.Length > 0)
            {
                Com.Sinoyd.Mis.BLL.MisObjectRow oRowTable = new Com.Sinoyd.Mis.BLL.MisObjectRow("TB_MisTable", Request["TableRowGuid"], "");
                string sqlTableName = oRowTable["SQLTableName"];

                int rowIndex;
                for (int i = 0, count = Grid1.SelectedRowIndexArray.Length; i < count; i++)
                {
                    rowIndex = Grid1.SelectedRowIndexArray[i];
                    string rowGuid = Convert.ToString(Grid1.DataKeys[rowIndex][0]);
                    if (oRowTable["DeleteType"] == "1")
                    {
                        new Com.Sinoyd.Mis.Dal.DBCommon().RecordDeleteAll(
                            new Com.Sinoyd.Mis.BLL.B_Convert().FiltrateSql(Request["TableRowGuid"]),sqlTableName, rowGuid);
                    }
                    else
                    {
                        new Com.Sinoyd.Mis.Dal.DBCommon().RecordDeleteAll_Logic(
                            new Com.Sinoyd.Mis.BLL.B_Convert().FiltrateSql(Request["TableRowGuid"]), sqlTableName, rowGuid);
                    }
                }
                FineUI.Alert.Show("删除成功。");
                loaddata();
            }
            else
            {
                FineUI.Alert.Show("请选择数据!");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            FineUI.Alert.Show("test");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
        </script>
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
        <f:Panel ID="Panel1" runat="server" BodyPadding="3px" ShowBorder="false" ShowHeader="false"
            Height="300px" Layout="Anchor">
            <Items>
                <f:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="5px" ShowBorder="false"
                    ShowHeader="false" Title="ContentPanel">
                    <table>
                        <tr>
                            <td>
                                <table id="table1" runat="server">
                                </table>
                            </td>
                            <td>
                                <f:Button ID="btnImport" Text="查找" runat="server" OnClick="btnSearch_Click" Icon="Magnifier">
                                </f:Button>
                            </td>
                            <td style="display:none;">
                                <f:TextBox ID="txt_EditUrl" Text="ListQueryAdd.aspx" runat="server" Hidden="true">
                                </f:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" ImageUrl="images/search.png" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </f:ContentPanel>
                <f:Panel ID="Panel2" ShowBorder="True" ShowHeader="false" runat="server" AnchorValue="100% -36"
                    Layout="Fit">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnNew" Text="添加" runat="server" Icon="Add" EnablePostBack="true">
                                </f:Button>
                                <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true"
                                    OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的数据吗？">
                                </f:Button>
                                <f:Button ID="btnExport" Text="导出" runat="server" Icon="PageExcel" EnableAjax="false"
                                    OnClick="btnExport_OnClick" Hidden="true">
                                </f:Button>
                                <f:Button ID="Button1" Text="导出" runat="server" Icon="PageExcel" EnableAjax="true"
                                     OnClick="Button1_Click" Hidden="true">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Grid ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False"
                            AllowSorting="true" SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="true"
                            DataKeyNames="RowGuid" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                            OnSort="Grid1_Sort" EnableTextSelection="true" SortField="ID" EnableHeaderMenu="false">
                            <Columns>
                                <f:RowNumberField Width="30"></f:RowNumberField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" Title="Add" EnableIFrame="true" runat="server" EnableCollapse="false"
            EnableDrag="false" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Self"
            Width="600px" Height="400px" OnClose="Window1_OnClose" Hidden="true">
        </f:Window>
        <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="false"
            EnableDrag="false" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Self"
            Width="600px" Height="400px" OnClose="Window2_OnClose" Hidden="true">
        </f:Window>
    </form>
</body>
</html>
