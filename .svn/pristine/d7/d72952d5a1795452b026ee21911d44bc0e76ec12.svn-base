using Aspose.Cells;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.BaseData.Alarm;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.Service.Frame;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.WebUI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class HourReportUpLoad_SZ : BasePage
    {
        /// <summary>
        /// 字典信息服务接口
        /// </summary>
        DictionaryService g_DictionaryService = Singleton<DictionaryService>.GetInstance();

        /// <summary>
        /// 小时数据接口
        /// </summary>
        DataQueryByHourService g_DataQueryByHourService = Singleton<DataQueryByHourService>.GetInstance();

        /// <summary>
        /// 日数据接口
        /// </summary>
        DataQueryByDayService g_DataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();

        /// <summary>
        /// 上传临时表接口
        /// </summary>
        DataQueryByHourForUpLoadSZService g_DataQueryByHourForUpLoadSZService = Singleton<DataQueryByHourForUpLoadSZService>.GetInstance();

        /// <summary>
        /// 测点接口
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
        /// <summary>
        /// Excel报错类型
        /// </summary>
        string ProblemData = "";
        /// <summary>
        /// 数据处理
        /// </summary>
        private CreateAlarmService m_CreatAlarmService;
        /// <summary>
        /// 文档路径
        /// </summary>
        string SavePath = ConfigurationManager.AppSettings["SavePath"].ToString();

        /// <summary>
        /// 需要上传的监测因子
        /// </summary>
        List<string> PollutantCodes = new List<string>() { "a21026", "a21003", "a21004", "a21002", "a34002", "a21005", "a05024", "a34004", "a01007", "a01008", "a01001", "a01002", "a01006", "a01020" };

        List<string> PollutantNames = new List<string>() { "SO2", "NO", "NO2", "NOX", "PM10", "CO", "O3", "PM2.5", "WS", "WD", "TEMP", "RH", "PRESS", "VISIBILITY" };

        //List<string> PollutantNames2 = new List<string>() { "SO2(mg/m3)", "NO2(mg/m3)", "PM10(mg/m3)", "PM2.5(mg/m3)", "O3-8h(mg/m3)", "CO(mg/m3)" };

        List<string> PollutantNames2 = new List<string>() { "SO2", "NO2", "PM10", "PM2.5", "O3-8H", "CO" };

        /// <summary>
        /// 上传模板Sheet1中除配置因子以外的其它表头
        /// </summary>
        List<string> ExcelTitle = new List<string>() { "stcode", "stname", "yyyy", "mm", "dd", "hh", "dwcode", "dwname" };

        /// <summary>
        /// 上传模板Sheet2中除配置因子以外的其它表头
        /// </summary>
        List<string> ExcelTitle2 = new List<string>() { "日期", "城市", "点位", "备注" };

        /// <summary>
        /// 小时值数据类型
        /// </summary>
        int DataType = 1;

        /// <summary>
        /// 日均值数据类型
        /// </summary>
        int DataType2 = 2;

        string PointValue = "";
        string feilName = "";
        DateTime sertTime = DateTime.Now;
        /// <summary>
        /// 区县上报小时值报表站点名称以及对应站点ID
        /// </summary>
        private string m_QXUpLoadSZHourPointNoAndNames = System.Configuration.ConfigurationManager.AppSettings["QXUpLoadSZHourPointNoAndNames"].ToString();

        /// <summary>
        /// 区县上报日均值报表站点名称以及对应站点ID
        /// </summary>
        private string m_QXUpLoadSZDayPointNoAndNames = System.Configuration.ConfigurationManager.AppSettings["QXUpLoadSZDayPointNoAndNames"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();

                //DataTable dt = null;
                //dt = GetData();
                //gridQuxian.DataSource = dt;
                //gridQuxian.VirtualItemCount = dt.Rows.Count;
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            dtBegin.SelectedDate = DateTime.Now.AddDays(-1);
            dtEnd.SelectedDate = DateTime.Now;
            string UserGuid = SessionHelper.Get("DisplayName");
            //测点
            List<string> PointIds = new List<string>();
            string pointId = "";
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    PointIds.Add(item.Value);
                    pointId += item.Value + ",";
                }
            }
            if (PointIds == null || PointIds.Count == 0)
            {
                Alert("请选择监测点");
                return;
            }
            if (!string.IsNullOrWhiteSpace(pointId))
            {
                pointId = pointId.Substring(0, pointId.Length - 1);
            }

            string[] portIds = pointId.Split(',');
            string insertSerciveTime = this.dtBegin.SelectedDate.ToString();
            string inTime = this.dtEnd.SelectedDate.ToString();
            bool isInsertSercive = this.cbxInsertService.Checked;//判断是否勾选传入服务器
            string InsertSercive = Convert.ToInt32(isInsertSercive).ToString();
            bool isSuccess = this.cbxSuccess.Checked;//判断是否勾选导入成功
            string success = Convert.ToInt32(isSuccess).ToString();

            //DataTable dt = GetData(portIds, insertSerciveTime, inTime, InsertSercive, success); ;
            DataTable dt = GetData(portIds, insertSerciveTime, inTime, InsertSercive, success); ;
            if (dt.Rows.Count > 0)
            {
                gridQuxian.DataSource = dt;
                gridQuxian.VirtualItemCount = dt.Rows.Count;
            }
            else
            {
                gridQuxian.DataSource = dt;
            }
        }
        #endregion
        #region RadGrid绑定数据源
        /// <summary>
        /// RadGrid绑定数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridQuxian_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string UserGuid = SessionHelper.Get("DisplayName");
            //测点
            List<string> PointIds = new List<string>();
            string pointId = "";
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    PointIds.Add(item.Value);
                    pointId += item.Value + ",";
                }
            }
            if (PointIds == null || PointIds.Count == 0)
            {
                Alert("请选择监测点");
                return;
            }
            if (!string.IsNullOrWhiteSpace(pointId))
            {
                pointId = pointId.Substring(0, pointId.Length - 1);
            }

            string[] portIds = pointId.Split(',');
            string insertSerciveTime = this.dtBegin.SelectedDate.ToString();
            string inTime = this.dtEnd.SelectedDate.ToString();
            bool isInsertSercive = this.cbxInsertService.Checked;//判断是否勾选传入服务器
            string InsertSercive = Convert.ToInt32(isInsertSercive).ToString();
            bool isSuccess = this.cbxSuccess.Checked;//判断是否勾选导入成功
            string success = Convert.ToInt32(isSuccess).ToString();

            //DataTable dt = GetData(portIds, insertSerciveTime, inTime, InsertSercive, success); ;
            DataTable dt = GetData(portIds, insertSerciveTime, inTime, InsertSercive, success); ;
            if (dt.Rows.Count > 0)
            {
                gridQuxian.DataSource = dt;
                gridQuxian.VirtualItemCount = dt.Rows.Count;
            }
            else
            {
                gridQuxian.DataSource = dt;
            }
        }
        #endregion
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            //每页显示数据个数            
            int pageSize = gridQuxian.PageSize;
            //当前页的序号
            int pageNo = gridQuxian.CurrentPageIndex + 1;

            //数据总行数
            int recordTotal = 0;
            var dataView = m_CreatAlarmService.GetGridViewPager(pageSize, pageNo, GetWhereString(), out recordTotal);
            gridQuxian.DataSource = dataView;
            //数据总行数
            gridQuxian.VirtualItemCount = recordTotal;
        }
        #endregion

        /// <summary>
        /// 取得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhereString()
        {

            return null;
        }
        /// <summary>
        /// 获取滑动8小时平均
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetMaxO3(List<int> pointId, DataTable dt)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (int pointItem in pointId)
            {
                decimal Max = -1;
                for (int i = 1; i + 7 <= 24; i++)
                {
                    int dtCount = 0;
                    decimal value = 0;
                    DataTable dtNew = dt.Select("pointId=" + pointItem + " and hour>=" + i + " and hour<=" + (i + 7)).CopyToDataTable();
                    for (int j = 0; j < dtNew.Rows.Count; j++)
                    {
                        if (dtNew.Rows[j]["value"] != DBNull.Value)
                        {
                            if (decimal.Parse(dtNew.Rows[j]["value"].ToString()) != -1)
                            {
                                value += decimal.Parse(dtNew.Rows[j]["value"].ToString());
                                dtCount = dtCount + 1;
                            }
                        }
                    }
                    decimal tempMax = -1;
                    if (dtCount != 0 && dtCount >= 6)
                    {
                        tempMax = DecimalExtension.GetPollutantValue(value / dtCount, 3);
                    }
                    if (Max <= tempMax)
                    {
                        Max = tempMax;
                    }
                }
                dic.Add(pointItem.ToString(), Max.ToString());
            }
            return dic;
        }
        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(ReportLogEntity)))
            {

                dataTable.Columns.Add(pd.Name);

            }
            foreach (ReportLogEntity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(ReportLogEntity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }

        #region 上传按钮事件
        /// <summary>
        /// 上传按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpLoad_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ReportLogService ReportLogService = Singleton<ReportLogService>.GetInstance();
                IQueryable<ReportLogEntity> customDatumData = ReportLogService.CustomDatumRetrieve("HourReportUpLoad_SZ", 1);
                DataTable dtCustom = ConvertToDataTable(customDatumData);
                if (dtCustom.Rows.Count == 0)
                {
                    ReportLogEntity customDatum = new ReportLogEntity();
                    customDatum.PageTypeID = "HourReportUpLoad_SZ";//页面ID
                    customDatum.StartDateTime = DateTime.Now;
                    customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
                    customDatum.CreatUser = SessionHelper.Get("DisplayName");
                    ReportLogService.ReportLogAdd(customDatum);
                }
                else
                {
                    string TimeInterval = System.Configuration.ConfigurationManager.AppSettings["TimeInterval"];
                    DateTime dtTime = DateTime.Parse(dtCustom.Rows[0]["StartDateTime"].ToString());
                    int ID = int.Parse(dtCustom.Rows[0]["Id"].ToString());
                    string DisplayName = dtCustom.Rows[0]["CreatUser"].ToString();
                    if (dtTime.AddMinutes(int.Parse(TimeInterval)) <= DateTime.Now)
                    {
                        var customDatum = ReportLogService.ReportLogRetrieveByid(ID).FirstOrDefault();
                        customDatum.StartDateTime = DateTime.Now;
                        customDatum.CreatUser = SessionHelper.Get("DisplayName");
                        ReportLogService.ReportLogUpdate(customDatum);
                    }
                    else
                    {
                        int minuteInterval = dtTime.AddMinutes(int.Parse(TimeInterval)).Minute - DateTime.Now.Minute;
                        int SecondInterval = dtTime.AddMinutes(int.Parse(TimeInterval)).Second - DateTime.Now.Second;
                        if (DisplayName != SessionHelper.Get("DisplayName"))
                        {
                            if (minuteInterval > 0)
                            {
                                Alert(DisplayName + "正在操作，请" + minuteInterval + "分钟后执行！");
                            }
                            else
                            {
                                Alert(DisplayName + "正在操作，请" + SecondInterval + "秒后执行！");
                            }
                        }
                        else
                        {
                            Alert("您的操作过于频繁，请稍后执行！");
                        }
                        return;
                    }
                }
                DataTable dtLimt = g_DataQueryByHourForUpLoadSZService.GetFactorLimt(PollutantCodes);

                ProblemData = "";
                string UserGuid = SessionHelper.Get("DisplayName");
                //判断文件路径
                if (FileUpload1.PostedFile.FileName.Length == 0)
                {
                    Alert("请选择需要上传的文件");
                    return;
                }
                //文件名
                string filename = FileUpload1.FileName;
                //FileUpload1.PostedFile.ContentType
                string timeStr = null;
                foreach (char item in filename)
                {
                    if (item >= 48 && item <= 58)
                    {
                        timeStr += item;
                    }
                }
                string fileExtension = System.IO.Path.GetExtension(filename).ToLower();
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    Alert("上传的文件类型不正确，请上传EXCEL文件");
                    return;
                }

                //上传文件
                string NewFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + filename;
                if (filename.Contains("张家港"))
                    SavePath += "张家港/";
                if (filename.Contains("常熟"))
                    SavePath += "常熟/";
                if (filename.Contains("太仓"))
                    SavePath += "太仓/";
                if (filename.Contains("昆山"))
                    SavePath += "昆山/";
                if (filename.Contains("吴江"))
                    SavePath += "吴江/";

                string filePath = Server.MapPath(SavePath + NewFileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                FileUpload1.PostedFile.SaveAs(filePath);//将文件存储到服务器上

                #region 读取Excel
                Workbook excel = new Workbook(filePath);
                DataTable dtHourForDay = new DataTable(); //存储根据小时值 计算的日均值
                DataTable dtO3 = new DataTable();  // 存储侧点日臭氧时数据
                dtO3.Columns.Add("pointId", typeof(string));
                dtO3.Columns.Add("hour", typeof(int));
                dtO3.Columns.Add("value", typeof(string));
                dtHourForDay.Columns.Add("pointId", typeof(string));
                foreach (string itemName in PollutantNames2)
                {
                    dtHourForDay.Columns.Add(itemName.ToUpper(), typeof(string));
                    dtHourForDay.Columns.Add(itemName.ToUpper() + "Count", typeof(string));
                }

                #region Sheet1
                try
                {
                    if (excel.Worksheets.Count > 0)
                    {
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
                                string columnName = cl[0, i].StringValue.Trim().ToLower();
                                dt.Columns[i].ColumnName = columnName;
                                if (!ExcelTitle.Contains(columnName.ToLower()) && !PollutantNames.Contains(columnName.ToUpper()))
                                {
                                    isPollutant = false;
                                    Alert("Excel列 " + columnName + " 元素不在模板配置因子中");
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
                            int yyyyJ = 0, mmJ = 0, ddJ = 0;
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

                                            if (i == 0)
                                            {
                                                yyyyJ = Convert.ToInt32(dt.Rows[i][j]);
                                            }
                                            else
                                            {
                                                if (yyyy != yyyyJ)
                                                {
                                                    isError = true;
                                                    Alert("Excel第 " + (i + 2).ToString() + " 行，列yyyy数据格式有误，请确认");
                                                    break;
                                                }

                                            }

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
                                            if (i == 0)
                                            {
                                                mmJ = Convert.ToInt32(dt.Rows[i][j]);
                                            }
                                            else
                                            {
                                                if (mm != mmJ)
                                                {
                                                    isError = true;
                                                    Alert("Excel第 " + (i + 2).ToString() + " 行，列mm数据格式有误，请确认");
                                                    break;
                                                }

                                            }
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
                                            if (i == 0)
                                            {
                                                ddJ = Convert.ToInt32(dt.Rows[i][j]);
                                            }
                                            else
                                            {
                                                if (dd != ddJ)
                                                {
                                                    isError = true;
                                                    Alert("Excel第 " + (i + 2).ToString() + " 行，列dd数据格式有误，请确认");
                                                    break;
                                                }

                                            }
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
                                            DataRow drNew = dtHourForDay.NewRow();
                                            drNew["pointId"] = Point.PointId;
                                            dtHourForDay.Rows.Add(drNew);
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
                                    msg += str.Split(';')[0] + " 已存在日期为：" + str.Split(';')[1] + " 的上报小时数据<br/>";
                                }
                            }
                            if (msg.Length > 0)
                            {
                                div1.InnerHtml = msg;
                            }
                            else
                            {
                                div1.InnerHtml = string.Empty;
                            }
                            #endregion

                            #region 组装数据导入
                            List<AirHourReportForUpLoadSZEntity> models = new List<AirHourReportForUpLoadSZEntity>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                DataRow drO3 = dtO3.NewRow();
                                DateTime Tstamp = Convert.ToDateTime(Convert.ToString(dr["yyyy"]) + "-" + Convert.ToString(dr["mm"]) + "-" + Convert.ToString(dr["dd"]) + " " + Convert.ToString(Convert.ToInt32(dr["hh"]) - 1) + ":00:00");
                                int HourOfDay = Convert.ToInt32(dr["hh"]) - 1;
                                int PointId = 0;
                                string pointName = string.Empty;
                                string cityTypeUid = string.Empty;

                                foreach (MonitoringPointEntity Point in PointLists)
                                {
                                    int PointDwcode = Point.MonitoringPointExtensionForEQMSAirEntity.Dwcode.Value;
                                    if (Convert.ToInt32(dr["dwcode"]) == PointDwcode)
                                    {
                                        pointName = Point.MonitoringPointName;
                                        PointId = Point.PointId;
                                        cityTypeUid = Point.MonitoringPointExtensionForEQMSAirEntity.CityTypeUid;

                                    }
                                }
                                DataRow drTemp = dtHourForDay.Select("pointId=" + PointId).FirstOrDefault();
                                //decimal PM25Value = 0;
                                //decimal PM10Value = 0;
                                drO3["pointId"] = PointId;
                                drO3["hour"] = Convert.ToInt32(dr["hh"]);
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    if (PollutantNames.Contains(dc.ColumnName.ToUpper()))
                                    {
                                        string strPollutantCode = GetFactorCode(dc.ColumnName.ToUpper());
                                        DataRow[] drArry = dtLimt.Select("PointId=" + PointId + " and PollutantCode='" + strPollutantCode + "'");
                                        AirHourReportForUpLoadSZEntity model = new AirHourReportForUpLoadSZEntity();
                                        model.PointId = PointId;
                                        if (PointId == 21)
                                            PointValue = "4296ce53-78d3-4741-9eda-6306e3e5b399";
                                        if (PointId == 13)
                                            PointValue = "f7444783-a425-411c-a54b-f9fed72ec72e";
                                        if (PointId == 16)
                                            PointValue = "d993d02f-fcc3-4ea6-b52b-9414fbd9b8e6";
                                        if (PointId == 19)
                                            PointValue = "636775d8-091d-4754-9ed2-cd9dfef1f6ab";
                                        if (PointId == 10)
                                            PointValue = "48d749e6-d07c-4764-8d50-50f170defe0b";
                                        feilName = Tstamp.ToString("yyyy-MM-dd");
                                        model.MonitoringRegionUid = cityTypeUid;
                                        model.Tstamp = Tstamp;
                                        model.HourOfDay = HourOfDay;
                                        model.PollutantCode = strPollutantCode;
                                        model.PollutantValue = -1;
                                        if (!string.IsNullOrEmpty(Convert.ToString(dr[dc.ColumnName])) && Convert.ToString(dr[dc.ColumnName]) != "-1")
                                        {
                                            decimal dcPollutantValue = Convert.ToDecimal(dr[dc.ColumnName]);
                                            model.PollutantValue = dcPollutantValue;
                                            if (dtHourForDay.Columns.Contains(dc.ColumnName.ToUpper()))
                                            {
                                                drTemp[dc.ColumnName.ToUpper()] = decimal.Parse(drTemp[dc.ColumnName.ToUpper() + "Count"] != DBNull.Value ? drTemp[dc.ColumnName.ToUpper()].ToString() : "0") + dcPollutantValue;
                                                drTemp[dc.ColumnName.ToUpper() + "Count"] = int.Parse(drTemp[dc.ColumnName.ToUpper() + "Count"] != DBNull.Value ? drTemp[dc.ColumnName.ToUpper() + "Count"].ToString() : "0") + 1;
                                            }
                                            if (dc.ColumnName.ToUpper() == "O3")
                                            {
                                                drO3["value"] = dcPollutantValue;
                                            }
                                            //if (dc.ColumnName.ToUpper().Contains("PM2.5") || dc.ColumnName.ToUpper().Contains("PM25"))
                                            //{
                                            //    PM25Value = dcPollutantValue;
                                            //}
                                            //if (dc.ColumnName.ToUpper().Contains("PM10"))
                                            //{
                                            //    PM10Value = dcPollutantValue;
                                            //}
                                            if (drArry.Length > 0)
                                            {
                                                if (drArry[0]["ExcessiveUpper"] != DBNull.Value)
                                                {
                                                    if (decimal.Parse(drArry[0]["ExcessiveUpper"].ToString()) < dcPollutantValue)
                                                    {
                                                        //isRight = false;
                                                        //isRightData = false;
                                                        ProblemData += "\\n小时值：" + pointName + Tstamp.ToString("yyyy-MM-dd HH:mm:ss") + dc.ColumnName + "浓度超上限";
                                                    }
                                                }
                                                if (drArry[0]["ExcessiveLow"] != DBNull.Value)
                                                {
                                                    if (decimal.Parse(drArry[0]["ExcessiveLow"].ToString()) > dcPollutantValue)
                                                    {
                                                        //isRight = false;
                                                        //isRightData = false;
                                                        ProblemData += "\\n小时值：" + pointName + Tstamp.ToString("yyyy-MM-dd HH:mm:ss") + dc.ColumnName + "浓度超下限";
                                                    }
                                                }
                                            }
                                        }
                                        model.DataType = DataType;
                                        model.Description = "区县上报小时值";
                                        model.CreatUser = UserGuid;
                                        model.CreatDateTime = DateTime.Now;
                                        model.UpdateUser = UserGuid;
                                        model.UpdateDateTime = DateTime.Now;
                                        models.Add(model);
                                    }
                                }
                                dtO3.Rows.Add(drO3);
                                //if (PM25Value != 0 && PM10Value != 0 && PM25Value >= PM10Value)
                                //{
                                //    //isRightData = false;
                                //    ProblemData += "\\n小时值：" + pointName + Tstamp.AddHours(+1).ToString("yyyy-MM-dd HH:mm:ss") + "出现倒挂现象！";
                                //}
                            }
                            //清除历史数据
                            g_DataQueryByHourForUpLoadSZService.DeleteByUserSZ(DataType);
                            if (models != null && models.Count > 0)
                            {
                                g_DataQueryByHourForUpLoadSZService.InsertAllSZ(models);
                            }
                            Dictionary<string, string> dic = GetMaxO3(PointIds, dtO3);
                            foreach (DataRow drItem in dtHourForDay.Rows)
                            {
                                drItem["O3-8H".ToUpper()] = dic[drItem["pointId"].ToString()];
                            }
                            #endregion

                            #region 预留生成标记为存储过程
                            #endregion

                            //gridQuxian.Rebind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Alert("请确认小时值Excel输入格式！");
                    return;
                }
                #endregion

                #region Sheet2
                try
                {
                    if (excel.Worksheets.Count > 1)
                    {
                        Worksheet sheet2 = excel.Worksheets[1];
                        Cells cl2 = sheet2.Cells;

                        int column2 = cl2.MaxDataColumn;
                        int row2 = cl2.MaxDataRow;
                        row2 = row2 - 3;
                        if (column2 != 9)
                        {
                            Alert("请确认Excel是否缺少列元素");
                            return;
                        }
                        if (row2 >= 1)
                        {
                            //把表中因子值为“--”的转为“-1”
                            for (int i = 1; i <= row2; i++)
                            {
                                for (int j = 0; j <= column2; j++)
                                {
                                    string columnName = cl2[0, j].StringValue.Replace("(mg/m3)", "").Trim().ToLower();
                                    string columnValue = cl2[i, j].StringValue.Trim();
                                    if (PollutantNames2.Contains(columnName.ToUpper()) && columnValue == "--")
                                    {
                                        cl2[i, j].PutValue("-1");
                                    }
                                }
                            }

                            //获取数据
                            DataTable dt2 = cl2.ExportDataTable(1, 0, row2, column2 + 1);

                            #region 验证配置因子
                            bool isPollutant = true;
                            for (int i = 0; i < dt2.Columns.Count; i++)
                            {
                                string columnName = cl2[0, i].StringValue.Replace("(mg/m3)", "").Trim().ToLower();
                                dt2.Columns[i].ColumnName = columnName;
                                if (!ExcelTitle2.Contains(columnName.ToLower()) && !PollutantNames2.Contains(columnName.ToUpper()))
                                {
                                    isPollutant = false;
                                    Alert("Excel列 " + columnName + " 元素不在模板配置因子中");
                                    break;
                                }
                            }
                            if (!isPollutant)
                            {
                                return;
                            }
                            if (!dt2.Columns.Contains("DateTime"))
                            {
                                dt2.Columns.Add("DateTime");
                            }
                            #endregion

                            #region 验证数据有效性
                            bool isError = false;
                            List<string> pointNameList = new List<string>();
                            List<DateTime> DateLists = new List<DateTime>();
                            string strDate = string.Empty;//日期
                            string cityName = string.Empty;
                            string remark = string.Empty;
                            string strDateJ = string.Empty;//日期判断
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {
                                DateTime date = DateTime.Now;

                                #region 日期验证
                                for (int j = 0; j < dt2.Columns.Count; j++)
                                {
                                    //验证日期格式
                                    if (dt2.Columns[j].ColumnName == "日期")
                                    {
                                        try
                                        {
                                            if (!string.IsNullOrWhiteSpace(dt2.Rows[i][j].ToString()))
                                            {
                                                strDate = dt2.Rows[i][j].ToString();

                                                if (timeStr != strDate)
                                                {
                                                    Alert("日数据Excel中的日期应为：" + timeStr);
                                                    break;
                                                }
                                            }
                                            if (DateTime.TryParse(strDate.Substring(0, 4) + "-" + strDate.Substring(4, 2) + "-" + strDate.Substring(6, 2), out date))
                                            {
                                                //dt2.Rows[i][j] = date;
                                                dt2.Rows[i]["DateTime"] = date;
                                                if (j == 0)
                                                {
                                                    strDateJ = date.ToString();
                                                }
                                                else
                                                {
                                                    if (strDateJ != date.ToString())
                                                    {
                                                        isError = true;
                                                        Alert("Excel第 " + (i + 2).ToString() + " 行，列日期数据格式有误，应该为yyyymmdd");
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                isError = true;
                                                Alert("Excel第 " + (i + 2).ToString() + " 行，列日期数据格式有误，应该为yyyymmdd");
                                                break;
                                            }
                                            if (!DateLists.Contains(date))
                                            {
                                                DateLists.Add(date);
                                            }
                                        }
                                        catch
                                        {
                                            isError = true;
                                            Alert("Excel第 " + (i + 2).ToString() + " 行，列日期数据格式有误，应该为yyyymmdd");
                                            break;
                                        }
                                    }

                                    //城市
                                    else if (dt2.Columns[j].ColumnName == "城市")
                                    {
                                        if (string.IsNullOrWhiteSpace(dt2.Rows[i][j].ToString()))
                                        {
                                            dt2.Rows[i][j] = cityName;
                                        }
                                        else
                                        {
                                            cityName = dt2.Rows[i][j].ToString().Trim();
                                        }
                                    }

                                    //点位
                                    else if (dt2.Columns[j].ColumnName == "点位")
                                    {
                                        try
                                        {
                                            string pointName = dt2.Rows[i][j].ToString().Trim();

                                            if (!pointNameList.Contains(pointName))
                                            {
                                                pointNameList.Add(pointName);
                                            }
                                        }
                                        catch
                                        {
                                            isError = true;
                                            Alert("Excel第 " + (i + 2).ToString() + " 行，列点位数据格式有误，请确认");
                                            break;
                                        }
                                    }

                                    //备注
                                    else if (dt2.Columns[j].ColumnName == "备注")
                                    {
                                        if (string.IsNullOrWhiteSpace(dt2.Rows[i][j].ToString()))
                                        {
                                            dt2.Rows[i][j] = remark;
                                        }
                                        else
                                        {
                                            remark = dt2.Rows[i][j].ToString().Trim();
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
                            }
                            #endregion

                            #region 验证是否dwcode<=>PointId对应
                            List<int> PointIds = new List<int>();
                            IQueryable<MonitoringPointEntity> PointLists;//= g_MonitoringPointAirService.RetrieveAirMPList();//获取所有空气点位列表
                            //if (PointLists == null || PointLists.ToArray().Length == 0)
                            //{
                            //    Alert("未找到需要上报的配置测点");
                            //    return;
                            //}
                            bool isPoint = false;
                            foreach (string pointName in pointNameList)
                            {
                                //MonitoringPointEntity monitoringPointEntity = PointLists.FirstOrDefault(t => t.MonitoringPointName == pointName);

                                int pointId = GetPointIdByDayReportPointName(pointName);

                                if (pointId == 0)
                                {
                                    if (pointName != "城市均值")
                                    {
                                        isPoint = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    isPoint = true;
                                    if (!PointIds.Contains(pointId))
                                    {
                                        PointIds.Add(pointId);
                                    }
                                }
                            }
                            if (!isPoint)
                            {
                                return;
                            }
                            #endregion

                            #region 验证数据是否已存在，只提示
                            string msg = "";
                            DataTable dayData = g_DataQueryByDayService.GetDayData(PointIds, PollutantCodes, DateLists);
                            PointLists = g_MonitoringPointAirService.Retrieve(t => PointIds.Contains(t.PointId));

                            if (dayData != null && dayData.Rows.Count > 0)
                            {
                                List<string> N_dw = new List<string>();
                                foreach (DataRow dr in dayData.Rows)
                                {
                                    foreach (MonitoringPointEntity Point in PointLists)
                                    {
                                        if (Point.PointId == Convert.ToInt32(dr["PointId"]))
                                        {
                                            string pname = Point.MonitoringPointName + ";" + Convert.ToDateTime(dr["DateTime"]).ToString("yyyy-MM-dd");
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
                                    msg += str.Split(';')[0] + " 已存在日期为：" + str.Split(';')[1] + " 的上报日数据<br/>";
                                }
                            }
                            if (msg.Length > 0)
                            {
                                div1.InnerHtml = string.IsNullOrWhiteSpace(div1.InnerHtml) ? msg : div1.InnerHtml + "<br/>" + msg;
                            }
                            #endregion

                            #region 组装数据导入
                            List<AirHourReportForUpLoadSZEntity> models = new List<AirHourReportForUpLoadSZEntity>();

                            foreach (DataRow dr in dt2.Rows)
                            {
                                if (!dr["点位"].Equals("城市均值"))
                                {
                                    string pointName = dr["点位"].ToString();
                                    DateTime dateTime = Convert.ToDateTime(dr["DateTime"]);
                                    int PointId = GetPointIdByDayReportPointName(dr["点位"].ToString());
                                    string cityTypeUid = GetCityTypeUidByCityName(dr["城市"].ToString());
                                    DataRow drItem = dtHourForDay.Select("pointId=" + PointId).FirstOrDefault();

                                    //decimal PM25Value = 0;
                                    //decimal PM10Value = 0;
                                    foreach (DataColumn dc in dt2.Columns)
                                    {
                                        if (PollutantNames2.Contains(dc.ColumnName.ToUpper()))
                                        {
                                            string strPollutantCode = GetFactorCode(dc.ColumnName.Replace("(mg/m3)", "").ToUpper());
                                            DataRow[] drArry = dtLimt.Select("PointId=" + PointId + " and PollutantCode='" + strPollutantCode + "'");
                                            AirHourReportForUpLoadSZEntity model = new AirHourReportForUpLoadSZEntity();
                                            model.PointId = PointId;
                                            model.MonitoringRegionUid = cityTypeUid;
                                            model.Tstamp = dateTime;
                                            //model.HourOfDay = 0;
                                            model.PollutantCode = strPollutantCode;
                                            model.PollutantValue = -1;
                                            if (!string.IsNullOrEmpty(Convert.ToString(dr[dc.ColumnName]))
                                                && Convert.ToString(dr[dc.ColumnName]) != "-1" && Convert.ToString(dr[dc.ColumnName]) != "--")
                                            {
                                                decimal dcPollutantValue = Convert.ToDecimal(dr[dc.ColumnName]);
                                                model.PollutantValue = dcPollutantValue;
                                                if (drItem != null)
                                                {
                                                    if (dc.ColumnName.ToUpper().Contains("O3"))
                                                    {
                                                        if (drItem[dc.ColumnName] != DBNull.Value)
                                                        {
                                                            decimal tempValue = decimal.Parse(drItem[dc.ColumnName].ToString());
                                                            if (tempValue != dcPollutantValue)
                                                            {
                                                                //isRightData = false;
                                                                ProblemData += "\\n" + pointName + dc.ColumnName + "通过小时值计算日均值错误！";
                                                                //break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //isRightData = false;
                                                            ProblemData += "\\n" + pointName + dc.ColumnName + "通过小时值计算日均值错误！";
                                                            //break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (drItem[dc.ColumnName + "Count"] != DBNull.Value && drItem[dc.ColumnName] != DBNull.Value)
                                                        {
                                                            if (decimal.Parse(drItem[dc.ColumnName + "Count"].ToString()) >= 20)
                                                            {
                                                                //int unit = dc.ColumnName.ToUpper() == "CO" ? 1 : 3;
                                                                decimal tempValue1 = DecimalExtension.GetPollutantValue(decimal.Parse(drItem[dc.ColumnName].ToString()) / decimal.Parse(drItem[dc.ColumnName + "Count"].ToString()), 1);
                                                                decimal tempValue2 = DecimalExtension.GetPollutantValue(decimal.Parse(drItem[dc.ColumnName].ToString()) / decimal.Parse(drItem[dc.ColumnName + "Count"].ToString()), 2);
                                                                decimal tempValue3 = DecimalExtension.GetPollutantValue(decimal.Parse(drItem[dc.ColumnName].ToString()) / decimal.Parse(drItem[dc.ColumnName + "Count"].ToString()), 3);
                                                                if (tempValue1 != dcPollutantValue && tempValue2 != dcPollutantValue && tempValue3 != dcPollutantValue)
                                                                {
                                                                    //isRightData = false;
                                                                    ProblemData += "\\n" + pointName + dc.ColumnName + "通过小时值计算日均值错误！";
                                                                    //break;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //isRightData = false;
                                                            ProblemData += "\\n" + pointName + dc.ColumnName + "通过小时值计算日均值错误！";
                                                            //break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //isRightData = false;
                                                    ProblemData += "\\n" + pointName + dc.ColumnName + "通过小时值计算日均值错误！";
                                                    //break;
                                                }
                                                //if (dc.ColumnName.ToUpper().Contains("PM2.5") || dc.ColumnName.ToUpper().Contains("PM25"))
                                                //{
                                                //    PM25Value = dcPollutantValue;
                                                //}
                                                //if (dc.ColumnName.ToUpper().Contains("PM10"))
                                                //{
                                                //    PM10Value = dcPollutantValue;
                                                //}
                                                if (drArry.Length > 0)
                                                {
                                                    if (drArry[0]["ExcessiveUpper"] != DBNull.Value)
                                                    {
                                                        if (decimal.Parse(drArry[0]["ExcessiveUpper"].ToString()) < dcPollutantValue)
                                                        {
                                                            //isRightData = false;
                                                            ProblemData += "\\n日均值：" + dateTime.ToString("yyyy-MM-dd") + dc.ColumnName + "浓度超上限";
                                                        }
                                                    }
                                                    if (drArry[0]["ExcessiveLow"] != DBNull.Value)
                                                    {
                                                        if (decimal.Parse(drArry[0]["ExcessiveLow"].ToString()) > dcPollutantValue)
                                                        {
                                                            //isRightData = false;
                                                            ProblemData += "\\n日均值：" + dateTime.ToString("yyyy-MM-dd") + dc.ColumnName + "浓度超下限";
                                                        }
                                                    }
                                                }
                                            }
                                            model.DataType = DataType2;
                                            model.Description = "区县上报日均值";
                                            model.Remark = dr["备注"].ToString();
                                            model.CreatUser = UserGuid;
                                            model.CreatDateTime = DateTime.Now;
                                            model.UpdateUser = UserGuid;
                                            model.UpdateDateTime = DateTime.Now;
                                            models.Add(model);
                                        }
                                    }
                                    //if (PM25Value != 0 && PM10Value != 0 && PM25Value >= PM10Value)
                                    //{
                                    //    //isRightData = false;
                                    //    ProblemData += "\\n日均值：" + pointName + dateTime.ToString("yyyy-MM-dd") + "出现倒挂现象！";
                                    //}
                                }
                            }
                            //清除历史数据
                            g_DataQueryByHourForUpLoadSZService.DeleteByUserSZ(DataType2);
                            if (models != null && models.Count > 0)
                            {
                                g_DataQueryByHourForUpLoadSZService.InsertAllSZ(models);
                            }
                            #endregion

                            #region 预留生成标记为存储过程
                            #endregion

                            //RadGrid2.Rebind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Alert("请确认日均值Excel输入格式！");
                    return;
                }
                #endregion
                #endregion

                //#region 删除上传文件
                //File.Delete(filePath);
                //#endregion

                #region 保存记录到数据库
                if (excel.Worksheets.Count > 0)
                {
                    btnSave_Click(null, null);
                }
                #endregion

            }
            catch (Exception ex)
            {
                Alert("上传文件出错：" + ex.Message);
            }
            if (ProblemData != "")
            {
                Alert(ProblemData);
            }
            gridQuxian.Rebind();
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
            gridQuxian.Rebind();
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
            string dayDBFstr = string.Empty;
            string hourDBFstr = string.Empty;
            var entity = g_DataQueryByHourForUpLoadSZService.RetrieveSZ(x => x.CreatUser.Equals(UserGuid) && x.DataType.Equals(DataType));
            if (entity != null)
            {
                dtEnd = entity.Select(x => x.Tstamp).Max();
                dtStart = entity.Select(x => x.Tstamp).Min();
                portIds = entity.Select(x => x.PointId.ToString()).Distinct().ToArray<string>();
            }
            g_DataQueryByHourForUpLoadSZService.BatchAddAirHourReportSZ(UserGuid, DataType, ApplicationType.Air);
            g_DataQueryByHourForUpLoadSZService.BatchAddAirHourReportSZ(UserGuid, DataType2, ApplicationType.Air);
            if (dtEnd != null && dtStart != null && portIds != null)
            {
                AuditDataService auditDataService = new AuditDataService();
                string errMsg = string.Empty;
                MonitoringPointAirService pointService = new MonitoringPointAirService();
                string[] regionGuids = pointService.GetRegionByPort(portIds);
                string[] cityGuids = pointService.GetCityByPort(portIds);
                auditDataService.GenerateDataFromDayReport(ApplicationType.Air, portIds, dtStart.Value, dtEnd.Value, out errMsg);
                auditDataService.GenerateDataRegionFromDay(ApplicationType.Air, regionGuids, dtStart.Value, dtEnd.Value, out errMsg);
                auditDataService.GenerateDataCityFromDay(ApplicationType.Air, cityGuids, dtStart.Value, dtEnd.Value, out errMsg);

                dayDBFstr = auditDataService.CreateShengKongAQIHourExportDBFFile(dtStart.Value, dtEnd.Value);
                hourDBFstr = auditDataService.CreateShengKongAQIDayExportDBFFile(dtStart.Value, dtEnd.Value);
                if (dayDBFstr != "" || hourDBFstr != "")
                    ProblemData = "文件上报失败，请重新导入！";
            }

            #region 保存小时值和日均值统计数据到数据库

            DataTable dtNew = g_DataQueryByHourForUpLoadSZService.GetData(PointValue, feilName).ToTable();
            if (dtNew.Rows.Count <= 0)
            {
                g_DataQueryByHourForUpLoadSZService.GetAddData(PointValue, DateTime.Now, UserGuid, feilName);
            }
            #endregion
            if (ProblemData == "")
            {
                ProblemData = UserGuid + "于" + DateTime.Now + "手动导入！";
                g_DataQueryByHourForUpLoadSZService.insertQuXianData(ProblemData.Replace("\\n", "<br />"), PointValue, feilName);
                Alert("保存成功！" + ProblemData);
            }
            else if (ProblemData.Contains("文件上报失败，请重新导入！"))
            {
                g_DataQueryByHourForUpLoadSZService.GetUpdateData(PointValue, feilName, ProblemData);
                Alert(ProblemData);
            }
            else
            {
                g_DataQueryByHourForUpLoadSZService.insertQuXianData(ProblemData.Replace("\\n", "<br />"), PointValue, feilName);
                Alert("保存成功！" + ProblemData);
            }

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
                case "O3-8H":
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

        #region 根据日均值报表中的站点名称获取对应的站点ID
        /// <summary>
        /// 根据日均值报表中的站点名称获取对应的站点ID
        /// </summary>
        /// <param name="pointName"></param>
        /// <returns></returns>
        private int GetPointIdByDayReportPointName(string pointName)
        {
            int pointId = 0;
            string[] QXUpLoadSZDayPointNoAndNameArray = m_QXUpLoadSZDayPointNoAndNames.Split(';');
            string strPointId = QXUpLoadSZDayPointNoAndNameArray.FirstOrDefault(t => t.EndsWith(pointName) & t.Length == (pointName.Length + 3));

            if (!string.IsNullOrWhiteSpace(strPointId))
            {
                strPointId = strPointId.Replace(":" + pointName, "");
                pointId = int.TryParse(strPointId, out pointId) ? pointId : 0;
            }
            return pointId;
        }
        #endregion

        #region 根据城市名称获取城市对应的GUID
        /// <summary>
        /// 根据城市名称获取城市对应的GUID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        private string GetCityTypeUidByCityName(string cityName)
        {
            string cityTypeUid = string.Empty;
            switch (cityName)
            {
                case "常熟":
                    cityTypeUid = SmartEP.Core.Enums.EnumMapping.GetDesc(CityType.ChangShu).Split(':')[1];
                    break;
                case "昆山":
                    cityTypeUid = SmartEP.Core.Enums.EnumMapping.GetDesc(CityType.KunShan).Split(':')[1];
                    break;
                case "太仓":
                    cityTypeUid = SmartEP.Core.Enums.EnumMapping.GetDesc(CityType.TaiCang).Split(':')[1];
                    break;
                case "吴江":
                    cityTypeUid = SmartEP.Core.Enums.EnumMapping.GetDesc(CityType.WuJiang).Split(':')[1];
                    break;
                case "张家港":
                    cityTypeUid = SmartEP.Core.Enums.EnumMapping.GetDesc(CityType.ZhangJiaGang).Split(':')[1];
                    break;
                default:
                    break;
            }
            return cityTypeUid;
        }
        #endregion

        #region 行数据处理
        protected void gridQuxian_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;

                if (item["IsInsertService"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["IsInsertService"];

                    if (pointCell.Text == "True")
                    {
                        //pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/icons/Green.PNG\" />";
                        pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/Images/icons/Green.png\" />";
                    }
                    else if (pointCell.Text == "False")
                    {
                        //pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/icons/red.PNG\" />";
                        pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/Images/icons/Red.png\" />";
                    }
                    else
                    {
                        pointCell.Text = "--";

                    }
                }

                if (item["IsSuccess"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["IsSuccess"];
                    if (pointCell.Text == "True")
                    {
                        // pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/icons/Green.PNG\" />";
                        pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/Images/icons/Green.png\" />";
                    }
                    else if (pointCell.Text == "False")
                    {
                        //pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/icons/red.PNG\" />";
                        pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/Images/icons/Red.png\" />";
                    }
                    else if (string.IsNullOrWhiteSpace(drv["IsSuccess"].ToString()))
                    {
                        pointCell.Text = "--";

                    }
                }

            }
        }
        #endregion
        public DataTable GetData(string[] PointIds, string insertSerciveTime, string inTime, string isInsertSercive, string isSuccess)
        {

            DataTable dt = null;
            string strSql = @"SELECT PointGuid,PointName,Tstamp,IsInsertService,InsertServiceTime,IsSuccess,InsertTime,ProblemData,CreaterUser FROM [dbo].[TB_QuXianData] WHERE 1=1";
            DatabaseHelper db = new DatabaseHelper();
            if (PointIds != null && PointIds.Length > 0)
            {
                // strSql += " AND PointId IN ('" + PointIds + "')";
                strSql += string.Format(" AND PointGuid in ('{0}')", StringExtensions.GetArrayStrNoEmpty(PointIds.ToList<string>(), "','"));
            }
            //时间范围
            if (dtBegin.SelectedDate != null)
                //strSql += string.Format(" AND Tstamp >= '{0}'", dtBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                strSql += string.Format(" AND Tstamp >= '{0}'", dtBegin.SelectedDate.Value.ToString("yyyy-MM-dd"));
            if (dtEnd.SelectedDate != null)
                strSql += string.Format(" AND Tstamp <= '{0}'", dtEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));
            if (isInsertSercive == "1")
            {
                strSql += " AND IsInsertService = '1' ";
            }
            if (isSuccess == "1")
            {
                strSql += " AND IsSuccess = '1' order by PointGuid desc,tstamp desc";
            }

            dt = db.ExecuteDataTable(strSql, "AMS_MonitoringBusinessConnection");
            return dt;
        }

        //test
        public DataTable GetData()
        {
            DataTable dt = null;
            string strSql = @"SELECT PointGuid,PointName,Tstamp,IsInsertService,InsertServiceTime,IsSuccess,InsertTime,ProblemData,CreaterUser FROM [dbo].[TB_QuXianData] WHERE 1=1 order by PointGuid desc,tstamp desc";
            DatabaseHelper db = new DatabaseHelper();

            dt = db.ExecuteDataTable(strSql, "AMS_MonitoringBusinessConnection");
            return dt;
        }

        protected void btnExcel_Click(object sender, ImageClickEventArgs e)
        {
            if (dtBegin.SelectedDate == null || dtEnd.SelectedDate == null)
            {
                Alert("开始时间或者截止时间，不能为空！");
                return;
            }
            else if (dtBegin.SelectedDate > dtEnd.SelectedDate)
            {
                Alert("开始时间不能大于截止时间！");
                return;
            }
            List<string> PointIds = new List<string>();
            string pointId = "";
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    PointIds.Add(item.Value);
                    pointId += item.Value + ",";
                }
            }
            if (PointIds == null || PointIds.Count == 0)
            {
                Alert("请选择监测点");
                return;
            }
            if (!string.IsNullOrWhiteSpace(pointId))
            {
                pointId = pointId.Substring(0, pointId.Length - 1);
            }

            string[] portIds = { "10", "11", "12", "13", "14", "15", "16", "17", "19", "20", "21", "22" };
            DateTime dtStart = dtBegin.SelectedDate.Value;
            DateTime dtTo = dtEnd.SelectedDate.Value;
            AuditDataService m_auditDataService = new AuditDataService();
            DataView dv = m_auditDataService.getDataViewDayXJSNew(dtStart, dtTo, portIds);
            DataTableToExcel(dv, "区县数据", "区县数据");
        }

        /// <summary>
        /// 导出空气质量实时报
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private void DataTableToExcel(DataView dv, string fileName, string sheetName)
        {
            DataTable dtNew = dv.ToTable();
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = sheet.Cells;
            Aspose.Cells.Style cellStyle = workbook.Styles[workbook.Styles.Add()];
            workbook.FileName = fileName;
            sheet.Name = sheetName;
            cellStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            cellStyle.Font.Name = "宋体"; //文字字体
            cellStyle.Font.Size = 12;//文字大小
            cellStyle.IsLocked = false; //单元格解锁
            cellStyle.IsTextWrapped = true; //单元格内容自动换行

            #region 表头
            //第一行
            cells[0, 0].PutValue("日期");
            cells.Merge(0, 0, 1, 1);
            cells[0, 1].PutValue("城市");
            cells.Merge(0, 1, 1, 1);
            cells[0, 2].PutValue("点位");
            cells.Merge(0, 2, 1, 1);
            cells[0, 3].PutValue("SO2(mg/m3)");
            cells.Merge(0, 3, 1, 1);
            cells[0, 4].PutValue("NO2(mg/m3)");
            cells.Merge(0, 4, 1, 1);
            cells[0, 5].PutValue("PM10(mg/m3)");
            cells.Merge(0, 5, 1, 1);
            cells[0, 6].PutValue("PM2.5(mg/m3)");
            cells.Merge(0, 6, 1, 1);
            cells[0, 7].PutValue("O3-8h(mg/m3)");
            cells.Merge(0, 6, 1, 1);
            cells[0, 8].PutValue("CO(mg/m3)");
            cells.Merge(0, 6, 1, 1);

            cells.SetRowHeight(0, 30);//设置行高
            for (int i = 0; i <= 8; i++)
            {
                cells.SetColumnWidth(i, 15);//设置列宽
            }
            #endregion

            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 1;
                cells[rowIndex, 0].PutValue(string.Format("{0:yyyy-MM-dd}", drNew["DateTime"]));
                cells[rowIndex, 1].PutValue(drNew["STNAME"].ToString());
                cells[rowIndex, 2].PutValue(drNew["PointName"].ToString());
                cells[rowIndex, 3].PutValue(drNew["SO2"].ToString());
                cells[rowIndex, 4].PutValue(drNew["NO2"].ToString());
                cells[rowIndex, 5].PutValue(drNew["PM10"].ToString());
                cells[rowIndex, 6].PutValue(drNew["PM25"].ToString());
                cells[rowIndex, 7].PutValue(drNew["Max8HourO3"].ToString());
                cells[rowIndex, 8].PutValue(drNew["CO"].ToString());
            }
            foreach (Cell cell in cells)
            {
                if (!cell.IsStyleSet)
                {
                    cell.SetStyle(cellStyle);
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss")));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.BinaryWrite(workbook.SaveToStream().ToArray());
            Response.End();
        }
    }
}