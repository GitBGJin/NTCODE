using SmartEP.Core.Generic;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class HourReportXML : BasePage
    {
        /// <summary>
        /// 60分钟数据服务层
        /// </summary>
        InfectantBy60Service g_InfectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
        string PointValue = "";
        string feilName = "";
        DateTime sertTime = DateTime.Now;
        DataTable ta;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            hourBegin.SelectedDate = DateTime.Now.AddDays(-1);
            hourEnd.SelectedDate = DateTime.Now;

        }
        #endregion
        #region RadGrid绑定数据源
        /// <summary>
        /// RadGrid绑定数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridXML_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();

        }
        #endregion
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            //每页显示数据个数            
            int pageSize = gridXML.PageSize;
            //当前页的序号
            int pageNo = gridXML.CurrentPageIndex + 1;

            //测点
            string pointId = "";
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    pointId += item.Value + ",";
                }
            }
            if (!string.IsNullOrWhiteSpace(pointId))
            {
                pointId = pointId.Substring(0, pointId.Length - 1);
            }

            string[] portIds = pointId.Split(',');
            DateTime dtBegin = hourBegin.SelectedDate.Value;
            DateTime dtEnd = hourEnd.SelectedDate.Value;
            DataView dv = g_InfectantBy60Service.GetDataLists(portIds, dtBegin, dtEnd);
            if (dv.Count > 0)
            {
                gridXML.DataSource = dv;
                gridXML.VirtualItemCount = dv.Count;
            }
            else
            {
                gridXML.DataSource = dv;
            }
        }
        #endregion

        #region 查询按钮事件
        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridXML.Rebind();
        }
        #endregion

        protected void btnBTF_Click(object sender, EventArgs e)
        {
            string pf = System.Configuration.ConfigurationManager.AppSettings["pointId"].ToString();
            string[] pfs = pf.Trim(';').Split(';');
            string pointIds = "";
            DateTime dtBegin = hourBegin.SelectedDate.Value;
            DateTime dtEnd = hourEnd.SelectedDate.Value;
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    pointIds += item.Value + ",";
                }
            }
            GetOridinalData(pfs, pointIds, dtBegin, dtEnd);

        }

        #region 根据站点获取原始小时数据
        public void GetOridinalData(String[] PointId, string pointIds, DateTime dtBegin, DateTime dtend)
        {
            try
            {
                string parameterPwd = System.Configuration.ConfigurationManager.AppSettings["pointId"].ToString();
                string[] factors = parameterPwd.Split(';');

                if (PointId.Length > 0)
                {
                    //上报文件路径
                    String dataFliePath = ConfigurationManager.AppSettings["xmlFilePath"].ToString();

                    //如果不存在则创建文件夹
                    if (!Directory.Exists(dataFliePath))
                        Directory.CreateDirectory(dataFliePath);

                    DateTime beginTime = Convert.ToDateTime(dtBegin.ToString("yyyy-MM-dd HH:mm:ss"));
                    DateTime endTime = Convert.ToDateTime(dtend.ToString("yyyy-MM-dd HH:mm:ss"));

                    foreach (string str in PointId)
                    {
                        string[] s = str.Split(':');
                        if (s.Length > 0)
                        {
                            string point = s[0];
                            if (pointIds.Contains(point))
                            {
                                string[] factor = s[1].Split(',');


                                int poor = 0;

                                TimeSpan ts = endTime.Subtract(beginTime);
                                poor = Convert.ToInt32((ts.TotalHours));

                                for (; poor >= 0; poor--)
                                {
                                    dt = g_InfectantBy60Service.GetDataList(point, endTime.AddHours(-poor)).ToTable();
                                    if (dt.Rows.Count <= 0)
                                    {
                                        string pointName = "";
                                        string jsfid = "";
                                        switch (int.Parse(point))
                                        {
                                            case 29:
                                                pointName = "东山";
                                                jsfid = "AQMS52100222";
                                                break;
                                            case 28:
                                                pointName = "方洲公园";
                                                jsfid = "AQMS52100209";
                                                break;
                                            case 27:
                                                pointName = "昆山花桥";
                                                jsfid = "AQMS52100231";
                                                break;
                                            case 26:
                                                pointName = "文昌中学";
                                                jsfid = "AQMS52100220";
                                                break;
                                            case 33:
                                                pointName = "拙政园";
                                                jsfid = "AQMS52100204";
                                                break;
                                            case 168:
                                                pointName = "香山站";
                                                jsfid = "AQMS52100221";
                                                break;
                                            case 179:
                                                pointName = "东南开发区子站";
                                                jsfid = "AQMS52100223";
                                                break;
                                            case 180:
                                                pointName = "氟化工业园";
                                                jsfid = "AQMS52100224";
                                                break;
                                            case 181:
                                                pointName = "沿江开发区";
                                                jsfid = "AQMS52100225";
                                                break;
                                            case 176:
                                                pointName = "乐余广电站";
                                                jsfid = "AQMS52100226";
                                                break;
                                            case 25:
                                                pointName = "张家港农业示范园";
                                                jsfid = "AQMS52100227";
                                                break;
                                            case 177:
                                                pointName = "托普学院";
                                                jsfid = "AQMS52100228";
                                                break;
                                            case 178:
                                                pointName = "淀山湖党校";
                                                jsfid = "AQMS52100229";
                                                break;
                                            case 172:
                                                pointName = "太仓三水厂";
                                                jsfid = "AQMS52100232";
                                                break;
                                            case 173:
                                                pointName = "太仓气象观测站";
                                                jsfid = "AQMS52100233";
                                                break;
                                            case 174:
                                                pointName = "双凤生态园";
                                                jsfid = "AQMS52100234";
                                                break;
                                            case 175:
                                                pointName = "荣文学校";
                                                jsfid = "AQMS52100235";
                                                break;
                                            case 184:
                                                pointName = "青剑湖";
                                                jsfid = "AQMS52100217";
                                                break;
                                            case 182:
                                                pointName = "苏州大学高教区";
                                                jsfid = "AQMS52100219";
                                                break;
                                            case 183:
                                                pointName = "东部工业区";
                                                jsfid = "AQMS52100218";
                                                break;
                                        }

                                        string FileName = dataFliePath;
                                        if (!Directory.Exists(FileName)) Directory.CreateDirectory(FileName); //如果不存在则创建文件夹

                                        ta = g_InfectantBy60Service.GetHourDatas(point, factor, endTime.AddHours(-poor), endTime.AddHours(-poor)).ToTable();
                                        if (ta.Rows.Count > 0)
                                        {
                                            XmlDocument xmlDoc = new XmlDocument();
                                            //创建根节点 
                                            XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
                                            xmlDoc.AppendChild(dec);

                                            XmlElement Body = xmlDoc.CreateElement("Body");
                                            xmlDoc.AppendChild(Body);

                                            //节点及元素
                                            XmlNode Head = xmlDoc.CreateElement("Head");
                                            Body.AppendChild(Head);

                                            string scsj = endTime.AddHours(-poor).ToString("yyyyMMddHHmmss");
                                            string sxh = "";
                                            string js = "";
                                            int hour = endTime.AddHours(-poor + 1).Hour;
                                            if (hour == 0)
                                            {
                                                sxh = "0024";
                                            }
                                            else
                                            {
                                                sxh = hour.ToString("0000");
                                            }


                                            //文件名
                                            String DBFFileName = "AQMS_0001_" + scsj + "_" + sxh + "_" + jsfid + ".xml";
                                            string StateUploadName = "\\" + DBFFileName;
                                            XmlAttribute fsfid = xmlDoc.CreateAttribute("fsfid");
                                            XmlAttribute jcsb = xmlDoc.CreateAttribute("jcsb");
                                            XmlAttribute jls = xmlDoc.CreateAttribute("jls");
                                            XmlAttribute id = xmlDoc.CreateAttribute("jsfid");
                                            XmlAttribute scsjs = xmlDoc.CreateAttribute("scsj");
                                            XmlAttribute sjblx = xmlDoc.CreateAttribute("sjblx");
                                            XmlAttribute sjcssj = xmlDoc.CreateAttribute("sjcssj");
                                            fsfid.Value = jsfid;
                                            jcsb.Value = "";
                                            jls.Value = ta.Rows.Count.ToString();
                                            id.Value = jsfid;
                                            scsjs.Value = scsj;
                                            sjblx.Value = "0001";
                                            sjcssj.Value = endTime.AddHours(-poor).ToString("yyyyMMddHHmmss");
                                            Head.Attributes.Append(fsfid);
                                            Head.Attributes.Append(jcsb);
                                            Head.Attributes.Append(jls);
                                            Head.Attributes.Append(id);
                                            Head.Attributes.Append(scsjs);
                                            Head.Attributes.Append(sjblx);
                                            Head.Attributes.Append(sjcssj);


                                            var query = from t in ta.AsEnumerable()
                                                        group t by new { t1 = t.Field<DateTime>("Tstamp") } into m
                                                        select new
                                                        {
                                                            Tstamp = m.Key.t1,
                                                        };
                                            if (query.ToList().Count > 0)
                                            {
                                                query.ToList().ForEach(q =>
                                                {
                                                    XmlNode Data = xmlDoc.CreateElement("Data");
                                                    Body.AppendChild(Data);
                                                    DataRow[] drs = ta.Select("Tstamp='" + q.Tstamp.ToString() + "'");

                                                    foreach (DataRow dr in drs)
                                                    {
                                                        XmlNode Item = xmlDoc.CreateElement("Item");
                                                        Data.AppendChild(Item);

                                                        XmlAttribute Code = xmlDoc.CreateAttribute("pollutant_code");
                                                        Code.Value = dr["PollutantName"].ToString();
                                                        XmlAttribute Status = xmlDoc.CreateAttribute("equip_status");
                                                        Status.Value = dr["Status"].ToString();
                                                        XmlAttribute Value = xmlDoc.CreateAttribute("value");
                                                        if (dr["PollutantValue"].ToString() != "")
                                                            Value.Value = Math.Round(decimal.Parse(dr["PollutantValue"].ToString()), int.Parse(dr["DecimalDigit"].ToString())).ToString();
                                                        else
                                                            Value.Value = dr["PollutantValue"].ToString();
                                                        Item.Attributes.Append(Code);
                                                        Item.Attributes.Append(Status);
                                                        Item.Attributes.Append(Value);
                                                    }
                                                });
                                            }
                                            xmlDoc.Save(FileName + StateUploadName);
                                            g_InfectantBy60Service.GetAddData(point, jsfid, DBFFileName, Convert.ToDateTime((endTime.AddHours(-poor)).ToString("yyyy-MM-dd HH:00:00")), endTime.AddHours(-poor), sxh, ta.Rows.Count, 0);
                                        }

                                    }
                                    else
                                    {
                                        string pointName = "";
                                        string jsfid = "";
                                        switch (int.Parse(point))
                                        {
                                            case 29:
                                                pointName = "东山";
                                                jsfid = "AQMS52100222";
                                                break;
                                            case 28:
                                                pointName = "方洲公园";
                                                jsfid = "AQMS52100209";
                                                break;
                                            case 27:
                                                pointName = "昆山花桥";
                                                jsfid = "AQMS52100231";
                                                break;
                                            case 26:
                                                pointName = "文昌中学";
                                                jsfid = "AQMS52100220";
                                                break;
                                            case 33:
                                                pointName = "拙政园";
                                                jsfid = "AQMS52100204";
                                                break;
                                            case 168:
                                                pointName = "香山站";
                                                jsfid = "AQMS52100221";
                                                break;
                                            case 179:
                                                pointName = "东南开发区子站";
                                                jsfid = "AQMS52100223";
                                                break;
                                            case 180:
                                                pointName = "氟化工业园";
                                                jsfid = "AQMS52100224";
                                                break;
                                            case 181:
                                                pointName = "沿江开发区";
                                                jsfid = "AQMS52100225";
                                                break;
                                            case 176:
                                                pointName = "乐余广电站";
                                                jsfid = "AQMS52100226";
                                                break;
                                            case 25:
                                                pointName = "张家港农业示范园";
                                                jsfid = "AQMS52100227";
                                                break;
                                            case 177:
                                                pointName = "托普学院";
                                                jsfid = "AQMS52100228";
                                                break;
                                            case 178:
                                                pointName = "淀山湖党校";
                                                jsfid = "AQMS52100229";
                                                break;
                                            case 172:
                                                pointName = "太仓三水厂";
                                                jsfid = "AQMS52100232";
                                                break;
                                            case 173:
                                                pointName = "太仓气象观测站";
                                                jsfid = "AQMS52100233";
                                                break;
                                            case 174:
                                                pointName = "双凤生态园";
                                                jsfid = "AQMS52100234";
                                                break;
                                            case 175:
                                                pointName = "荣文学校";
                                                jsfid = "AQMS52100235";
                                                break;
                                            case 184:
                                                pointName = "青剑湖";
                                                jsfid = "AQMS52100217";
                                                break;
                                            case 182:
                                                pointName = "苏州大学高教区";
                                                jsfid = "AQMS52100219";
                                                break;
                                            case 183:
                                                pointName = "东部工业区";
                                                jsfid = "AQMS52100218";
                                                break;
                                        }
                                        string FileName = dataFliePath;
                                        if (!Directory.Exists(FileName)) Directory.CreateDirectory(FileName); //如果不存在则创建文件夹

                                        ta = g_InfectantBy60Service.GetHourDatas(point, factor, endTime.AddHours(-poor), endTime.AddHours(-poor)).ToTable();
                                        if (ta.Rows.Count > 0)
                                        {
                                            XmlDocument xmlDoc = new XmlDocument();
                                            //创建根节点 
                                            XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
                                            xmlDoc.AppendChild(dec);

                                            XmlElement Body = xmlDoc.CreateElement("Body");
                                            xmlDoc.AppendChild(Body);

                                            //节点及元素
                                            XmlNode Head = xmlDoc.CreateElement("Head");
                                            Body.AppendChild(Head);

                                            string scsj = endTime.AddHours(-poor).ToString("yyyyMMddHHmmss");
                                            string sxh = "";
                                            string js = "";
                                            int hour = endTime.AddHours(-poor + 1).Hour;
                                            if (hour == 0)
                                            {
                                                sxh = "0024";
                                            }
                                            else
                                            {
                                                sxh =hour.ToString("0000");
                                            }


                                            //文件名
                                            String DBFFileName = "AQMS_0001_" + scsj + "_" + sxh + "_" + jsfid + ".xml";
                                            string StateUploadName = "\\" + DBFFileName;
                                            XmlAttribute fsfid = xmlDoc.CreateAttribute("fsfid");
                                            XmlAttribute jcsb = xmlDoc.CreateAttribute("jcsb");
                                            XmlAttribute jls = xmlDoc.CreateAttribute("jls");
                                            XmlAttribute id = xmlDoc.CreateAttribute("jsfid");
                                            XmlAttribute scsjs = xmlDoc.CreateAttribute("scsj");
                                            XmlAttribute sjblx = xmlDoc.CreateAttribute("sjblx");
                                            XmlAttribute sjcssj = xmlDoc.CreateAttribute("sjcssj");
                                            fsfid.Value = jsfid;
                                            jcsb.Value = "";
                                            jls.Value = ta.Rows.Count.ToString();
                                            id.Value = jsfid;
                                            scsjs.Value = scsj;
                                            sjblx.Value = "0001";
                                            sjcssj.Value = endTime.AddHours(-poor).ToString("yyyyMMddHHmmss");
                                            Head.Attributes.Append(fsfid);
                                            Head.Attributes.Append(jcsb);
                                            Head.Attributes.Append(jls);
                                            Head.Attributes.Append(id);
                                            Head.Attributes.Append(scsjs);
                                            Head.Attributes.Append(sjblx);
                                            Head.Attributes.Append(sjcssj);


                                            var query = from t in ta.AsEnumerable()
                                                        group t by new { t1 = t.Field<DateTime>("Tstamp") } into m
                                                        select new
                                                        {
                                                            Tstamp = m.Key.t1,
                                                        };
                                            if (query.ToList().Count > 0)
                                            {
                                                query.ToList().ForEach(q =>
                                                {
                                                    XmlNode Data = xmlDoc.CreateElement("Data");
                                                    Body.AppendChild(Data);
                                                    DataRow[] drs = ta.Select("Tstamp='" + q.Tstamp.ToString() + "'");

                                                    foreach (DataRow dr in drs)
                                                    {
                                                        XmlNode Item = xmlDoc.CreateElement("Item");
                                                        Data.AppendChild(Item);

                                                        XmlAttribute Code = xmlDoc.CreateAttribute("pollutant_code");
                                                        Code.Value = dr["PollutantName"].ToString();
                                                        XmlAttribute Status = xmlDoc.CreateAttribute("equip_status");
                                                        Status.Value = dr["Status"].ToString();
                                                        XmlAttribute Value = xmlDoc.CreateAttribute("value");
                                                        if (dr["PollutantValue"].ToString() != "")
                                                            Value.Value = Math.Round(decimal.Parse(dr["PollutantValue"].ToString()), int.Parse(dr["DecimalDigit"].ToString())).ToString();
                                                        else
                                                            Value.Value = dr["PollutantValue"].ToString();
                                                        Item.Attributes.Append(Code);
                                                        Item.Attributes.Append(Status);
                                                        Item.Attributes.Append(Value);
                                                    }
                                                });
                                            }
                                            File.Delete(FileName + StateUploadName);
                                            xmlDoc.Save(FileName + StateUploadName);
                                            g_InfectantBy60Service.GetUpdateData(point, Convert.ToDateTime((endTime.AddHours(-poor)).ToString("yyyy-MM-dd HH:00:00")), ta.Rows.Count);
                                        }

                                    }


                                }
                            }
                        }
                    }

                }
                Alert("生成XML成功！");
                gridXML.Rebind();
            }
            catch (Exception ex)
            {
                WriteTextLog("生成XML", ex.Message, DateTime.Now);
            }
        }
        #endregion
        private XmlElement GetXmlElement(XmlDocument doc, string elementName, string value)
        {
            XmlElement element = doc.CreateElement(elementName);
            element.InnerText = value;
            return element;
        }
        /// <summary>  
        /// 写入日志到文本文件  
        /// </summary>  
        /// <param name="action">动作</param>  
        /// <param name="strMessage">日志内容</param>  
        /// <param name="time">时间</param>  
        public static void WriteTextLog(string action, string strMessage, DateTime time)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"System\Log\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".System.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + time.ToString() + "\r\n");
            str.Append("Action:  " + action + "\r\n");
            str.Append("Message: " + strMessage + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
    }
}