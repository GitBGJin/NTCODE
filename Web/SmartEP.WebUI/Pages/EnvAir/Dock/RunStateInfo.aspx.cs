using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Dock
{
    /// <summary>
    /// 名称：RunStateInfo.aspx
    /// 创建人：刘长敏
    /// 创建日期：2015-09-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：首页元件 运行状态
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class RunStateInfo : SmartEP.WebUI.Common.BasePage
    {

        AirRealTimeOnlineStateService m_AirRealTimeOnlineStateService = new AirRealTimeOnlineStateService(PollutantDataType.Min60);
        DictionaryService dicService = new DictionaryService();

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //InitControl();
            }
        }
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
            //comboCityProper.DataSource = dicService.RetrieveRegionList(CityType.SuZhou);
            //comboCityProper.DataTextField = "ItemText";
            //comboCityProper.DataValueField = "ItemGuid";
            //comboCityProper.DataBind();
            //if (comboCityProper.Items.Count > 0)
            //{
            //    comboCityProper.Items[0].Checked = true;
            //}
            comboCityProper.DataSource = dicService.RetrieveCityList();
            comboCityProper.DataTextField = "ItemText";
            comboCityProper.DataValueField = "ItemGuid";
            comboCityProper.DataBind();
            if (comboCityProper.Items.Count > 0)
            {
                for (int i = 0; i < comboCityProper.Items.Count; i++)
                {
                    comboCityProper.Items[i].Checked = true;
                }
            }
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

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            // 绑定数据
            string regionGuid = "";
            foreach (RadComboBoxItem item in comboCityProper.CheckedItems)
            {
                regionGuid += (item.Value.ToString() + ",");
            }
            string[] regionGuids = regionGuid.Trim(',').Split(',');
            //DataTable runState = m_AirRealTimeOnlineStateService.GetRunningStateInfoData(regionGuids).Table;
            points = pointCbxRsm.GetPoints();
            string[] portIds = points.Select(t => t.PointID).ToArray();
            DataTable runState = m_AirRealTimeOnlineStateService.GetRunningStateInfoData(portIds).Table;
            //runState = GetDataTableToShowECharts(runState);

            hdRunState.Value = ToJson(runState);//有效数据
        }


        ///// <summary>
        ///// 根据要显示的图表获取相对应的数据表
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <returns></returns>
        //private static DataTable GetDataTableToShowECharts(DataTable dt)
        //{
        //    DataTable dtNew = new DataTable();
        //    dtNew.Columns.Add("State");
        //    dtNew.Columns.Add("RunStateCount");
        //    dtNew.Columns.Add("RunStateRate");
        //    dtNew.Rows.Add("超标", 24, 12);
        //    dtNew.Rows.Add("异常", 31, 5);
        //    dtNew.Rows.Add("故障", 30, 6);
        //    dtNew.Rows.Add("停运", 18, 18);
        //    return dtNew;
        //}


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