using Aspose.Cells;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Caching;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class CheckDataUpload : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 文档路径
        /// </summary>
        string SavePath = "~/Files/";
        /// <summary>
        /// 需要上传的监测因子
        /// </summary>
        //List<string> PollutantCodes = new List<string>() { "a21026", "a21003", "a21004", "a21002", "a34002", "a21005", "a05024", "a34004", "a01007", "a01008", "a01001", "a01002", "a01006", "a01020" };
        List<string> PollutantNames = new List<string>() { "SO2", "NO2", "PM10", "PM25", "CO", "O3_1H", "O3_8H" };
        /// <summary>
        /// 上传模板中除配置因子以外的其它表头
        /// </summary>
        List<string> ExcelTitle = new List<string>() { "portid", "portname", "datetime" };
        /// <summary>
        /// 测点接口
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();

        DataQueryByDayService m_DataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //国控点，对照点，路边站
                //string strpointName = "";
                //IQueryable<MonitoringPointEntity> EnableOrNotports = g_MonitoringPointAirService.RetrieveAirMPListByCountryControlled();
                //string[] EnableOrNotportsarry = EnableOrNotports.Select(p => p.MonitoringPointName).ToArray();
                //foreach (string point in EnableOrNotportsarry)
                //{
                //    strpointName += point + ";";
                //}
                //pointCbxRsm.SetPointValuesFromNames(strpointName);
                InitControl();
            }
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            DataTable dvType = m_DataQueryByDayService.GetCheckDataType();
            ddlType.DataSource = dvType;
            ddlType.DataTextField = "DataType";
            ddlType.DataValueField = "DataType";
            ddlType.DataBind();
        }

        protected void btnUpLoad_Click(object sender, ImageClickEventArgs e)
        {
            string DataType = tbType.Text;
            string UserGuid = SessionHelper.Get("DisplayName");
            //判断文件路径
            if (FileUpload1.PostedFile.FileName.Length == 0)
            {
                Alert("请选择需要上传的文件");
                return;
            }
            if (tbType.Text == "")
            {
                Alert("请输入考核基数类型");
                return;
            }
            DataTable dvType = m_DataQueryByDayService.GetCheckDataType();
            DataRow[] dr = dvType.Select("DataType ='" + tbType.Text + "'");
            //if (dr.Length != 0)
            //{
            //    RegisterScript("clear1()");
            //}
            //文件名
            string filename = FileUpload1.FileName;
            //上传文件
            string NewFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + filename;
            string filePath = Server.MapPath(SavePath + NewFileName);
            string fileExtension = System.IO.Path.GetExtension(filename).ToLower();
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                Alert("文件类型不正确，请选择Excel格式文件！");
                return;
            }
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
            if (column != 9)
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
                IQueryable<MonitoringPointEntity> PointLists = g_MonitoringPointAirService.RetrieveAirMPList();
                bool isError = false;
                List<string> DWnames = new List<string>();
                List<DateTime> DateLists = new List<DateTime>();
                List<string> PointIds = new List<string>();
                string dwname = "";
                //验证日期格式
                DateTime date = DateTime.Now;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName == "datetime")
                        {
                            try
                            {
                                date = Convert.ToDateTime(dt.Rows[i][j]);
                            }
                            catch
                            {
                                isError = true;
                                Alert("Excel第 " + (i + 2) + " 行，列dateTime数据格式有误，请确认");
                                break;
                            }
                        }
                        else if (dt.Columns[j].ColumnName == "portid")
                        {

                            try
                            {
                                dwname = dt.Rows[i][j].ToString();
                                IQueryable<MonitoringPointEntity> portList = PointLists.Where(x => x.PointId == int.Parse(dwname));
                                if (portList == null || portList.ToArray().Length == 0)
                                {
                                    Alert("未找到需要导入的配置测点" + dwname);
                                    return;
                                }
                                foreach (MonitoringPointEntity Point in portList)
                                {
                                    int strId = Point.PointId;
                                    if (!PointIds.Contains(strId.ToString()))
                                    {
                                        PointIds.Add(strId.ToString());
                                    }
                                }
                            }
                            catch
                            {
                                isError = true;
                                Alert("Excel第 " + (i + 2).ToString() + " 行，列portName数据格式有误，请确认");
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
                    if (!DWnames.Contains(dwname))
                    {
                        DWnames.Add(dwname);
                    }
                    if (!DateLists.Contains(date))
                    {
                        DateLists.Add(date);
                    }
                }
                #endregion
                #region 组装数据导入
                m_DataQueryByDayService.insertTable(dt, DataType, PointIds.ToArray());
                #endregion
                DataTable dddd = dt;
                #region 验证是否portName<=>PointId对应
                #endregion
                InitControl();
            }
            #endregion

            #region 删除上传文件
            File.Delete(filePath);
            #endregion
            Alert("上传成功！");
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            RadGrid1.CurrentPageIndex = 0;
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            string[] pointIds = pointCbxRsm.GetPointValues(Core.Enums.CbxRsmReturnType.ID);
            string DataType = ddlType.SelectedText.ToString();
            DataView dv = m_DataQueryByDayService.GetCheckDataDayAQI(pointIds, DataType);
            RadGrid1.DataSource = dv;
        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["DateTime"] != null)
                {
                    GridTableCell DateTimeCell = (GridTableCell)item["DateTime"];
                    DateTime dtime = DateTime.Parse(DateTimeCell.Text.Trim());
                    DateTimeCell.Text = dtime.Month + "月" + dtime.Day + "日";
                }
            }
        }
    }
}