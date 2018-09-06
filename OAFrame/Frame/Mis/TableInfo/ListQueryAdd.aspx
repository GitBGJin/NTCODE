<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListQueryAdd.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfo.ListQueryAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>编辑</title>
    <script runat="server" language="C#">
    
        //页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["TableRowGuid"] == "c7c2156f-5dca-4a06-bc1c-dc7f6b962243")
                {
                    for (int i = 0; i < Form3.Rows.Count; i++)
                    {
                        foreach (Control c in Form3.Rows[i].Controls)
                        {
                            if (c.ID != null)
                            {
                                if (c.ID == "txt★T1")
                                {
                                    FineUI.TextBox txtT1 = (FineUI.TextBox)Form3.Rows[i].FindControl(c.ID);
                                    if (txtT1 != null)
                                    {
                                        txtT1.Text = "abcd";
                                    }
                                }

                                if (c.ID == "txt★T3")
                                {
                                    FineUI.DatePicker txtT3 = (FineUI.DatePicker)Form3.Rows[i].FindControl(c.ID);
                                    if (txtT3 != null)
                                    {
                                        txtT3.MaxDate = Convert.ToDateTime("2015-01-11");
                                        txtT3.MinDate = Convert.ToDateTime("2015-01-01");
                                    }
                                }
                            }
                        }
                    }
                }



                //清除页面缓存
                Page.Response.Buffer = false;
                Page.Response.Cache.SetNoStore();

                //获取mis表对象
                Com.Sinoyd.Mis.BLL.MisObjectRow oRowTable = new Com.Sinoyd.Mis.BLL.MisObjectRow("TB_MisTable", Request["TableRowGuid"], "");

                //设置文本宽度
                Form3.LabelWidth = Unit.Parse(new Com.Sinoyd.Mis.BLL.B_Convert().ConvertStrToInt(oRowTable["LabelWidth"]).ToString());

                //页面编辑
                if (!string.IsNullOrEmpty(Request["RowGuid"]))
                {
                    //初始化编辑字段
                    new Com.Sinoyd.Mis.BLL.Edit().PageEdit_Init_NoPlatform(Request["RowGuid"], oRowTable["SQLTableName"].ToString(), Form3, "2");

                    //如果是明细页面，那么设置只读、隐藏相应按钮
                    if (Request["Type"] == "detail")
                    {
                        new Com.Sinoyd.Mis.BLL.Detail().EnablePageControlReadonly(Form3, "2");
                        Toolbar2.Hidden = true;
                        btnNew.Hidden = true;
                        Toolbar1.Hidden = true;
                        Grid1.EnableCheckBoxSelect = false;
                    }

                    ViewState["RowGuid"] = Request["RowGuid"];

                    //刷新字段处理
                    System.Data.DataView dv = new Com.Sinoyd.Mis.BLL.Edit().RefreshFieldOperate(Request["TableRowGuid"]);
                    for (int i = 0; i < dv.Count; i++)
                    {
                        InitRefreshFieldName(dv[i]["SQLFieldName"].ToString(), dv[i]["RefreshFieldName"].ToString(), oRowTable["SQLTableName"].ToString());
                    }
                }
                else
                {
                    //页面新增时
                    ViewState["RowGuid"] = Guid.NewGuid().ToString();
                    btnNew.Icon = Icon.Add;
                    btnNew.Text = "添加";
                }

                //非主表
                if (oRowTable["IsPrimary"].ToString() == "0")
                {
                    Form5.Hidden = true;
                }
                else
                {
                    ts1.Height = Unit.Parse(oRowTable["SonTableHeight"].ToString());
                    InitTab();
                }

                //没有附件
                if (oRowTable["FormAttach"].ToString() == "0")
                {
                    Form4.Hidden = true;
                }
                else
                {
                    Button1.OnClientClick = Window1.GetShowReference("../Attach/AttachAdd.aspx?TableRowGuid=" + Request["TableRowGuid"] + "&RowGuid=" + ViewState["RowGuid"].ToString(), "新增附件");
                    lodaattach();
                }

                //自动滚动条
                Panel1.AutoScroll = oRowTable["AutoScroll"].ToString() == "1" ? true : false;

                //垃圾处理情况
                if (Request["TableRowGuid"] == "19835b8c-552b-4c48-ba5d-587c517fd5d2")
                {
                    for (int i = 0; i < Form3.Rows.Count; i++)
                    {
                        foreach (Control c in Form3.Rows[i].Controls)
                        {
                            if (c.ID != null)
                            {
                                if (c.ID == "txt★handleWeight")
                                {
                                    FineUI.NumberBox txt = (FineUI.NumberBox)Form3.Rows[i].FindControl(c.ID);
                                    txt.MaxValue = 100;
                                    txt.MinValue = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            //获取Mis表对象
            Com.Sinoyd.Mis.BLL.MisObjectRow oRowTable = new Com.Sinoyd.Mis.BLL.MisObjectRow("TB_MisTable", Request["TableRowGuid"], "");

            //判断下拉框、单选框是否选择了有效项（下拉框新增时会追加一行“请选择”）
            string CheckMessage = new Com.Sinoyd.Mis.BLL.Add().PageAddCheck(oRowTable["SQLTableName"].ToString(), Form3, "2");
            if (CheckMessage != "")
            {
                Alert.Show(CheckMessage);
                return;
            }

            //判断记录唯一性
            string errorMessage = "";
            if (RowUnique(oRowTable["SQLTableName"].ToString(), oRowTable["DeleteType"], ViewState["RowGuid"].ToString(), out errorMessage) == true)
            {
                Alert.Show(errorMessage, "错误", MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(Request["RowGuid"]))
            {
                //通过Mis新增
                new Com.Sinoyd.Mis.BLL.Add().PageAdd_NoPlatform(ViewState["RowGuid"].ToString(), oRowTable["SQLTableName"].ToString(), Form3, "2");

                //保存外键、保存默认值
                new Com.Sinoyd.Mis.BLL.Add().SetDefaultValue(ViewState["RowGuid"].ToString(), Request["ParentRowGuid"], Request["TableRowGuid"], oRowTable);

                FineUI.Alert.Show("添加成功！", "添加成功！", ActiveWindow.GetHidePostBackReference());
            }
            else
            {
                //页面编辑时
                new Com.Sinoyd.Mis.BLL.Edit().PageEdit_Mis_NoPlatform(ViewState["RowGuid"].ToString(), oRowTable["SQLTableName"].ToString(), Form3, "2");

                FineUI.Alert.Show("保存成功！", "保存成功！", ActiveWindow.GetHidePostBackReference());
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" EnableAjax="true"></f:PageManager>
        <f:Panel ID="Panel1" runat="server" ShowBorder="False" ShowHeader="false" BodyPadding="5px"
            AutoScroll="true" Layout="Form">
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
                    runat="server" LabelWidth="80" EnableCollapse="true" Collapsed="false">
                </f:Form>
                <f:Form runat="server" ID="Form4" ShowBorder="false" ShowHeader="false">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:Panel ID="Panel2" runat="server" Layout="Fit" Title="附件" ShowBorder="true" ShowHeader="true"
                                    EnableCollapse="true" EnableAjax="true">
                                    <Toolbars>
                                        <f:Toolbar ID="Toolbar2" runat="server">
                                            <Items>
                                                <f:Button ID="Button1" Text="上传附件" runat="server" Icon="Add" EnablePostBack="true">
                                                </f:Button>
                                                <f:Button ID="Button2" Text="删除附件" runat="server" Icon="Delete" ConfirmText="您确定删除选中的附件?"
                                                    EnableAjax="true" OnClick="delAT">
                                                </f:Button>
                                            </Items>
                                        </f:Toolbar>
                                    </Toolbars>
                                    <Items>
                                        <f:Grid ID="Grid1" ShowBorder="false" Icon="Eye" AllowPaging="false" EnableCheckBoxSelect="True"
                                            Height="120" ShowHeader="false" runat="server" DataKeyNames="RowGuid" EnableHeaderMenu="false">
                                            <Columns>
                                                <f:RowNumberField></f:RowNumberField>
                                                <f:TemplateField HeaderText="预览" Width="60" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <a target="_blank" href='../<%#DataBinder.Eval(Container.DataItem,"[Src]")%>'>
                                                            <img src="../images/icons/zoom.png"></a>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField HeaderText="附件名称" ExpandUnusedSpace="true" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <div style="text-align: left;">
                                                            <%#DataBinder.Eval(Container.DataItem,"[FileName]")%>
                                                        </div>
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
                <f:Form runat="server" ID="Form5" ShowBorder="false" ShowHeader="false">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:TabStrip runat="server" ID="ts1" Height="400">
                                </f:TabStrip>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true"
            EnableDrag="true" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Self"
            Width="400px" Height="100px" OnClose="Window1_Close" Hidden="true">
        </f:Window>
        <f:Window ID="Window2" Title="明细选择" EnableIFrame="true" runat="server" EnableCollapse="true"
            EnableDrag="true" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Parent"
            Width="400px" Height="600px" OnClose="Window1_Close" Hidden="true">
        </f:Window>
    </form>
</body>
</html>
