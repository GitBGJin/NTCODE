using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Alarm;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Dock
{
    public partial class AlarmInfoStatistic : SmartEP.WebUI.Common.BasePage
    {
        CreateAlarmService m_CreateAlarmService = new CreateAlarmService();
        DictionaryService g_dicService = new DictionaryService();
        protected void Page_Load(object sender, EventArgs e)
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
            //国控点，对照点，路边站
            MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            string strpointName = "";
            IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad" || p.ContrlUid == "bdf0837a-eb59-4c4a-a05f-c774a17f3077" || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
            foreach (string point in EnableOrNotportsarry)
            {
                strpointName += point + ";";
            }
            pointCbxRsm.SetPointValuesFromNames(strpointName);
            dayBegin.SelectedDate = DateTime.Now;
            dayEnd.SelectedDate = DateTime.Now;
            hdpointiddata.Value = strpointName;
            hdBeginTime.Value = new DateTime(dayBegin.SelectedDate.Value.Year, dayBegin.SelectedDate.Value.Month, dayBegin.SelectedDate.Value.Day).ToString();
            hdEndTime.Value = new DateTime(dayEnd.SelectedDate.Value.Year, dayEnd.SelectedDate.Value.Month, dayEnd.SelectedDate.Value.Day).AddDays(+1).AddMilliseconds(-1).ToString();
            BindData();
        }
        //绑定数据
        private void BindData()
        {
            //报警类型
            IQueryable<V_CodeMainItemEntity> alarmTypeEntites = g_dicService.RetrieveList(DictionaryType.AMS, "报警类型").Where(t => !(t.ItemText.Contains("零值")));
            string[] Alarmtype = alarmTypeEntites.Select(t => t.ItemGuid).ToArray();
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.Guid);
            DateTime dtstart = new DateTime(dayBegin.SelectedDate.Value.Year, dayBegin.SelectedDate.Value.Month, dayBegin.SelectedDate.Value.Day);
            DateTime dtend = new DateTime(dayEnd.SelectedDate.Value.Year, dayEnd.SelectedDate.Value.Month, dayEnd.SelectedDate.Value.Day).AddDays(+1).AddMilliseconds(-1);
            DataTable dt = m_CreateAlarmService.GetAlarmInfo(portIds, dtstart, dtend, SmartEP.Core.Enums.EnumMapping.GetDesc(ApplicationType.Air), Alarmtype);
            hdSinglePollutant.Value = ToJson(dt);
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
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            hdBeginTime.Value = new DateTime(dayBegin.SelectedDate.Value.Year, dayBegin.SelectedDate.Value.Month, dayBegin.SelectedDate.Value.Day).ToString();
            hdEndTime.Value = new DateTime(dayEnd.SelectedDate.Value.Year, dayEnd.SelectedDate.Value.Month, dayEnd.SelectedDate.Value.Day).AddDays(+1).AddMilliseconds(-1).ToString();
            BindData();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowECharts(); ", true);
        }

        protected void dayBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            hdBeginTime.Value = new DateTime(dayBegin.SelectedDate.Value.Year, dayBegin.SelectedDate.Value.Month, dayBegin.SelectedDate.Value.Day).ToString();
            hdEndTime.Value = new DateTime(dayEnd.SelectedDate.Value.Year, dayEnd.SelectedDate.Value.Month, dayEnd.SelectedDate.Value.Day).AddDays(+1).AddMilliseconds(-1).ToString();
        }

        protected void dayEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            hdBeginTime.Value = new DateTime(dayBegin.SelectedDate.Value.Year, dayBegin.SelectedDate.Value.Month, dayBegin.SelectedDate.Value.Day).ToString();
            hdEndTime.Value = new DateTime(dayEnd.SelectedDate.Value.Year, dayEnd.SelectedDate.Value.Month, dayEnd.SelectedDate.Value.Day).AddDays(+1).AddMilliseconds(-1).ToString();
        }

        protected void pointCbxRsm_SelectedChanged()
        {
            IList<IPoint> points = pointCbxRsm.GetPoints();//测点
            string[] portIds = points.Select(t => t.PointID).ToArray();
            string strPointName = "";
            foreach (string item in portIds)
            {
                strPointName += (item.ToString() + ";");
            }
            hdpointiddata.Value = strPointName;
        }
    }
}