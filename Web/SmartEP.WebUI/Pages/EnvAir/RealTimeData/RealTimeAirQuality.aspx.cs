using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.Frame;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebControl.CbxRsm;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.RealTimeData
{
    /// <summary>
    /// 名称：RealTimeAirQuality.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：实时环境空气质量
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class RealTimeAirQuality : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private HourAQIService m_HourAQIService;

        /// <summary>
        /// 字典
        /// </summary>
        DictionaryService dicService = new DictionaryService();

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<PollutantCodeEntity> factors = null;

        ///// <summary>
        ///// 选择站点
        ///// </summary>
        private IList<IPoint> points = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_HourAQIService = new HourAQIService();
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
            string type = PageHelper.GetQueryString("Type");//从其它界面跳到该页面时的参数
            string cityTypeUids = PageHelper.GetQueryString("CityTypeUids");
            string[] cityTypeUidArray = cityTypeUids.Split(';');
            //IQueryable<MonitoringPointEntity> monitoringPointQueryable = null;
            //string[] regionUidArray = null;
            //if (!string.IsNullOrWhiteSpace(cityTypeUids))
            //{
            //    MonitoringPointAirService monitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            //    monitoringPointQueryable = monitoringPointAirService
            //         .RetrieveAirMPList();//获取所有空气点位列表//根据统计城市类型获取点位列表
            //    monitoringPointQueryable = monitoringPointQueryable.Where(p => cityTypeUidArray.Contains(p.MonitoringPointExtensionForEQMSAirEntity.CityTypeUid));
            //    regionUidArray = monitoringPointQueryable.Select(t => t.MonitoringPointExtensionForEQMSAirEntity.RegionTypeUid).Distinct().ToArray();
            //}
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
            dtpBegin.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00"));
            dtpEnd.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
            //DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            //IQueryable<V_CodeMainItemEntity> codeMainItemQueryable = dictionaryService.RetrieveList(DictionaryType.Air, "点位城市类型");//获取城市类型

            //comboRegion.DataSource = codeMainItemQueryable;
            //comboRegion.DataValueField = "ItemGuid";
            //comboRegion.DataTextField = "ItemText";
            //comboRegion.DataBind();

            //comboRegion.Items.Add(new RadComboBoxItem("苏州市区", "7e05b94c-bbd4-45c3-919c-42da2e63fd43"));
            //comboRegion.Items.Add(new RadComboBoxItem("吴江区", "48d749e6-d07c-4764-8d50-50f170defe0b"));
            //comboRegion.Items.Add(new RadComboBoxItem("昆山市", "636775d8-091d-4754-9ed2-cd9dfef1f6ab"));
            //comboRegion.Items.Add(new RadComboBoxItem("太仓市", "d993d02f-fcc3-4ea6-b52b-9414fbd9b8e6"));
            //comboRegion.Items.Add(new RadComboBoxItem("常熟市", "f7444783-a425-411c-a54b-f9fed72ec72e"));
            //comboRegion.Items.Add(new RadComboBoxItem("张家港市", "4296ce53-78d3-4741-9eda-6306e3e5b399"));

            //comboRegion.DataSource = dicService.RetrieveCityList();
            //comboRegion.DataTextField = "ItemText";
            //comboRegion.DataValueField = "ItemGuid";
            //comboRegion.DataBind();
            //for (int i = 0; i < comboRegion.Items.Count; i++)
            //{

            //    if (!IsPostBack && !string.IsNullOrWhiteSpace(cityTypeUids))
            //    {
            //        if (cityTypeUidArray.Contains(comboRegion.Items[i].Value))
            //        {
            //            comboRegion.Items[i].Checked = true;
            //        }
            //    }
            //    else
            //    {
            //        comboRegion.Items[i].Checked = true;
            //    }
            //}

            //if (!string.IsNullOrWhiteSpace(type))
            //{
            //    string regionUid = PageHelper.GetQueryString("RegionUid");
            //    if (comboRegion.Items.FindItemByValue(regionUid) != null)
            //    { 
            //        comboRegion.Items.FindItemByValue(regionUid).Checked =true;
            //    }
            //}
            //else
            //{
            //    //if (comboRegion.Items.Count > 0)
            //    //{
            //    //    comboRegion.Items[0].Checked = true;
            //    //}
            //    for (int i = 0; i < comboRegion.Items.Count; i++)
            //    {
            //        comboRegion.Items[i].Checked = true;
            //    }
            //}
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            if (!IsPostBack)
            {
                //comboRegion_SelectedIndexChanged(null, null);
                //RadComboBox RadCBoxPoint = (RadComboBox)pointCbxRsm.FindControl("RadCBoxPoint");
                //RadioButtonList pointType = (RadioButtonList)RadCBoxPoint.Header.FindControl("radPointType");
                //pointType.SelectedValue = RsmPointMode.Region.ToString();
                //string script = string.Format("$('#{0}').change();", pointType.ClientID);
                //ScriptManager.RegisterStartupScript(Page, GetType(), DateTime.Now.ToFileTime().ToString(), script, true);
            }
            DateTime dtmEnd = dtpEnd.SelectedDate.Value; //DateTime.Now;
            DateTime dtmBegion = dtpBegin.SelectedDate.Value;//dtmEnd.AddDays(-7);
            points = pointCbxRsm.GetPoints();
            //string[] pointArray = points.Select(t => t.PointID).ToArray();
            //string[] pointArray = comboPoint.CheckedItems.Select(t => t.Value).ToArray();
            string[] pointArray = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            factors = GetPollutantListByCalAQI();//获取参与评价AQI的常规6因子
            int pageSize = gridRealTimeAQI.PageSize;  //每页显示数据个数  
            int pageNo = gridRealTimeAQI.CurrentPageIndex;  //当前页的序号
            int recordTotal = 0;  //数据总行数
            if (pointArray == null || pointArray.Length == 0)
            {
                gridRealTimeAQI.DataSource = new DataTable();
            }
            else
            {
                var dataView = m_HourAQIService.RealTimeAirPointsQuality(pointArray, dtmBegion, dtmEnd);

                if (dataView == null)
                {
                    gridRealTimeAQI.DataSource = new DataTable();
                }
                else
                {
                    //dataView = GetNewViewByTurnData(dataView);
                    gridRealTimeAQI.DataSource = dataView;
                    gridRealTimeAQI.VirtualItemCount = dataView.Count;
                }
            }
        }

        /// <summary>
        /// 获取根据单位转换浓度值后的新数据视图
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        private DataView GetNewViewByTurnData(DataView dv)
        {
            DataView dvNew = new DataView();
            DataTable dt = dv.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                //mg/m3转成μg/m3的浓度值
                if (!string.IsNullOrWhiteSpace(dr["SO2"].ToString()))
                {
                    dr["SO2"] = (decimal.Parse(dr["SO2"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["NO2"].ToString()))
                {
                    dr["NO2"] = (decimal.Parse(dr["NO2"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["PM10"].ToString()))
                {
                    dr["PM10"] = (decimal.Parse(dr["PM10"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["PM25"].ToString()))
                {
                    dr["PM25"] = (decimal.Parse(dr["PM25"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["O3"].ToString()))
                {
                    dr["O3"] = (decimal.Parse(dr["O3"].ToString()) * 1000).ToString("G0");
                }
            }
            dvNew = dt.AsDataView();
            return dvNew;
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    //string pointName = points.Where(x => x.PointID.Equals(drv["PointId"].ToString().Trim()))
                    //                   .Select(t => t.PointName).FirstOrDefault();
                    //string pointName = comboPoint.Items.Where(x => x.Value.Equals(drv["PointId"].ToString().Trim()))
                    //                   .Select(t => t.Text).FirstOrDefault();
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                    if (points != null)
                        pointCell.Text = point.PointName;
                }
                if (item["DateTime"] != null)
                {
                    GridTableCell dateTimeCell = (GridTableCell)item["DateTime"];
                    DateTime dateTime;
                    if (DateTime.TryParse(dateTimeCell.Text, out dateTime))
                    {
                        dateTimeCell.Text = dateTime.AddHours(1).ToString("MM-dd HH时");
                    }
                }
                if (drv.DataView.Table.Columns.Contains("RGBValue")) //if (item["RGBValue"] != null)
                {
                    //GridTableCell cell = item["RGBValue"] as GridTableCell;
                    //cell.Style.Add("background-color", cell.Text);
                    //cell.Text = string.Empty;
                    GridTableCell cell = item["Class"] as GridTableCell;
                    cell.Style.Add("background-color", drv["RGBValue"].ToString().Trim());
                }
                for (int i = 0; i < factors.Count; i++)
                {
                    string uniqueName = GetUniqueNameByPollutantName(factors[i].PollutantName);
                    if (drv.DataView.Table.Columns.Contains(uniqueName) && item[uniqueName] != null)
                    {
                        GridTableCell factorCell = (GridTableCell)item[uniqueName];
                        decimal pollutantValue;

                        if (decimal.TryParse(factorCell.Text, out pollutantValue))
                        {
                            //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                            AirPollutantService m_AirPollutantService = new AirPollutantService();
                            int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factors[i].PollutantCode).PollutantDecimalNum);

                            //保留小数位数,value 需要进行小数位处理的数据 类型Decimal
                            if (uniqueName == "CO")
                            {
                                factorCell.Text = DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum).ToString();
                            }
                            else
                            {
                                factorCell.Text = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum) * 1000).ToString("G0");
                            }
                        }
                    }
                }
            }
        }

        private string GetUniqueNameByPollutantName(string pollutantName)
        {
            string returnValue = string.Empty;
            switch (pollutantName)
            {
                case "二氧化硫":
                    returnValue = "SO2";
                    break;
                case "二氧化氮":
                    returnValue = "NO2";
                    break;
                case "PM10":
                    returnValue = "PM10";
                    break;
                case "PM2.5":
                    returnValue = "PM25";
                    break;
                case "一氧化碳":
                    returnValue = "CO";
                    break;
                case "臭氧":
                    returnValue = "O3";
                    break;
                default: break;
            }
            return returnValue;
        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridRealTimeAQI.CurrentPageIndex = 0;
            gridRealTimeAQI.Rebind();
        }


        /// <summary>
        /// 区域选择切换站点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void comboRegion_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    RadComboBox comboBox = comboRegion;//sender as RadComboBox;
        //    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();//测点类
        //    PersonalizedSetService personalizedSet = new PersonalizedSetService();
        //    IList<IPoint> pointList = new List<IPoint>();//测点列表
        //    string pointNames = string.Empty;

        //    #region 根据用户的配置的userGuid 获取该测点

        //    //string userGuid = "4ce5bed9-78bd-489f-8b3f-a830098759c4";
        //    string userGuid = SessionHelper.Get("UserGuid");
        //    IQueryable<PersonalizedSettingEntity> getPoint = personalizedSet.GetPersonalizedPoint(userGuid);
        //    string[] pointUidsByUser = getPoint.Select(t => t.ParameterUid).GroupBy(p => p).Select(p => p.Key).ToArray();//获取测点uid
        //    //IQueryable<MonitoringPointEntity> monitoringPointsByUser = monitoringPointAir.RetrieveListByPointUids(pointUidsByUser);
        //    //string[] pointIdsByUser = monitoringPointsByUser.Select(t => t.PointId.ToString()).GroupBy(p => p).Select(p => p.Key).ToArray();//根据测点数组获取测点Id列 用户配置的pointID 
        //    #endregion

        //    for (int i = 0; i < comboBox.CheckedItems.Count; i++)
        //    {
        //        string regionUid = comboBox.CheckedItems[i].Value;
        //        //IQueryable<MonitoringPointEntity> entityQueryable = monitoringPointAir.RetrieveAirMPListByRegion(regionUid);//根据区域获取测点
        //        //CityType cityType = CityType.SuZhou;
        //        //switch (comboBox.CheckedItems[i].Text)
        //        //{
        //        //    case "苏州市区":
        //        //        cityType = CityType.SuZhou;
        //        //        break;
        //        //    case "吴江区":
        //        //        cityType = CityType.WuJiang;
        //        //        break;
        //        //    case "昆山市":
        //        //        cityType = CityType.KunShan;
        //        //        break;
        //        //    case "太仓市":
        //        //        cityType = CityType.TaiCang;
        //        //        break;
        //        //    case "常熟市":
        //        //        cityType = CityType.ChangShu;
        //        //        break;
        //        //    case "张家港市":
        //        //        cityType = CityType.ZhangJiaGang;
        //        //        break;
        //        //    default: break;
        //        //}
        //        ////获取城市均值字典Guid
        //        //string cityTypeUid = SmartEP.Core.Enums.EnumMapping.GetDesc(cityType).Split(':')[1];
        //        IQueryable<MonitoringPointEntity> entityQueryable = monitoringPointAir.RetrieveAirMPListByEnable(); //monitoringPointAir.RetrieveAirMPListByCity(cityType);//根据城市均值类型获取所有启用点位列表
        //        entityQueryable = entityQueryable.Where(p => p.MonitoringPointExtensionForEQMSAirEntity.CityTypeUid == regionUid
        //                                                     && pointUidsByUser.Contains(p.MonitoringPointUid));
        //        foreach (MonitoringPointEntity monitorPointEntity in entityQueryable)
        //        {
        //            //如果没有该测点，则添加测点
        //            if (pointList.Count(t => t.PointID == monitorPointEntity.PointId.ToString()) == 0)
        //            {
        //                IPoint point = new RsmPoint(monitorPointEntity.MonitoringPointName, monitorPointEntity.PointId.ToString(),
        //                                              monitorPointEntity.MonitoringPointUid);
        //                pointList.Add(point);
        //            }
        //        }
        //    }
        //    //for (int i = 0; i < pointList.Count; i++)
        //    //{
        //    //    pointNames += pointList[i].PointName + ";";
        //    //}
        //    //pointCbxRsm.SetPointValuesFromNames(pointNames);//设置默认点位（点位名称）
        //    comboPoint.DataSource = pointList;
        //    comboPoint.DataValueField = "PointID";
        //    comboPoint.DataTextField = "PointName";
        //    comboPoint.DataBind();
        //    //if (comboPoint.Items.Count > 0)
        //    //{
        //    //    comboPoint.Items[0].Checked = true;
        //    //}
        //    for (int i = 0; i < comboPoint.Items.Count; i++)
        //    {
        //        comboPoint.Items[i].Checked = true;
        //    }
        //}

        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
                points = pointCbxRsm.GetPoints();
                //string[] portIds = comboPoint.CheckedItems.Select(t => t.Value).ToArray();
                DateTime dtmEnd = dtpEnd.SelectedDate.Value; //DateTime.Now;
                DateTime dtmBegion = dtpBegin.SelectedDate.Value;//dtmEnd.AddDays(-7);
                DataView dv = m_HourAQIService.RealTimeAirPointsQuality(portIds, dtmBegion, dtmEnd);
                DataTable dt = UpdateExportColumnName(dv);
                ExcelHelper.DataTableToExcel(dt, "空气质量实时报", "空气质量实时报", this.Page);
            }
        }

        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataView dv)
        {
            DataTable dtOld = dv.ToTable();
            DataTable dtNew = dtOld.Clone();
            if (dtNew.Columns.Contains("PointId"))
            {
                dtNew.Columns["PointId"].DataType = typeof(string);
            }
            if (dtNew.Columns.Contains("DateTime"))
            {
                dtNew.Columns["DateTime"].DataType = typeof(string);
            }
            //points = pointCbxRsm.GetPoints();
            //string[] pointArray = points.Select(t => t.PointID).ToArray();
            //factors = GetPollutantCodesByPointIds(pointArray);
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            factors = GetPollutantListByCalAQI();//获取参与评价AQI的常规6因子

            for (int i = 0; i < dtOld.Rows.Count; i++)
            {
                DataRow drOld = dtOld.Rows[i];
                DataRow drNew = dtNew.NewRow();
                foreach (DataColumn dcOld in dtOld.Columns)
                {
                    drNew[dcOld.ColumnName] = drOld[dcOld];
                    if (dcOld.ColumnName == "DateTime")
                    {
                        DateTime dateTime = DateTime.Parse(drOld[dcOld].ToString()).AddHours(1);
                        drNew[dcOld.ColumnName] = string.Format("{0:yyyy-MM-dd HH时}", dateTime);
                    }
                }
                drNew["PointId"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                    ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                    : drNew["PointId"].ToString();
                //drNew["PointId"] = (comboPoint.Items.Count(t => t.Value == drNew["PointId"].ToString()) > 0)
                //    ? comboPoint.Items.Where(t => t.Value == drNew["PointId"].ToString()).Select(t => t.Text).FirstOrDefault()
                //    : drNew["PointId"].ToString();
                dtNew.Rows.Add(drNew);
                for (int j = 0; j < factors.Count; j++)
                {
                    string uniqueName = GetUniqueNameByPollutantName(factors[j].PollutantName);
                    //foreach (string uniqueName in uniqueNames)
                    //{
                    if (dtNew.Columns.Contains(uniqueName) && !string.IsNullOrWhiteSpace(drNew[uniqueName].ToString()))
                    {
                        //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                        int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factors[j].PollutantCode).PollutantDecimalNum);
                        decimal pollutantValue = decimal.TryParse(drNew[uniqueName].ToString(), out pollutantValue) ? pollutantValue : 0;

                        //保留小数位数,value 需要进行小数位处理的数据 类型Decimal
                        if (uniqueName == "CO")
                        {
                            drNew[uniqueName] = DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum).ToString();
                        }
                        else
                        {
                            drNew[uniqueName] = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum) * 1000).ToString("G0");
                        }
                    }
                    //}
                }
            }
            for (int i = 0; i < dtNew.Columns.Count; i++)
            {
                DataColumn dcNew = dtNew.Columns[i];
                if (dcNew.ColumnName == "PointId")
                {
                    dcNew.ColumnName = "监测点位名称";
                }
                else if (dcNew.ColumnName == "DateTime")
                {
                    string tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    dcNew.ColumnName = "日期";
                }
                else if (dcNew.ColumnName == "SO2")
                {
                    dcNew.ColumnName = "二氧化硫/(μg/m3)";
                }
                else if (dcNew.ColumnName == "NO2")
                {
                    dcNew.ColumnName = "二氧化氮/(μg/m3)";
                }
                else if (dcNew.ColumnName == "PM10")
                {
                    dcNew.ColumnName = "PM10/(μg/m3)";
                }
                else if (dcNew.ColumnName == "PM25")
                {
                    dcNew.ColumnName = "PM2.5/(μg/m3)";
                }
                else if (dcNew.ColumnName == "CO")
                {
                    dcNew.ColumnName = "一氧化碳/(mg/m3)";
                }
                else if (dcNew.ColumnName == "O3")
                {
                    dcNew.ColumnName = "臭氧/(μg/m3)";
                }
                else if (dcNew.ColumnName == "AQIValue")
                {
                    dcNew.ColumnName = "AQI";
                }
                else if (dcNew.ColumnName == "PrimaryPollutant")
                {
                    dcNew.ColumnName = "首要污染物";
                }
                else if (dcNew.ColumnName == "Class")
                {
                    dcNew.ColumnName = "类别";
                }
                else
                {
                    dtNew.Columns.Remove(dcNew);
                    i--;
                }
            }
            return dtNew;
        }
        #endregion

        /// <summary>
        /// 根据测点Id数组获取因子列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantCodesByPointIds(string[] pointIds)
        {
            MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = monitoringPointAir.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            IList<PollutantCodeEntity> pollutantList = new List<PollutantCodeEntity>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                    instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                pollutantList = pollutantList.Union(pollutantCodeQueryable).ToList();
            }
            return pollutantList;
        }

        /// <summary>
        /// 获取参与评价AQI的常规6因子
        /// </summary>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantListByCalAQI()
        {
            AirPollutantService airPollutantService = new AirPollutantService();
            return airPollutantService.RetrieveListByCalAQI().ToList();
        }
    }
}