using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.WebUI.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Dock
{
    /// <summary>
    /// 名称：24HSinglePollutantDataAnalyze.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-09-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：首页元件 单参数变化趋势分析
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class _24HSinglePollutantDataAnalyze : SmartEP.WebUI.Common.BasePage
    {
        //数据处理服务
        InfectantBy60Service m_InfectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
        AirPollutantService m_AirPollutantService = Singleton<AirPollutantService>.GetInstance();
        /// <summary>
        /// 站点服务
        /// </summary>
        MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        InitControl();
        //    }

        //}
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //Update By Lifei On 2015-10-22 Start
            //取得国控点
            IQueryable<MonitoringPointEntity> ports = m_MonitoringPointAirService.RetrieveAirMPListByCountryControlled();
            rcbPoint.DataValueField = "PointId";
            rcbPoint.DataTextField = "MonitoringPointName";
            rcbPoint.DataSource = ports;
            rcbPoint.DataBind();
            for (int i = 0; i < rcbPoint.Items.Count; i++)
            {
                rcbPoint.Items[i].Checked = true;
            }
            //Update By Lifei On 2015-10-22 end

            IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveListByCalAQI();
            DataTable dt = ConvertToDataTable(Pollutant);
            rcbFactors.DataSource = dt;
            rcbFactors.DataTextField = "PollutantName";
            rcbFactors.DataValueField = "PollutantCode";
            rcbFactors.DataBind();
            BindData();

            string strPointName = "";
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    strPointName += (item.Value.ToString() + ";");
                }
            }
            hdPointNames.Value = strPointName;
            hdFactorName.Value = rcbFactors.SelectedValue;
        }
        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(PollutantCodeEntity)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (PollutantCodeEntity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(PollutantCodeEntity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                if (Row["GBCode"].ToString() != "")
                {
                    dataTable.Rows.Add(Row);
                }
            }
            return dataTable;
        }
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowECharts(); ", true);
        }

        //绑定数据
        private void BindData()
        {
            //IList<IPoint> points = pointCbxRsm.GetPoints();//测点
            //string[] portIds = points.Select(t => t.PointID).ToArray();



            DateTime dtmEnd = DateTime.Now;
            DateTime dtmBegion = DateTime.Now.AddHours(-24);
            string Factor = rcbFactors.SelectedValue;
            SmartEP.Core.Interfaces.IPollutant Ifactor = m_AirPollutantService.GetPollutantInfo(Factor);
            string DecimalNum = Ifactor.PollutantDecimalNum;
            string[] factors = { Factor };
            int pageSize = int.MaxValue;//每页显示数据个数  
            int pageNo = 0;//当前页的序号
            int recordTotal = 0;//数据总行数 
            //绑定数据
            string portId = "";

            //Update By Lifei On 2015-10-22 Start
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    portId += (item.Value.ToString() + ",");
                }
            }
            //Update By Lifei On 2015-10-22 end

            string[] portIds = portId.Trim(',').Split(',');
            var analyzeDate = m_InfectantBy60Service.GetDataPager(portIds, factors, dtmBegion, dtmEnd, pageSize, pageNo, out recordTotal);
            DataTable dt = analyzeDate.ToTable();
            DataTable dtNew = new DataTable();
            DataTable dtpoint = new DataTable();
            dtNew.Columns.Add("DateTime", typeof(string));
            dtpoint.Columns.Add("pointId", typeof(string));
            dtpoint.Columns.Add("pointName", typeof(string));
            foreach (RadComboBoxItem item in rcbPoint.CheckedItems)
            {
                //IPoint strpoint = points.FirstOrDefault(x => x.PointID.Equals(point.Trim()));
                //string pointname = strpoint.PointName;
                DataRow dr = dtpoint.NewRow();
                dr[0] = item.Value;
                dr[1] = item.Text;
                dtpoint.Rows.Add(dr);
            }
            foreach (RadComboBoxItem item in rcbPoint.CheckedItems)
            {
                dtNew.Columns.Add(item.Value);
            }
            for (int j = 0; j < 24; j++)
            {
                DataRow drNew = dtNew.NewRow();
                dtNew.Rows.Add(drNew);
                DateTime dtTime = DateTime.Now.AddHours(-23 + j);
                string strTime = dtTime.ToString("yyyy-MM-dd HH:00:00");
                string strEndTime = dtTime.ToString("yyyy-MM-dd HH:59:59");
                if (dtTime.Hour == 0)
                {
                    drNew["DateTime"] = "24时";
                }
                else
                {
                    drNew["DateTime"] = dtTime.Hour + "时";
                }
                for (int i = 0; i < rcbPoint.CheckedItems.Count(); i++)
                {
                    DataRow[] rows = dt.Select("Tstamp>='" + strTime + "' and Tstamp<='" + strEndTime + "' and PointId='" + rcbPoint.CheckedItems[i].Value + "'");
                    foreach (DataRow row in rows)
                    {
                        if (row[Factor].ToString() != "")
                        {
                            drNew[rcbPoint.CheckedItems[i].Value] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row[Factor]), Convert.ToInt32(DecimalNum))).ToString(); ;
                        }
                        else
                        {
                            drNew[rcbPoint.CheckedItems[i].Value] = "-";
                        }
                    }
                }
                foreach (DataRow dr in dtNew.Rows)
                {
                    for (int i = 0; i < rcbPoint.CheckedItems.Count(); i++)
                    {
                        if (dr[rcbPoint.CheckedItems[i].Value].ToString() == "")
                        {
                            dr[rcbPoint.CheckedItems[i].Value] = "-";
                        }
                    }
                }
            }
            hdSinglePollutant.Value = ToJson(dtNew);
            hdpointiddata.Value = ToJson(dtpoint);
            string strPointName = "";
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    strPointName += (item.Value.ToString() + ";");
                }
            }
            hdPointNames.Value = strPointName;
            hdFactorName.Value = rcbFactors.SelectedValue;
        }


        /// <summary>   
        /// Datatable转换为Json   
        /// </summary>   
        /// <param name="table">Datatable对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

        /// <summary>  
        /// DataTable转成Json   
        /// </summary>  
        /// <param name="jsonName"></param>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (string.IsNullOrEmpty(str))
            {
                str = "null";
            }
            return str;
        }

        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        protected void rcbPoint_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string strPointName = "";
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    strPointName += (item.Value.ToString() + ";");
                }
            }
            hdPointNames.Value = strPointName;
        }

        protected void rcbFactors_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            hdFactorName.Value = rcbFactors.SelectedValue;
        }

    }
}