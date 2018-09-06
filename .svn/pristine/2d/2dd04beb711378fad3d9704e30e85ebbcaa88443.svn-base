using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Messaging;
using SmartEP.Utilities.AdoData;
using SmartEP.Core.Interfaces;
using Telerik.Web.UI;
using CustomFilterCustomEditors = SmartEP.WebUI.Pages.EnvAir.RemoteControl.FilterCustomEditors;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.DomainModel.AirAutoMonitoring;
using System.Text.RegularExpressions;

namespace SmartEP.WebUI.Pages.EnvAir.RemoteControl
{
    public partial class RemoteControl2 : SmartEP.WebUI.Common.BasePage
    {

        DataView dvPoint = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string strCN = ViewState["CN"].ToString();
                DataView dv = (DataView)ViewState["dv"];
                if (dv.Count > 0)
                {
                    bindExpression(dv, strCN);

                    RadFilter1.RecreateControl();
                    RadFilter1.FireApplyCommand();
                }
            }
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.DesignMode)
            {
                ViewState["CN"] = Request.QueryString["cn"];
                ViewState["cmdDesc"] = Request.QueryString["cmdDesc"];
                string strCN = "1011";
                if (ViewState["CN"] != null && ViewState["CN"].ToString() != "")
                    strCN = ViewState["CN"].ToString();
                else
                    ViewState["CN"] = strCN;

                string strCmdDesc = "1011提取现场机时间";
                if (ViewState["cmdDesc"] != null && ViewState["cmdDesc"].ToString() != "")
                    strCmdDesc = ViewState["cmdDesc"].ToString();
                else
                    ViewState["cmdDesc"] = strCmdDesc;

                string strSql = " SELECT * FROM V_Command_Column_CommandColumn where 1=1 ";
                string strWhere = " and commandMode='8A8393DD-5129-4715-ABD8-3FEB5E8770E3' and parentGuid is not null ";
                strWhere += " and commandNumber='" + strCN + "'";
                string strOrderBy = "order by commandNumber,orderNumber";

                DataView dv = new DatabaseHelper().ExecuteDataView(strSql + strWhere + strOrderBy, "AMS_BaseDataConnection");
                ViewState["dv"] = dv;
                if (dv.Count > 0)
                {
                    btnDownload.Enabled = true;
                    RadFilter1.Enabled = true;
                    bindFieldEditors(dv);
                }
                else
                {
                    btnDownload.Enabled = false;
                    RadFilter1.Enabled = false;
                }
            }
        }

        #region 控件遍历

        /// <summary>
        /// 过滤不需要显示的控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadFilter1_PreRender(object sender, EventArgs e)
        {
            ////隐藏指定的RootGroupItem“添加组”按钮
            //RadFilter1.RootGroupItem.Controls[0].Controls[2].Controls[1].Visible = false;
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
                }
            }
        }

        #endregion

        #region 绑定控件对应表达式

        /// <summary>
        /// 绑定具体命令参数
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="strCN">命令号</param>
        protected void bindExpression(DataView dv, string strCN)
        {
            string strMN = "33333333333333", strSql = "";

            RadFilter1.RootGroup.Expressions.Clear();

            Session["QN"] = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            addRootGroupExpression("QN", Session["QN"].ToString());
            addRootGroupExpression("ST", "22");//地表水质监测21;空气质量监测22;区域环境噪声监测23;大气环境污染源31;地表水质污染源32
            addRootGroupExpression("CN", strCN);
            addRootGroupExpression("MN", strMN);

            dv.RowFilter = "key='PW'";
            if (dv.Count > 0)
                addRootGroupExpression("PW", "123456");

            dv.RowFilter = "key='Flag'";
            if (dv.Count > 0)
                addRootGroupExpression("Flag", "3");

            RadFilterGroupExpression group = new RadFilterGroupExpression();

            dv.RowFilter = "key='CP'";
            if (dv.Count > 0)
            {
                group.GroupOperation = RadFilterGroupOperation.Or;
                RadFilter1.RootGroup.AddExpression(group);
            }
            foreach (DataRowView drv in dv)
            {
                dv.RowFilter = "parentGuid='" + drv["protocolGuid"].ToString() + "'";

                foreach (DataRowView drvFilter in dv)
                    addGroupExpression(drvFilter["commandName"].ToString(), group, drvFilter["key"].ToString(), drvFilter["columnType"].ToString());
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
                //case "datetime":
                //    RadFilterEqualToFilterExpression<DateTime> dtExpression = new RadFilterEqualToFilterExpression<DateTime>(key);
                //    group.AddExpression(dtExpression);
                //    dtExpression.Value = DateTime.Now.AddDays(-1);
                //    break;
                //case "int":
                //    RadFilterEqualToFilterExpression<int> intExpression = new RadFilterEqualToFilterExpression<int>(key);
                //    group.AddExpression(intExpression);
                //    break;
                //case "bool":
                //    RadFilterEqualToFilterExpression<bool> boolExpression = new RadFilterEqualToFilterExpression<bool>(key);
                //    group.AddExpression(boolExpression);
                //    break;
                //default:
                //    RadFilterEqualToFilterExpression<string> strExpression = new RadFilterEqualToFilterExpression<string>(key);
                //    group.AddExpression(strExpression);
                //    break;
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

        #endregion

        #region 绑定Filter中控件类型

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
                RadFilterDropDownEditor rfd = new RadFilterDropDownEditor()
                {
                    DisplayName = keyName,
                    FieldName = key,

                    DataTextField = "monitoringPointName",
                    DataValueField = "MN"
                };

                RadFilter1.FieldEditors.Add(rfd);
                return;
            }

            if (key.Equals("CalName"))
            {
                RadFilterDropDownEditor rfddl = new RadFilterDropDownEditor()
                    {
                        DisplayName = commandName.Equals("校零") ? keyName + "1" : keyName + "2",
                        FieldName = key,
                        DataTextField = key,
                        DataValueField = "id"
                    };

                RadFilter1.FieldEditors.Add(rfddl);

                //FilterCustomEditors.RadFilterDropDownEditor rfd = new FilterCustomEditors.RadFilterDropDownEditor()
                //{
                //    DisplayName = commandName.Equals("校零") ? keyName + "1" : keyName + "2",
                //    FieldName = key,
                //    //DataSource = dv.ToTable(),
                //    DataTextField = key,
                //    DataValueField = "id"
                //};
                //RadFilter1.FieldEditors.Add(rfd);
                return;
            }
            if (key.Equals("Gas"))
            {

                RadFilterDropDownEditor rfddl = new RadFilterDropDownEditor()
                {
                    DisplayName = commandName.Equals("校零") ? keyName + "1" : keyName + "2",
                    FieldName = key,
                    DataTextField = key,
                    DataValueField = key
                };

                RadFilter1.FieldEditors.Add(rfddl);
                //FilterCustomEditors.RadFilterDropDownEditor rfd = new FilterCustomEditors.RadFilterDropDownEditor()
                //{
                //    DisplayName = commandName.Equals("校零") ? keyName + "1" : keyName + "2",
                //    FieldName = key,
                //    //DataSource = dv.ToTable(),
                //    DataTextField = key,
                //    DataValueField = key
                //};
                //RadFilter1.FieldEditors.Add(rfd);
                return;
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

        #endregion

        public IEnumerable<T> ControlsOfType<T>(Control parent) where T : class
        {
            foreach (Control control in parent.Controls)
            {
                if (control is T)
                {
                    yield return control as T;
                    continue;
                }
                foreach (T descendant in ControlsOfType<T>(control))
                {
                    yield return descendant;
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            string strMN = "";
            string strFormat = "", strCP = "", strTemp = "";
            RadFilter1.RecreateControl();
            RadFilter1.FireApplyCommand();

            RadFilterSqlQueryProvider provider = new RadFilterSqlQueryProvider();
            provider.ProcessGroup(RadFilter1.RootGroup);

            //遍历DropDownList控件，获取MN号
            RadFilterNonGroupExpression nonGroupExpression = RadFilter1.RootGroup.FindByFieldName("MN");
            RadFilterEqualToFilterExpression<string> expression = nonGroupExpression as RadFilterEqualToFilterExpression<string>;
            strMN = expression.Value.Trim();
            //dvPoint.RowFilter = "MN='" + strMN + "'";
            //if (dvPoint.Count > 0)
            //    strMN += dvPoint[0]["monitoringPointName"].ToString().Trim();

            strFormat = provider.Result.ToString().TrimStart('(').TrimEnd(')').Replace("[", "").Replace("]", "").Replace("'", "").Replace("AND", ";").Replace("OR", ",");
            strTemp = strFormat;
            strFormat = strFormat.Replace("(", "CP=&&");

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
            //string outputData = MyCoder.TextToAscII_16(strCmdContent);

            SendMSMQCommands(strCmdContent, strMN, txtQueryProvider);

            //记录反控命令
            IBaseEntityProperty originalPacketRequestEntity = new OriginalPacketRequestEntity()
            {
                Mn = strMN,
                Qn = Session["QN"].ToString(),
                Cn = ViewState["CN"].ToString(),
                CmdDesc = ViewState["cmdDesc"].ToString(),
                CmdContent = strCmdContent,
                Operater = "web",
                OperaterTime = DateTime.Now,
                ReqState = "等待请求应答",
                IsFinished = false,
                Flag = "0"
            };
            new OriginalPacketRequestService().Save(originalPacketRequestEntity);

            DataView dv = null;
            bool isSuccess = false;
            int intSleeps = 0;
            string strSql = " SELECT top 10 * FROM TB_OriginalPacketBackup where 1=1 ";
            strSql += " and receiveTime>='" + DateTime.Now.AddMinutes(-5).ToString() + "' and cmdContent like '%QN=" + Session["QN"].ToString() + "%' ORDER BY receiveTime ";


            while (!isSuccess && intSleeps <= 10000)
            {
                if (Page.IsPostBack)
                {
                    System.Threading.Thread.Sleep(5000);
                    intSleeps += 5000;
                }

                //在接收原始表中过滤反控确认包（性能差，需要修改）
                dv = new DatabaseHelper().ExecuteDataView(strSql, "AMS_AirAutoMonitorConnection");
                //dv = OriginalPacketSendBiz.RetrieveList(strWhere);
                if (dv.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < dv.Count; i++)
                        {
                            string Content = dv[i]["cmdContent"].ToString();
                            string[] cmdContents = Regex.Split(Content, "&&", RegexOptions.IgnoreCase);
                            string value = cmdContents[1];
                            string[] items = value.Split(';');
                            if (items[1].Contains("QnRtn"))
                            {
                                string[] item = items[1].Split('=');
                                if (item[1].Equals("1"))
                                {
                                    string strUpdateSql = "UPDATE dbo.TB_OriginalPacketRequest SET reqState='请求成功' WHERE 1=1 ";
                                    strUpdateSql += " and  QN='" + Session["QN"].ToString() + "'";
                                    new DatabaseHelper().ExecuteNonQuery(strUpdateSql, "AMS_AirAutoMonitorConnection");
                                    isSuccess = true;
                                }
                                else
                                {
                                    string strUpdateSql = "UPDATE dbo.TB_OriginalPacketRequest SET reqState='请求失败' WHERE 1=1 ";
                                    strUpdateSql += " and  QN='" + Session["QN"].ToString() + "'";
                                    new DatabaseHelper().ExecuteNonQuery(strUpdateSql, "AMS_AirAutoMonitorConnection");
                                    isSuccess = false;
                                }
                            }
                            else if (items[1].Contains("ExeRtn"))
                            {
                                string[] item = items[1].Split('=');
                                if (item[1].Equals("1"))
                                {
                                    string strUpdateSql = "UPDATE dbo.TB_OriginalPacketRequest SET execState='执行成功',isFinished=1 WHERE 1=1 ";
                                    strUpdateSql += " and  QN='" + Session["QN"].ToString() + "'";
                                    new DatabaseHelper().ExecuteNonQuery(strUpdateSql, "AMS_AirAutoMonitorConnection");
                                    isSuccess = true;
                                }
                                else
                                {
                                    string strUpdateSql = "UPDATE dbo.TB_OriginalPacketRequest SET execState='执行失败',isFinished=1 WHERE 1=1 ";
                                    strUpdateSql += " and  QN='" + Session["QN"].ToString() + "'";
                                    new DatabaseHelper().ExecuteNonQuery(strUpdateSql, "AMS_AirAutoMonitorConnection");
                                    isSuccess = false;
                                }
                            }
                            else
                            {
                                string v = items[1];
                                string strUpdateSql = "UPDATE dbo.TB_OriginalPacketRequest SET Description='" + v + "' WHERE 1=1 ";
                                strUpdateSql += " and  QN='" + Session["QN"].ToString() + "'";
                                new DatabaseHelper().ExecuteNonQuery(strUpdateSql, "AMS_AirAutoMonitorConnection");
                                isSuccess = true;
                            }
                        }
                    }
                    catch
                    {
                        isSuccess = false;
                    }
                }
                else
                    isSuccess = false;

            }
            ViewState["isValid"] = "Valid";
            try
            {
                if (isSuccess)
                {
                    txtQueryProvider.Text += " <<<< 应答" + ViewState["cmdDesc"] + "命令:\r\n";
                    txtQueryProvider.Text += dv[0]["cmdContent"].ToString() + "\r\n";
                    txtQueryProvider.Text += " >>>> 请求成功！\r\n";
                    txtQueryProvider.Text += "**************" + DateTime.Now.ToString() + "**************\r\n\r\n";

                    txtQueryProvider.Text += " <<<< 命令反馈" + ViewState["cmdDesc"] + "命令:\r\n";
                    txtQueryProvider.Text += dv[1]["cmdContent"].ToString() + "\r\n";
                    txtQueryProvider.Text += " >>>> 反馈成功！\r\n";
                    txtQueryProvider.Text += "**************" + DateTime.Now.ToString() + "**************\r\n\r\n";

                    txtQueryProvider.Text += " <<<< 执行" + ViewState["cmdDesc"] + "命令:\r\n";
                    txtQueryProvider.Text += dv[2]["cmdContent"].ToString() + "\r\n";
                    txtQueryProvider.Text += " >>>> 执行成功！\r\n";
                    txtQueryProvider.Text += "**************" + DateTime.Now.ToString() + "**************\r\n\r\n";

                    //if (new OriginalPacketRequestService().IsExit(Session["QN"].ToString()))
                    //{
                    //    string strUpdateSql = "UPDATE dbo.TB_OriginalPacketRequest SET execState='请求成功',isFinished=1 WHERE 1=1 ";
                    //    strUpdateSql += " and  QN='" + Session["QN"].ToString() + "'";
                    //    new DatabaseHelper().ExecuteNonQuery(strUpdateSql, "AMS_AirAutoMonitorConnection");
                    //}
                }
                else
                {
                    txtQueryProvider.Text += " >>>> 请求超时！请重新操作！\r\n";
                    txtQueryProvider.Text += "**************" + DateTime.Now.ToString() + "**************\r\n\r\n";

                    //if (new OriginalPacketRequestService().IsExit(Session["QN"].ToString()))
                    //{
                    //    string strUpdateSql = "UPDATE dbo.TB_OriginalPacketRequest SET execState='请求超时',isFinished=0 WHERE 1=1 ";
                    //    strUpdateSql += "  and QN='" + Session["QN"].ToString() + "'";
                    //    new DatabaseHelper().ExecuteNonQuery(strUpdateSql, "AMS_AirAutoMonitorConnection");
                    //}
                }
            }
            catch
            {
                txtQueryProvider.Text += " >>>> 请求超时！请重新操作！\r\n";
                txtQueryProvider.Text += "**************" + DateTime.Now.ToString() + "**************\r\n\r\n";
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
                MSMQPath = ConfigurationManager.AppSettings["MSMQEnvAir"];
                MessageQueue myMq = new MessageQueue(MSMQPath);
                myMq.Send(strCmdContent, strMN, System.Messaging.MessageQueueTransactionType.Automatic);
            }
            catch (Exception ee)
            {
                txt.Text += " >>>> 命令发送失败！请正确配置使用的消息队列路径！\r\n";
                throw new Exception("请正确配置使用的消息队列路径！");
            }
        }

        protected void RadFilter1_ExpressionItemCreated(object sender, RadFilterExpressionItemCreatedEventArgs e)
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
                RadDropDownList dropDownList = singleItem.InputControl as RadDropDownList;
                //未验证用户权限
                string Sql = "SELECT  PointId, MN, MonitoringPointName FROM  V_Point_Acquisition";
                Sql += " where PointId in(" + ConfigurationManager.AppSettings["RemoteAirPoint"].ToString() + ") order by PointId";
                dvPoint = new DatabaseHelper().ExecuteDataView(Sql, "AMS_BaseDataConnection");

                dropDownList.DataSource = dvPoint;
                dropDownList.DataBind();
            }
            else if (singleItem != null && singleItem.FieldName.Equals("CalName") && singleItem.IsSingleValue)
            {
                //RadFilterEqualToFilterExpression<string> strExpression = singleItem.Expression as RadFilterEqualToFilterExpression<string>;
                string Sql = "";
                if (singleItem.FieldNameChooserLink.Text.Contains("1"))
                {
                    Sql = string.Format(@"SELECT 1 as id, '设备全校零'  AS CalName
                                             UNION SELECT 2 as id, 'CO校零检查'  AS CalName
                                             UNION SELECT 3 as id, 'SO2校零检查'  AS CalName 
                                            UNION SELECT 4 as id, 'NO校零检查'  AS CalName                                            
                                            UNION SELECT 5 as id, 'O3校零检查'  AS CalName");
                }
                else
                {
                    Sql = string.Format(@" SELECT 8 as id, 'CO内跨度检查'  AS CalName
                                            UNION SELECT 11 as id, 'N0内跨度检查'  AS CalName
                                            UNION SELECT 14 as id, 'SO2内跨度检查'  AS CalName                                                                                    
                                            UNION SELECT 17 as id, 'O3内跨度检查'  AS CalName");
                }

                DataView dv = new DatabaseHelper().ExecuteDataView(Sql, "AMS_BaseDataConnection");
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
                string Sql = "";
                if (singleItem.FieldNameChooserLink.Text.Contains("1"))
                {
                    Sql = string.Format(@"SELECT 1 as id, 'Zero'  AS Gas");
                }
                else
                {
                    Sql = string.Format(@"SELECT 2 as id,  'CO' AS Gas 
                                            UNION SELECT 3 as id, 'NO'  AS Gas
                                            UNION SELECT 4 as id, 'SO2'  AS Gas
                                            UNION SELECT 5 as id, 'O3'  AS Gas");
                }

                DataView dv = new DatabaseHelper().ExecuteDataView(Sql, "AMS_BaseDataConnection");
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

        protected void RadFilter1_FieldEditorCreating(object sender, RadFilterFieldEditorCreatingEventArgs e)
        {
            if (e.EditorType == "RadFilterDropDownEditor")
            {
                e.Editor = new Telerik.Web.UI.RadFilterDropDownEditor();
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
                        RadNumericTextBox calconc = singleItem.InputControl as RadNumericTextBox;
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

    }
}