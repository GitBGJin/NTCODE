using Aspose.Cells;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Core.Enums;
using SmartEP.Utilities.Caching;
using SmartEP.Service.Frame;
using SmartEP.Service.Core.Enums;
using SmartEP.DomainModel;
using SmartEP.Service.DataAuditing.AuditInterfaces;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class HourReportUpLoad : BasePage
    {
        DictionaryService g_DictionaryService = Singleton<DictionaryService>.GetInstance();
        /// <summary>
        /// 小时数据接口
        /// </summary>
        DataQueryByHourService g_DataQueryByHourService = Singleton<DataQueryByHourService>.GetInstance();
        /// <summary>
        /// 上传临时表接口
        /// </summary>
        DataQueryByHourForUpLoadService g_DataQueryByHourForUpLoadService = Singleton<DataQueryByHourForUpLoadService>.GetInstance();
        /// <summary>
        /// 测点接口
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
        /// <summary>
        /// 文档路径
        /// </summary>
        string SavePath = "~/Files/";
        /// <summary>
        /// 需要上传的监测因子
        /// </summary>
        List<string> PollutantCodes = new List<string>() { "a21026", "a21003", "a21004", "a21002", "a34002", "a21005", "a05024", "a34004", "a01007", "a01008", "a01001", "a01002", "a01006", "a01020" };
        List<string> PollutantNames = new List<string>() { "SO2", "NO", "NO2", "NOX", "PM10", "CO", "O3", "PM2.5", "WS", "WD", "TEMP", "RH", "PRESS", "VISIBILITY" };
        /// <summary>
        /// 上传模板中除配置因子以外的其它表头
        /// </summary>
        List<string> ExcelTitle = new List<string>() { "stcode", "stname", "yyyy", "mm", "dd", "hh", "dwcode", "dwname" };
        /// <summary>
        /// 数据类型
        /// </summary>
        int DataType = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        #region RadGrid绑定数据源
        /// <summary>
        /// RadGrid绑定数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            string UserGuid = SessionHelper.Get("DisplayName");
            //测点
            List<int> DWcodes = new List<int>();
            foreach (RadComboBoxItem item in RadPsSel.Items)
            {
                if (item.Checked)
                {
                    DWcodes.Add(Convert.ToInt32(item.Value));
                }
            }
            if (DWcodes == null || DWcodes.Count == 0)
            {
                Alert("请选择监测点");
                return;
            }
            List<int> PointIds = new List<int>();
            IQueryable<MonitoringPointEntity> PointLists = g_MonitoringPointAirService.Retrieve(x => DWcodes.Contains((int)x.MonitoringPointExtensionForEQMSAirEntity.Dwcode));
            foreach (MonitoringPointEntity Point in PointLists)
            {
                int PointDwcode = Point.MonitoringPointExtensionForEQMSAirEntity.Dwcode.Value;
                foreach (int code in DWcodes)
                {
                    if (code == PointDwcode)
                    {
                        if (!PointIds.Contains(Point.PointId))
                        {
                            PointIds.Add(Point.PointId);
                        }
                    }
                }
            }
            DataTable dt = g_DataQueryByHourForUpLoadService.GetUpLoadData(PointIds, PollutantCodes, DataType, UserGuid);
            IQueryable<V_CodeMainItemEntity> ItemLists = g_DictionaryService.RetrieveList(DictionaryType.AMS, "国家数据标记");
            foreach (DataRow dr in dt.Rows)
            {
                foreach (string code in PollutantCodes)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr[code + "_Flag"])))
                    {
                        foreach (V_CodeMainItemEntity item in ItemLists)
                        {
                            if (Convert.ToString(dr[code + "_Flag"]) == item.ItemValue)
                            {
                                dr[code + "_Flag"] = item.ItemValue + "," + item.ItemText;
                                break;
                            }
                        }
                    }
                }
            }
            RadGrid1.DataSource = dt;
        }
        #endregion

        #region 上传按钮事件
        /// <summary>
        /// 上传按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpLoad_Click(object sender, ImageClickEventArgs e)
        {
            string UserGuid = SessionHelper.Get("DisplayName");
            //判断文件路径
            if (FileUpload1.PostedFile.FileName.Length == 0)
            {
                Alert("请选择需要上传的文件");
                return;
            }
            //文件名
            string filename = FileUpload1.FileName;
            //上传文件
            string NewFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + filename;
            string filePath = Server.MapPath(SavePath + NewFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            FileUpload1.PostedFile.SaveAs(filePath);//将文件存储到服务器上

            #region 读取Excel
            Workbook excel = new Workbook(filePath);
            Worksheet sheet = excel.Worksheets[0];
            Cells cl = sheet.Cells;

            int column = cl.MaxDataColumn;
            int row = cl.MaxDataRow;
            if (column != 21)
            {
                Alert("请确认Excel是否缺少列元素");
                return;
            }
            if (row >= 1)
            {
                //获取数据
                DataTable dt = cl.ExportDataTable(1, 0, row, column + 1);

                #region 验证配置因子
                bool isPollutant = true;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ColumnName = cl[0, i].StringValue.ToLower();
                    if (!ExcelTitle.Contains(cl[0, i].StringValue.ToLower()) && !PollutantNames.Contains(cl[0, i].StringValue.ToUpper()))
                    {
                        isPollutant = false;
                        Alert("Excel列 " + cl[0, i].StringValue + " 元素不在模板配置因子中");
                        break;
                    }
                }
                if (!isPollutant)
                {
                    return;
                }
                #endregion

                #region 验证数据有效性
                bool isError = false;
                List<int> DWcodes = new List<int>();
                List<DateTime> DateLists = new List<DateTime>();
                int yyyy = 0, mm = 0, dd = 0, hh = 0, dwcode = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 年、月、日、小时、Code验证
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //年
                        if (dt.Columns[j].ColumnName == "yyyy")
                        {
                            try
                            {
                                yyyy = Convert.ToInt32(dt.Rows[i][j]);
                                if (yyyy < 1900 || yyyy > 2099)
                                {
                                    isError = true;
                                    Alert("Excel第 " + (i + 2).ToString() + " 行，列yyyy数据格式有误，请确认");
                                    break;
                                }
                            }
                            catch
                            {
                                isError = true;
                                Alert("Excel第 " + (i + 2).ToString() + " 行，列yyyy数据格式有误，请确认");
                                break;
                            }
                        }
                        //月
                        else if (dt.Columns[j].ColumnName == "mm")
                        {
                            try
                            {
                                mm = Convert.ToInt32(dt.Rows[i][j]);
                                if (mm < 1 || mm > 12)
                                {
                                    isError = true;
                                    Alert("Excel第 " + (i + 2).ToString() + " 行，列mm数据格式有误，请确认");
                                    break;
                                }
                            }
                            catch
                            {
                                isError = true;
                                Alert("Excel第 " + (i + 2).ToString() + " 行，列mm数据格式有误，请确认");
                                break;
                            }
                        }
                        //日
                        else if (dt.Columns[j].ColumnName == "dd")
                        {
                            try
                            {
                                dd = Convert.ToInt32(dt.Rows[i][j]);
                                if (dd < 1 || dd > 31)
                                {
                                    isError = true;
                                    Alert("Excel第 " + (i + 2).ToString() + " 行，列dd数据格式有误，请确认");
                                    break;
                                }
                            }
                            catch
                            {
                                isError = true;
                                Alert("Excel第 " + (i + 2).ToString() + " 行，列dd数据格式有误，请确认");
                                break;
                            }
                        }
                        //小时
                        else if (dt.Columns[j].ColumnName == "hh")
                        {
                            try
                            {
                                hh = Convert.ToInt32(dt.Rows[i][j]);
                                if (hh < 1 || hh > 24)
                                {
                                    isError = true;
                                    Alert("Excel第 " + (i + 2).ToString() + " 行，列hh数据格式有误，请确认");
                                    break;
                                }
                            }
                            catch
                            {
                                isError = true;
                                Alert("Excel第 " + (i + 2).ToString() + " 行，列hh数据格式有误，请确认");
                                break;
                            }
                        }
                        //Code
                        else if (dt.Columns[j].ColumnName == "dwcode")
                        {
                            try
                            {
                                dwcode = Convert.ToInt32(dt.Rows[i][j]);
                            }
                            catch
                            {
                                isError = true;
                                Alert("Excel第 " + (i + 2).ToString() + " 行，列dwcode数据格式有误，请确认");
                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (isError)
                    {
                        break;
                    }
                    #endregion

                    if (!DWcodes.Contains(dwcode))
                    {
                        DWcodes.Add(dwcode);
                    }

                    //验证日期格式
                    DateTime date = DateTime.Now;
                    try
                    {
                        date = Convert.ToDateTime(Convert.ToString(yyyy) + "-" + Convert.ToString(mm) + "-" + Convert.ToString(dd) + " " + Convert.ToString(hh - 1) + ":00:00");
                    }
                    catch
                    {
                        Alert("Excel第 " + (i + 2) + " 行，列yyyy/mm/dd/hh数据格式有误，请确认");
                        break;
                    }
                    if (!DateLists.Contains(date))
                    {
                        DateLists.Add(date);
                    }
                }
                #endregion

                #region 验证是否dwcode<=>PointId对应
                List<int> PointIds = new List<int>();
                IQueryable<MonitoringPointEntity> PointLists = g_MonitoringPointAirService.Retrieve(x => DWcodes.Contains((int)x.MonitoringPointExtensionForEQMSAirEntity.Dwcode));
                if (PointLists == null || PointLists.ToArray().Length == 0)
                {
                    Alert("未找到需要上报的配置测点");
                    return;
                }
                bool isPoint = false;
                foreach (MonitoringPointEntity Point in PointLists)
                {
                    int PointDwcode = Point.MonitoringPointExtensionForEQMSAirEntity.Dwcode.Value;
                    foreach (int code in DWcodes)
                    {
                        if (code == PointDwcode)
                        {
                            if (!PointIds.Contains(Point.PointId))
                            {
                                PointIds.Add(Point.PointId);
                            }
                            isPoint = true;
                            break;
                        }
                    }
                    if (!isPoint)
                    {
                        Alert("Excel列dwcode值 " + PointDwcode.ToString() + " 未找到对应的配置测点");
                        break;
                    }
                }
                if (!isPoint)
                {
                    return;
                }
                #endregion

                #region 验证数据是否已存在，只提示
                string msg = "";
                DataTable HourData = g_DataQueryByHourService.GetHourData(PointIds, PollutantCodes, DateLists);
                if (HourData != null && HourData.Rows.Count > 0)
                {
                    List<string> N_dw = new List<string>();
                    foreach (DataRow dr in HourData.Rows)
                    {
                        foreach (MonitoringPointEntity Point in PointLists)
                        {
                            if (Point.PointId == Convert.ToInt32(dr["PointId"]))
                            {
                                string pname = Point.MonitoringPointName + ";" + Convert.ToDateTime(dr["Tstamp"]).ToString("yyyy-MM-dd");
                                if (!N_dw.Contains(pname))
                                {
                                    N_dw.Add(pname);
                                }
                                break;
                            }
                        }
                    }
                    //解析提示信息
                    foreach (string str in N_dw)
                    {
                        msg += str.Split(';')[0] + " 已存在日期为：" + str.Split(';')[1] + " 的上报数据<br/>";
                    }
                }
                if (msg.Length > 0)
                {
                    div1.InnerHtml = msg;
                }
                #endregion

                #region 组装数据导入
                List<AirHourReportForUpLoadEntity> models = new List<AirHourReportForUpLoadEntity>();
                foreach (DataRow dr in dt.Rows)
                {
                    DateTime Tstamp = Convert.ToDateTime(Convert.ToString(dr["yyyy"]) + "-" + Convert.ToString(dr["mm"]) + "-" + Convert.ToString(dr["dd"]) + " " + Convert.ToString(Convert.ToInt32(dr["hh"]) - 1) + ":00:00");
                    int HourOfDay = Convert.ToInt32(dr["hh"]) - 1;
                    int PointId = 0;
                    foreach (MonitoringPointEntity Point in PointLists)
                    {
                        int PointDwcode = Point.MonitoringPointExtensionForEQMSAirEntity.Dwcode.Value;
                        if (Convert.ToInt32(dr["dwcode"]) == PointDwcode)
                        {
                            PointId = Point.PointId;
                        }
                    }
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (PollutantNames.Contains(dc.ColumnName.ToUpper()))
                        {
                            AirHourReportForUpLoadEntity model = new AirHourReportForUpLoadEntity();
                            model.PointId = PointId;
                            model.Tstamp = Tstamp;
                            model.HourOfDay = HourOfDay;
                            model.PollutantCode = GetFactorCode(dc.ColumnName.ToUpper());
                            model.PollutantValue = -1;
                            if (!string.IsNullOrEmpty(Convert.ToString(dr[dc.ColumnName])) && Convert.ToString(dr[dc.ColumnName]) != "-1")
                            {
                                model.PollutantValue = Convert.ToDecimal(dr[dc.ColumnName]);
                            }
                            model.DataType = DataType;
                            model.Description = "区县上报";
                            model.CreatUser = UserGuid;
                            model.CreatDateTime = DateTime.Now;
                            model.UpdateUser = UserGuid;
                            model.UpdateDateTime = DateTime.Now;
                            models.Add(model);
                        }
                    }
                }
                //清除历史数据
                g_DataQueryByHourForUpLoadService.DeleteByUser(DataType, UserGuid);
                if (models != null && models.Count > 0)
                {
                    g_DataQueryByHourForUpLoadService.InsertAll(models);
                }
                #endregion

                #region 预留生成标记为存储过程
                #endregion

                RadGrid1.Rebind();
            }
            #endregion

            #region 删除上传文件
            File.Delete(filePath);
            #endregion
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
            RadGrid1.Rebind();
        }
        #endregion

        #region 保存按钮方法
        /// <summary>
        /// 保存按钮方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            string UserGuid = SessionHelper.Get("DisplayName");
            string[] portIds = null;
            DateTime? dtEnd = null;
            DateTime? dtStart = null;
            var entity = g_DataQueryByHourForUpLoadService.Retrieve(x => x.CreatUser.Equals(UserGuid) && x.DataType.Equals(DataType));
            if (entity != null)
            {
                dtEnd = entity.Select(x => x.Tstamp).Max();
                dtStart = entity.Select(x => x.Tstamp).Min();
                portIds = entity.Select(x => x.PointId.ToString()).Distinct().ToArray<string>();
            }
            g_DataQueryByHourForUpLoadService.BatchAddAirHourReport(UserGuid, DataType, ApplicationType.Air);
            if (dtEnd != null && dtStart != null && portIds != null)
            {
                AuditDataService auditDataService = new AuditDataService();
                string errMsg = string.Empty;
                MonitoringPointAirService pointService = new MonitoringPointAirService();
                string[] regionGuids = pointService.GetRegionByPort(portIds);
                string[] cityGuids = pointService.GetCityByPort(portIds);
                auditDataService.GenerateDataFromHourReport(ApplicationType.Air, portIds, dtStart.Value, dtEnd.Value, out errMsg);
                auditDataService.GenerateDataRegion(ApplicationType.Air, regionGuids, dtStart.Value, dtEnd.Value, out errMsg);
                auditDataService.GenerateDataCity(ApplicationType.Air, cityGuids, dtStart.Value, dtEnd.Value, out errMsg);
            string a=auditDataService.CreateShengKongAQIHourExportDBFFile(dtStart.Value, dtEnd.Value);
            }
            Alert("保存成功！");
            RadGrid1.Rebind();
        }
        #endregion

        #region 获取对应的因子Code
        /// <summary>
        /// 获取对应的因子Code
        /// </summary>
        /// <param name="ExcelFactor"></param>
        /// <returns></returns>
        public string GetFactorCode(string ExcelFactor)
        {
            string FactorCode = "";
            switch (ExcelFactor.ToUpper())
            {
                case "SO2":
                    FactorCode = "a21026";
                    break;
                case "NO":
                    FactorCode = "a21003";
                    break;
                case "NO2":
                    FactorCode = "a21004";
                    break;
                case "NOX":
                    FactorCode = "a21002";
                    break;
                case "PM10":
                    FactorCode = "a34002";
                    break;
                case "CO":
                    FactorCode = "a21005";
                    break;
                case "O3":
                    FactorCode = "a05024";
                    break;
                case "PM2.5":
                    FactorCode = "a34004";
                    break;
                case "WS":
                    FactorCode = "a01007";
                    break;
                case "WD":
                    FactorCode = "a01008";
                    break;
                case "TEMP":
                    FactorCode = "a01001";
                    break;
                case "RH":
                    FactorCode = "a01002";
                    break;
                case "PRESS":
                    FactorCode = "a01006";
                    break;
                case "VISIBILITY":
                    FactorCode = "a01020";
                    break;
            }
            return FactorCode;
        }
        #endregion

        #region 根据标记位返回值
        /// <summary>
        /// 根据标记位返回值
        /// </summary>
        /// <param name="Value">值</param>
        /// <param name="Flag">标记位</param>
        /// <returns></returns>
        public string GetValue(object Value, object Flag)
        {
            double val = Convert.ToDouble(Math.Round(Convert.ToDecimal(Value), 3));
            if (string.IsNullOrEmpty(Convert.ToString(Flag)))
            {
                return val.ToString();
            }
            else
            {
                return "<font title='标记位：" + Flag.ToString().Split(',')[1] + "' color='red' style='font-weight:bold;'>" + val.ToString() + "(" + Flag.ToString().Split(',')[0] + ")</font>";
            }
        }
        #endregion
    }
}