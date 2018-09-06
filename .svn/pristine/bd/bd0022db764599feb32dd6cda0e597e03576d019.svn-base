using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.WebUI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.RealTimeData
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
        HourAQIService m_HourAQIService = Singleton<HourAQIService>.GetInstance();

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
            BindData();
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
            IList<IPoint> points = pointCbxRsm.GetPoints();//测点
            string[] portIds = points.Select(t => t.PointID).ToArray();
            DateTime dtmEnd = DateTime.Now;
            DateTime dtmBegion = DateTime.Now.AddHours(-24);
            string Factor = rcbFactors.SelectedValue;
            int pageSize = int.MaxValue;//每页显示数据个数  
            int pageNo = 0;//当前页的序号
            int recordTotal = 0;//数据总行数 
            //绑定数据
            var analyzeDate = m_HourAQIService.GetPortDataPager(portIds, dtmBegion, dtmEnd, pageSize, pageNo, out recordTotal);
            DataTable dt = analyzeDate.ToTable();
            DataTable dtNew = new DataTable();
            DataTable dtpoint = new DataTable();
            dtNew.Columns.Add("DateTime", typeof(DateTime));
            dtpoint.Columns.Add("pointId", typeof(string));
            dtpoint.Columns.Add("pointName", typeof(string));
            foreach (string point in portIds)
            {
                IPoint strpoint = points.FirstOrDefault(x => x.PointID.Equals(point.Trim()));
                string pointname = strpoint.PointName;
                DataRow dr = dtpoint.NewRow();
                dr[0] = point;
                dr[1] = pointname;
                dtpoint.Rows.Add(dr);
            }
            foreach (string pointId in portIds)
            {
                dtNew.Columns.Add(pointId);
            }
            for (int j = 0; j < 24; j++)
            {
                DataRow drNew = dtNew.NewRow();
                dtNew.Rows.Add(drNew);
                string strTime = DateTime.Now.AddHours(-23 + j).ToString("yyyy-MM-dd HH:00");
                drNew["DateTime"] = strTime;
                for (int i = 0; i < portIds.Count(); i++)
                {
                    DataRow[] rows = dt.Select("DateTime='" + strTime + "' and PointId='" + portIds[i] + "'");
                    foreach (DataRow row in rows)
                    {
                        drNew[portIds[i]] = row[Factor].ToString();              
                    }
                }
            }
            hdSinglePollutant.Value = ToJson(dtNew);
            hdpointiddata.Value = ToJson(dtpoint);
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

    }
}