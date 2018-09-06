using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
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
    /// 名称：OnlineInfoNew.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-12-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：首页元件 实时在线信息
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class OnlineInfoNew : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        AirRealTimeOnlineStateNewService airRealTimeOnlineStateNew = new AirRealTimeOnlineStateNewService();
        /// <summary>
        /// 个性化配置服务
        /// </summary>
        PersonalizedSetService personalizedSet = new PersonalizedSetService();

        /// <summary>
        /// 获取区域服务
        /// </summary>
        DictionaryService dicService = new DictionaryService();

        /// <summary>
        /// 获取测点或因子的服务
        /// </summary>
        MonitoringPointAirService monitoringPointAir = new MonitoringPointAirService();

        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

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
            comboCityProper.DataSource = dicService.RetrieveCityList();
            comboCityProper.DataTextField = "ItemText";
            comboCityProper.DataValueField = "ItemGuid";
            comboCityProper.DataBind();
            for (int i = 0; i < comboCityProper.Items.Count; i++)
            {
                comboCityProper.Items[i].Checked = true;
            }

            BindData();
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //InitControl();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            #region 根据用户的配置的userGuid 获取该测点

            //string userGuid = "4ce5bed9-78bd-489f-8b3f-a830098759c4";
            string userGuid = this.Session["UserGuid"].ToString();
            IQueryable<PersonalizedSettingEntity> getPoint = personalizedSet.GetPersonalizedPoint(userGuid);
            string[] pointUid = getPoint.Select(it => it.ParameterUid).GroupBy(p => p).Select(p => p.Key).ToArray();//获取测点uid
            IQueryable<MonitoringPointEntity> monitoringPoint = monitoringPointAir.RetrieveListByPointUids(pointUid);
            string[] point = GetPointIdsByPointEntitys(monitoringPoint.ToArray()).GroupBy(p => p).Select(p => p.Key).ToArray();//根据测点数组获取测点Id列 用户配置的pointID 

            #endregion

            #region 根据区域获取该测点 并和用户配置的测点去交集

            List<string> points = new List<string>();
            List<string> strPointID = new List<string>();
            //获取选定的值
            foreach (RadComboBoxItem item in comboCityProper.CheckedItems)
            {
                IQueryable<MonitoringPointEntity> monitoringPointEntity = monitoringPointAir.RetrieveAirMPListByStatisticalCityType(item.Value.ToString());
                string[] pointID = GetPointIdsByPointEntitys(monitoringPointEntity.ToArray());//根据测点数组获取测点Id列
                strPointID.AddRange(pointID);

            }

            string[] portIds = strPointID.GroupBy(p => p).Select(p => p.Key).ToArray();

            var z2 = portIds.Intersect(point);//portIds为根据区域获取的测点 point为用户配置的测点 取两者的交集
            foreach (var i in z2)
            {
                points.Add(i.ToString());
            }

            string[] strPoints = points.ToArray();
            string[] strFactors = GetPollutantCodesByPointIds(strPoints).GroupBy(p => p).Select(p => p.Key).ToArray();//根据测点Id数组获取因子列
            hdPointNames.Value = string.Join(";", strPoints);
            hdFactorName.Value = string.Join(";", strFactors);
            #endregion

            #region 绑定数据

            Dictionary<string, int> dicStatusCode = new Dictionary<string, int>();
            dicStatusCode.Add("OnlineCount", 1);//在线数
            dicStatusCode.Add("OfflineCount", 0);//离线
            //dicStatusCode.Add("WarnCount", 4);//报警
            //dicStatusCode.Add("FaultCount", 8);//故障
            //dicStatusCode.Add("StopCount", 16);//停运
            //dicStatusCode.Add("AlwaysOnlineCount", 32);//始终在线

            //DateTime dtmEnd = DateTime.Now;//结束时间为现在
            //DateTime dtmStart = dtmEnd.AddDays(-7);//开始时间为一星期前

            DataTable dtOnlineRate = airRealTimeOnlineStateNew.GetRealTimeOnlineStateDataPager(strPoints,  dicStatusCode);
            hdAirOnlineInfo.Value = ToJson(dtOnlineRate);

            #endregion

        }

        /// <summary>
        /// 根据测点数组获取测点Id列
        /// </summary>
        /// <param name="monitoringPointEntitys">测点数组</param>
        /// <returns></returns>
        private string[] GetPointIdsByPointEntitys(MonitoringPointEntity[] monitoringPointEntitys)
        {
            IList<string> pointIdList = new List<string>();
            foreach (MonitoringPointEntity monitoringPointEntity in monitoringPointEntitys)
            {
                if (!pointIdList.Contains(monitoringPointEntity.PointId.ToString()))
                {
                    pointIdList.Add(monitoringPointEntity.PointId.ToString());
                }
            }
            return pointIdList.ToArray();
        }

        /// <summary>
        /// 根据测点Id数组获取因子列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        public string[] GetPollutantCodesByPointIds(string[] pointIds)
        {
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointAir.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            string[] pollutantCodes = GetPollutantCodesByPointEntitys(monitorPointQueryable.ToArray());
            return pollutantCodes;
        }

        /// <summary>
        /// 根据测点数组获取因子列
        /// </summary>
        /// <param name="monitoringPointEntitys">测点数组</param>
        /// <returns></returns>
        public string[] GetPollutantCodesByPointEntitys(MonitoringPointEntity[] monitoringPointEntitys)
        {
            IList<string> pollutantList = new List<string>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            foreach (MonitoringPointEntity monitoringPointEntity in monitoringPointEntitys)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                    instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                foreach (PollutantCodeEntity pollutantCodeEntity in pollutantCodeQueryable)
                {
                    pollutantList.Add(pollutantCodeEntity.PollutantCode);
                }
            }
            return pollutantList.ToArray();
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindData();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowECharts(); ", true);
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