using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.RemoteControl
{
    public partial class RemoteControl : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //string strCN = ViewState["CN"].ToString();
                //DataView dv = (DataView)ViewState["dv"];
                //if (dv.Count > 0)
                //{
                //    bindExpression(dv, strCN);
                //    RadFilter1.RecreateControl();
                //    RadFilter1.FireApplyCommand();
                //}
            }
        }
        #region 绑定控件对应表达式

        /// <summary>
        /// 绑定具体命令参数
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="strCN">命令号</param>
        protected void bindExpression(DataView dv, string strCN)
        {
            RadFilter1.RootGroup.Expressions.Clear();
        }
        #endregion
        protected void RadFilter1_FieldEditorCreating(object sender, Telerik.Web.UI.RadFilterFieldEditorCreatingEventArgs e)
        {
            if (e.EditorType == "RadFilterDropDownEditor")
            {
                e.Editor = new Telerik.Web.UI.RadFilterDropDownEditor();
            }
        }

        protected void RadFilter1_PreRender(object sender, EventArgs e)
        {
            InitialControl(RadFilter1.RootGroupItem.Controls);
        }
        /// <summary>
        /// 控件遍历，文本控件只读，LinkButton类型按钮隐藏
        /// </summary>
        /// <param name="objControlCollection"></param>
        private void InitialControl(ControlCollection objControlCollection)
        {
            foreach (System.Web.UI.Control objControl in objControlCollection)
            {
                if (objControl.HasControls())
                {
                    InitialControl(objControl.Controls);
                }
                else
                {
                    //if (objControl is System.Web.UI.WebControls.TextBox)
                    //{
                    //    ((TextBox)objControl).Enabled = false;
                    //}

                    //if (objControl is Telerik.Web.UI.RadNumericTextBox)
                    //{
                    //    ((RadNumericTextBox)objControl).Enabled = false;
                    //}

                    if (objControl is System.Web.UI.WebControls.LinkButton)
                    {
                        ((LinkButton)objControl).Visible = false;
                    }

                    //if (objControl is Telerik.Web.UI.RadDropDownList)
                    //{
                    //    RadDropDownList list = (Telerik.Web.UI.RadDropDownList)objControl;
                    //    string Sql = "SELECT  portId, MN, monitoringPointName FROM  V_MonitoringPoint_AcquisitionInstrument  order by portid";
                    //    DataView dv = myComm.CreatDataView(Sql, myConnStr);
                    //    list.DataSource = dv;
                    //    list.DataTextField = "monitoringPointName";
                    //    list.DataValueField = "MN";
                    //    list.DataBind();
                    //}
                }
            }
        }

        /// <summary>
        /// 增加根节点表达式
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="keyValue"></param>
        protected void addRootGroupExpression(string key, string keyValue)
        {
            RadFilterEqualToFilterExpression<string> Expr = new RadFilterEqualToFilterExpression<string>(key);
            RadFilter1.RootGroup.AddExpression(Expr);
            Expr.Value = keyValue;

        }

        /// <summary>
        /// 增加父节点表达式
        /// </summary>
        /// <param name="group"></param>
        /// <param name="keyName"></param>
        /// <param name="columnType"></param>
        protected void addGroupExpression(string commandName, RadFilterGroupExpression group, string key, string columnType)
        {
            switch (columnType)
            {
                case "datetime":
                    RadFilterEqualToFilterExpression<DateTime> dtExpression = new RadFilterEqualToFilterExpression<DateTime>(key);
                    group.AddExpression(dtExpression);
                    dtExpression.Value = DateTime.Now.AddDays(-1);
                    break;
                case "int":
                    RadFilterEqualToFilterExpression<int> intExpression = new RadFilterEqualToFilterExpression<int>(key);
                    if (key.Equals("ContinueTime")) intExpression.Value = 5;
                    group.AddExpression(intExpression);
                    break;
                case "bool":
                    RadFilterEqualToFilterExpression<bool> boolExpression = new RadFilterEqualToFilterExpression<bool>(key);
                    group.AddExpression(boolExpression);
                    break;
                default:
                    RadFilterEqualToFilterExpression<string> strExpression = new RadFilterEqualToFilterExpression<string>(key);
                    if (key.Equals("CalName")) strExpression.Value = "SO2校零检查";
                    else if (key.Equals("Gas")) strExpression.Value = "Zero";
                    else if (key.Equals("CalConc")) strExpression.Value = commandName.Equals("校零") ? "0" : "18000";
                    else if (key.Equals("CalFlow")) strExpression.Value = "4000";

                    group.AddExpression(strExpression);
                    break;

            }
        }

        /// <summary>
        /// 绑定所有的KEY
        /// </summary>
        /// <param name="dv"></param>
        protected void bindFieldEditors(DataView dv)
        {
            RadFilter1.FieldEditors.Clear();
            foreach (DataRowView drv in dv)
            {
                switch (drv["key"].ToString())
                {
                    case "##": break;
                    case "Length": break;
                    case "CRC": break;
                    case "\r\n": break;
                    case "CP": break;
                    default:
                        addFieldEditors(drv["commandName"].ToString(), drv["keyName"].ToString(), drv["key"].ToString(), drv["columnType"].ToString()); break;
                }
            }
        }


        /// <summary>
        /// 根据columnType类型，动态增加FieldEditors数据列
        /// </summary>
        /// <param name="keyName">显示名称</param>
        /// <param name="key">字段名称</param>
        /// <param name="columnType">字段类型</param>
        protected void addFieldEditors(string commandName, string keyName, string key, string columnType)
        {
            if (key == "MN")
            {                           
            }

            switch (columnType)
            {
                case "datetime":
                    RadFilterDateFieldEditor rfd = new RadFilterDateFieldEditor()
                    {
                        DataType = Type.GetType("System.DateTime"),
                        DisplayName = keyName,
                        FieldName = key,
                        PreviewDataFormat = "'{0:yyyy-MM-dd hh:mm:ss}'"
                    };

                    RadFilter1.FieldEditors.Add(rfd);
                    break;
                case "int":
                    RadFilterNumericFieldEditor rfn = new RadFilterNumericFieldEditor()
                    {
                        DataType = Type.GetType("System.Int32"),
                        DisplayName = keyName,
                        FieldName = key
                    };
                    RadFilter1.FieldEditors.Add(rfn);
                    break;
                case "bool":
                    RadFilterBooleanFieldEditor rfb = new RadFilterBooleanFieldEditor()
                    {
                        DataType = Type.GetType("System.Boolean"),
                        DisplayName = keyName,
                        FieldName = key
                    };
                    RadFilter1.FieldEditors.Add(rfb);
                    break;
                default:
                    RadFilterTextFieldEditor rft = new RadFilterTextFieldEditor()
                    {
                        DataType = Type.GetType("System.String"),
                        DisplayName = keyName,
                        FieldName = key
                    };
                    RadFilter1.FieldEditors.Add(rft);

                    break;
            }

        }


        protected void RadFilter1_ExpressionItemCreated(object sender, Telerik.Web.UI.RadFilterExpressionItemCreatedEventArgs e)
        {
            RadFilterSingleExpressionItem singleItem = e.Item as RadFilterSingleExpressionItem;
            if (singleItem != null && singleItem.IsSingleValue && singleItem.InputControl is System.Web.UI.WebControls.TextBox)
            {
                singleItem.InputControl.Enabled = false;
            }
            if (singleItem != null && singleItem.IsSingleValue && singleItem.InputControl is Telerik.Web.UI.RadNumericTextBox)
            {
                singleItem.InputControl.Enabled = false;
            }
            if (singleItem != null && singleItem.FieldName == "MN" && singleItem.IsSingleValue)
            {
                //需要数据源  获取 pointID  MN  monitoringPointName
                DataView dv = new DataView();
                //
                RadDropDownList dropDownList = singleItem.InputControl as RadDropDownList;
                dropDownList.DataSource = dv;
                dropDownList.DataBind();
            }
            else if (singleItem != null && singleItem.FieldName.Equals("CalName") && singleItem.IsSingleValue)
            {

                //需要数据源 dv  校零检查（SELECT 1 as id, '设备全校零'  AS CalName UNION SELECT 2 as id, 'CO校零检查'  AS CalName UNION SELECT 3 as id, 'SO2校零检查'  AS CalName 
                //UNION SELECT 4 as id, 'NO校零检查'  AS CalName      UNION SELECT 5 as id, 'O3校零检查'  AS CalName）和跨度检查（SELECT 8 as id, 'CO内跨度检查'  AS CalName
                // UNION SELECT 11 as id, 'N0内跨度检查'  AS CalName   UNION SELECT 14 as id, 'SO2内跨度检查'  AS CalName  UNION SELECT 17 as id, 'O3内跨度检查'  AS CalName)
                DataView dv = new DataView();
                //
                RadDropDownList dropDownList = singleItem.InputControl as RadDropDownList;
                if (!singleItem.FieldNameChooserLink.Text.Contains("1"))
                {
                    dropDownList.AutoPostBack = true;
                    dropDownList.SelectedIndexChanged += new DropDownListEventHandler(dropDownListSelectChanged2_Name);
                }
                dropDownList.DataSource = dv;
                dropDownList.DataBind();
                singleItem.FieldNameChooserLink.Text = singleItem.FieldNameChooserLink.Text.Replace("1", "").Replace("2", "");
            }
            else if (singleItem != null && singleItem.FieldName.Equals("Gas") && singleItem.IsSingleValue)
            {
                //RadFilterEqualToFilterExpression<string> strExpression = singleItem.Expression as RadFilterEqualToFilterExpression<string>;
                //                string Sql = "";
                //                if (singleItem.FieldNameChooserLink.Text.Contains("1"))
                //                {
                //                    Sql = string.Format(@"SELECT 1 as id, 'Zero'  AS Gas");
                //                }
                //                else
                //                {
                //                    Sql = string.Format(@"SELECT 2 as id,  'CO' AS Gas 
                //                                            UNION SELECT 3 as id, 'NO'  AS Gas
                //                                            UNION SELECT 4 as id, 'SO2'  AS Gas
                //                                            UNION SELECT 5 as id, 'O3'  AS Gas");
                //                }

                //                DataView dv = myComm.CreatDataView(Sql, myConnStr);
                //需要数据源
                DataView dv = new DataView();
                RadDropDownList dropDownList = singleItem.InputControl as RadDropDownList;
                if (!singleItem.FieldNameChooserLink.Text.Contains("1"))
                {
                    dropDownList.AutoPostBack = true;
                    dropDownList.SelectedIndexChanged += new DropDownListEventHandler(dropDownListSelectChanged1_Name);
                }
                dropDownList.DataSource = dv;
                dropDownList.DataBind();
                singleItem.FieldNameChooserLink.Text = singleItem.FieldNameChooserLink.Text.Replace("1", "").Replace("2", "");

            }
            else if (singleItem != null && singleItem.FieldName.Equals("CalConc") && singleItem.IsSingleValue)
            {
                singleItem.InputControl.Enabled = true;
            }
            else if (singleItem != null && singleItem.FieldName.Equals("CalFlow") && singleItem.IsSingleValue)
            {
                singleItem.InputControl.Enabled = true;
            }
            else if (singleItem != null && singleItem.FieldName.Equals("ContinueTime") && singleItem.IsSingleValue)
            {
                singleItem.InputControl.Enabled = true;
                //singleItem.InputControl.
            }

        }
        protected void dropDownListSelectChanged1_Name(object sender, DropDownListEventArgs e)
        {
            RadFilterEqualToFilterExpression<string> strExpression = new RadFilterEqualToFilterExpression<string>("CalConc");
            //RadFilterSingleExpressionItem item = new RadFilterSingleExpressionItem();
            //List<RadFilterSingleExpressionItem> obj = RadFilter1.GetAllExpressionItems().Where(x => x is RadFilterSingleExpressionItem);
            foreach (RadFilterExpressionItem item in RadFilter1.GetAllExpressionItems())
            {
                if (item is RadFilterSingleExpressionItem)
                {
                    RadFilterSingleExpressionItem singleItem = item as RadFilterSingleExpressionItem;
                    if (singleItem.FieldName.Equals("CalConc"))
                    {
                        TextBox calconc = singleItem.InputControl as TextBox;
                        if (e.Value.Equals("SO2"))
                            calconc.Text = "180";
                        else if (e.Value.Equals("NO"))
                            calconc.Text = "450";
                        else if (e.Value.Equals("O3"))
                            calconc.Text = "450";
                        else if (e.Value.Equals("CO"))
                            calconc.Text = "18000";
                        break;
                    }
                }
            }
            strExpression.Value = "1000";
        }

        protected void dropDownListSelectChanged2_Name(object sender, DropDownListEventArgs e)
        {
            RadFilterEqualToFilterExpression<string> strExpression = new RadFilterEqualToFilterExpression<string>("CalConc");
            //RadFilterSingleExpressionItem item = new RadFilterSingleExpressionItem();
            //List<RadFilterSingleExpressionItem> obj = RadFilter1.GetAllExpressionItems().Where(x => x is RadFilterSingleExpressionItem);
            foreach (RadFilterExpressionItem item in RadFilter1.GetAllExpressionItems())
            {
                if (item is RadFilterSingleExpressionItem)
                {
                    RadFilterSingleExpressionItem singleItem = item as RadFilterSingleExpressionItem;
                    if (singleItem.FieldName.Equals("Gas"))
                    {
                        RadDropDownList dropList = singleItem.InputControl as RadDropDownList;
                        if (e.Value.Equals("8"))
                            dropList.SelectedValue = "CO";
                        else if (e.Value.Equals("11"))
                            dropList.SelectedValue = "NO";
                        else if (e.Value.Equals("14"))
                            dropList.SelectedValue = "SO2";
                        else if (e.Value.Equals("17"))
                            dropList.SelectedValue = "O3";
                        //break;
                    }
                    if (singleItem.FieldName.Equals("CalConc"))
                    {
                        TextBox calconc = singleItem.InputControl as TextBox;
                        if (e.Value.Equals("14"))
                            calconc.Text = "180";
                        else if (e.Value.Equals("11"))
                            calconc.Text = "450";
                        else if (e.Value.Equals("17"))
                            calconc.Text = "450";
                        else if (e.Value.Equals("8"))
                            calconc.Text = "18000";
                        break;
                    }
                }
            }
            strExpression.Value = "1000";
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string strMN = "";
                string strFormat = "", strCP = "", strTemp = "";
                RadFilter1.RecreateControl();
                RadFilter1.FireApplyCommand();

                RadFilterSqlQueryProvider provider = new RadFilterSqlQueryProvider();
                provider.ProcessGroup(RadFilter1.RootGroup);

                //遍历DropDownList控件，获取MN号


                try
                {
                    var MNExpression = RadFilter1.RootGroup.Expressions[3] as RadFilterEqualToFilterExpression<string>;
                    strMN = MNExpression.Value;
                }
                catch
                {
                }


                strFormat = provider.Result.ToString().TrimStart('(').TrimEnd(')').Replace("[", "").Replace("]", "").Replace("'", "").Replace("AND", ";").Replace("OR", ";");
                strTemp = strFormat;
                strFormat = strFormat.Replace("(", "CP=&&").Replace("CalName", "Calibrate");

                string[] arrStr = strTemp.Split('(');

                try
                {
                    strCP = arrStr[1].Replace(",", ";");
                    string[] arrstrCP = strCP.Split(';');
                    foreach (string str in arrstrCP)
                    {
                        try
                        {
                            string[] arrValue = str.Split('=');
                            DateTime dt = Convert.ToDateTime(arrValue[1]);
                            string strRepDate = dt.ToString("yyyyMMddHHmmss");
                            strFormat = strFormat.Replace(arrValue[1], strRepDate);
                        }
                        catch
                        { }
                    }
                }
                catch
                {
                    strFormat += ";CP=&&";
                }

                strFormat = strFormat.Replace(" ", "") + "&&";

                #region 异常判断

                if (strFormat.IndexOf("QN=;") >= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "<script>alert(\"请求编号错误！\");var btnDownload = $find(\"ctl00_ContentPlaceHolder2_btnDownload\");btnDownload.set_enabled(true); btnDownload.set_text(\"发送命令\");$get(\"downloadStatus\").style.display = \"none\";</script>", false);
                    return;
                }
                if (strFormat.IndexOf("ST=;") >= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "<script>alert(\"系统编号错误！\");var btnDownload = $find(\"ctl00_ContentPlaceHolder2_btnDownload\");btnDownload.set_enabled(true); btnDownload.set_text(\"发送命令\");$get(\"downloadStatus\").style.display = \"none\";</script>", false);
                    return;
                }
                if (strFormat.IndexOf("CN=;") >= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "<script>alert(\"命令编号错误！\");var btnDownload = $find(\"ctl00_ContentPlaceHolder2_btnDownload\");btnDownload.set_enabled(true); btnDownload.set_text(\"发送命令\");$get(\"downloadStatus\").style.display = \"none\";</script>", false);
                    return;
                }
                if (strFormat.IndexOf("MN=;") >= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "<script>alert(\"请选择测点！\");var btnDownload = $find(\"ctl00_ContentPlaceHolder2_btnDownload\");btnDownload.set_enabled(true); btnDownload.set_text(\"发送命令\");$get(\"downloadStatus\").style.display = \"none\";</script>", false);
                    return;
                }

                #endregion

                string strCmdContent = "##" + strFormat.Length.ToString("D4") + strFormat + GenerateCRC(strFormat) + "\r\n";

                txtQueryProvider.Text += "\r\n**************" + DateTime.Now.ToString() + "**************\r\n";
                txtQueryProvider.Text += " >>>> 发送" + ViewState["cmdDesc"] + "命令:\r\n";
                txtQueryProvider.Text += strCmdContent + "\r\n";
                //发送反控命令


                SendMSMQCommands(strCmdContent, strMN, txtQueryProvider);

                //记录反控命令

                // 

                DataView dv = null;
                bool isSuccess = false;
                int intSleeps = 0;
                string strWhere = " receiveTime>='" + DateTime.Now.AddMinutes(-5).ToString() + "' and cmdContent like '%QN=" + Session["QN"].ToString() + "%' ORDER BY receiveTime DESC ";

                while (!isSuccess && intSleeps <= 10000)
                {
                    if (Page.IsPostBack)
                    {
                        System.Threading.Thread.Sleep(2000);
                        intSleeps += 2000;
                    }

                    //在接收原始表中过滤反控确认包（性能差，需要修改）




                    //判断发送命令是否得到反馈
                    if (dv.Count > 0)
                        isSuccess = true;
                    else
                        isSuccess = false;

                }
                ViewState["isValid"] = "Valid";
                if (isSuccess)
                {
                    txtQueryProvider.Text += " <<<< 应答" + ViewState["cmdDesc"] + "命令:\r\n";
                    txtQueryProvider.Text += dv[0]["cmdContent"].ToString() + "\r\n";
                    txtQueryProvider.Text += " >>>> 请求成功！\r\n";
                    txtQueryProvider.Text += "**************" + DateTime.Now.ToString() + "**************\r\n";

                    //dv = OriginalPacketSendBiz.RetrieveList(" where qn='" + Session["QN"].ToString() + "'");
                    //if (dv.Count > 0)
                    //{
                    //    objOriginal = OriginalPacketSendBiz.RetrieveModel(Convert.ToInt32(dv[0]["id"]));
                    //    objOriginal.execState = "请求成功";
                    //    objOriginal.feedback = "请求成功";
                    //    objOriginal.isFinished = true;
                    //    OriginalPacketSendBiz.Update(objOriginal);
                    //}
                }
                else
                {
                    txtQueryProvider.Text += " >>>> 请求超时！请重新操作！\r\n";
                    txtQueryProvider.Text += "**************" + DateTime.Now.ToString() + "**************\r\n";

                    //dv = OriginalPacketSendBiz.RetrieveList(" where qn='" + Session["QN"].ToString() + "'");
                    //if (dv.Count > 0)
                    //{
                    //    objOriginal = OriginalPacketSendBiz.RetrieveModel(Convert.ToInt32(dv[0]["id"]));
                    //    objOriginal.execState = "请求超时";
                    //    objOriginal.feedback = "无";
                    //    objOriginal.isFinished = false;
                    //    OriginalPacketSendBiz.Update(objOriginal);
                    //}
                }
            }
            catch (Exception ee)
            {
            }
        }


        public static string GenerateCRC(string strData)
        {
            ushort num = 0xffff;
            int length = strData.Length;
            int num4 = 0;
            while (num4 < length)
            {
                byte num6 = (byte)(num >> 8);
                num = (ushort)(((short)strData[num4]) ^ num6);
                num4++;
                for (int i = 0; i < 8; i++)
                {
                    ushort num2 = (ushort)(num & 1);
                    num = (ushort)(num >> 1);
                    if (num2 == 1)
                    {
                        num = (ushort)(num ^ 0xa001);
                    }
                }
            }
            string str = Convert.ToString((int)num, 0x10);
            switch (str.Length)
            {
                case 1:
                    str = "000" + str;
                    break;

                case 2:
                    str = "00" + str;
                    break;
                case 3:
                    str = "0" + str;
                    break;
            }
            return str.ToUpper();
        }

        public void SendMSMQCommands(string strCmdContent, string strMN, TextBox txt)
        {
            string MSMQPath = "";
            try
            {
                MSMQPath = "";//获取路径
            }
            catch
            {
                txt.Text += " >>>> 命令发送失败！请正确配置使用的消息队列路径！\r\n";
                throw new Exception("请正确配置使用的消息队列路径！");
            }
            try
            {
                MessageQueue myMq = new MessageQueue(MSMQPath);

                myMq.Send(strCmdContent, strMN, System.Messaging.MessageQueueTransactionType.Automatic);
            }
            catch (Exception ee)
            {
                txt.Text += " >>>> 命令发送失败！\r\n" + ee.ToString();
                throw new Exception(ee.ToString());
            }
        }
    }
}