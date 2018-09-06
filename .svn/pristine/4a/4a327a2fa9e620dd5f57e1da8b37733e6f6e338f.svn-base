using SmartEP.Utilities.Office;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Service.Core.Enums;
using SmartEP.WebControl.CbxRsm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.DomainModel.BaseData;
using SmartEP.Core.Generic;
using SmartEP.Service.BaseData.BusinessRule;
using System.ComponentModel;
using System.Collections;
using SmartEP.Utilities.Web.UI;
using SmartEP.Service.Frame;
using SmartEP.DomainModel;

namespace SmartEP.WebUI.Pages.EnvAir.RealTimeData
{
    /// <summary>
    /// 名称：RealTimeData.aspx.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：环境空气实时数据操作
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class RealTimeData : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private IInfectantDALService g_IInfectantDALService = null;
        InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
        //服务处理
        MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
        SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = Singleton<SmartEP.Service.BaseData.Channel.AirPollutantService>.GetInstance();
        //获取因子上下限数据处理服务
        ExcessiveSettingService m_ExcessiveSettingService = new ExcessiveSettingService();
        //代码项服务层
        DictionaryService dicService = new DictionaryService();
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        string LZSPfactor = string.Empty;
        string LZSfactorName = string.Empty;
        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;

        /// <summary>
        /// 上下限
        /// </summary>
        DataTable dtExcessive = null;
        /// <summary>
        /// 国家标记位
        /// </summary>
        IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string PointIds = PageHelper.GetQueryString("pointNames");
                string FactorCode = PageHelper.GetQueryString("FactorName");
                if (PointIds != "")
                {
                    string[] pointIdarry = PointIds.Trim(';').Split(';');
                    string pointName = "";
                    foreach (string pointId in pointIdarry)
                    {
                        MonitoringPointEntity monitoringPointEntity = m_MonitoringPointAirService.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        pointName += monitoringPointEntity.MonitoringPointName + ";";
                    }
                    pointCbxRsm.SetPointValuesFromNames(pointName);
                }
                else
                {
                    //国控点，对照点，路边站
                    MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
                    string strpointName = "";
                    IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
                    string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad" || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
                    foreach (string point in EnableOrNotportsarry)
                    {
                        strpointName += point + ";";
                    }
                    pointCbxRsm.SetPointValuesFromNames(strpointName);
                }
                if (!string.IsNullOrWhiteSpace(FactorCode))
                {
                    string[] strFactorCodes = FactorCode.Split(';');
                    string PollutantName = string.Empty;
                    foreach (string strFactorCode in strFactorCodes)
                    {
                        PollutantCodeEntity pollutantCodeEntity = m_AirPollutantService.RetrieveEntityByCode(strFactorCode);
                        PollutantName += pollutantCodeEntity.PollutantName + ";";
                    }
                    //PollutantCodeEntity pollutantCodeEntity = m_AirPollutantService.RetrieveEntityByCode(FactorCode);
                    //string PollutantName = pollutantCodeEntity.PollutantName;
                    factorCbxRsm.SetFactorValuesFromNames(PollutantName);
                }
                else
                {
                    string pollutantName = System.Configuration.ConfigurationManager.AppSettings["AirPollutant"];
                    factorCbxRsm.SetFactorValuesFromNames(pollutantName);
                }
                InitControl();
            }
            //数据类型对应接口初始化
            g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue));
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //数据类型
            //radlDataType.Items.Add(new ListItem("一分钟", PollutantDataType.Min1.ToString()));
            radlDataType.Items.Add(new ListItem("五分钟", PollutantDataType.Min5.ToString()));
            radlDataType.Items.Add(new ListItem("一小时", PollutantDataType.Min60.ToString()));
            radlDataType.SelectedValue = PollutantDataType.Min60.ToString();
        }


        #endregion
        #region 绑定因子
        public void BindFactors(string CategoryUid, out string Name, out string code, string type = "name")
        {
            IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveList().Where(x => x.CategoryUid == CategoryUid);
            string PollutantName = "";
            string PollutantCode = "";
            //if (type == "name")
            //{
            string[] pollutantarry = Pollutant.Select(p => p.PollutantName).ToArray();
            foreach (string strName in pollutantarry)
            {
                PollutantName += strName + ";";
            }
            //}
            //else
            //{
            string[] pollutantCodearry = Pollutant.Select(p => p.PollutantCode).ToArray();
            foreach (string strName in pollutantCodearry)
            {
                PollutantCode += strName + ";";
            }
            //}
            Name = PollutantName;
            code = PollutantCode;
        }
        #endregion
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {

            //if (!IsPostBack)
            //{
            //    //因子关联
            //    pointCbxRsm_SelectedChanged();

            //}
            BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", out LZSfactorName, out LZSPfactor);

            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            DateTime dtBegion = DateTime.Now.AddHours(-24);
            DateTime dtEnd = DateTime.Now;
            if (radlDataType.SelectedValue == "Min5")
            {
                dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:mm"));
                dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:mm"));
            }
            else if (radlDataType.SelectedValue == "Min60")
            {
                dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:mm"));
                dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:mm"));
            }
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            factors = factorCbxRsm.GetFactors();

            //生成RadGrid的绑定列
            dvStatistical = null;
            //是否显示统计行
            if (IsStatistical.Checked)
            {
                gridRT.ShowFooter = true;
                dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
            }
            else
            {
                gridRT.ShowFooter = false;
            }
            //每页显示数据个数            
            int pageSize = gridRT.PageSize;
            //当前页的序号
            int pageNo = gridRT.CurrentPageIndex;

            //数据总行数
            int recordTotal = 0;

            //【给隐藏域赋值，用于显示Chart】
            SetHiddenData(portIds, factors, dtBegion, dtEnd);

            //排序
            string orderby = "PointId,Tstamp desc";
            if (TimeSort.SelectedValue == "时间升序")
                orderby = "PointId,Tstamp asc";
            var monitorData = new DataView();
            if (portIds != null)
            {
                monitorData = g_IInfectantDALService.GetDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderby);

                //for (int i = 0; i < monitorData.Count; i++)
                //{
                //    for (int iRow = 0; iRow < factors.Count; iRow++)
                //    {
                //        IPollutant factor = factors[iRow];
                //        //GridTableCell cell = (GridTableCell)monitorData[i][iRow];
                //        decimal value = 0M;
                //        if (decimal.TryParse(monitorData[i][factors[iRow].PollutantCode].ToString(), out value))
                //        {
                //            if (factor.PollutantMeasureUnit == "μg/m3")
                //            {
                //           monitorData[i][factors[iRow].PollutantCode] = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);

                //            }
                //            //else
                //            //{
                //            //    value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                //            //}   
                //        }
                //    }
                //}

                gridRT.DataSource = monitorData;
            }
            else
            {
                gridRT.DataSource = null;
            }

            //获取上下限
            string DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
            IQueryable<ExcessiveSettingInfo> Excessive = m_ExcessiveSettingService.RetrieveListByPointAndFactor(ApplicationValue.Air, factorCodes, portIds, DataTypeUid);
            dtExcessive = ConvertToDataTable(Excessive);
            //国家数据标记位
            siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "国家数据标记");
            //DataTable dt = monitorData.ToTable();
            //if (radlDataType.SelectedValue == "Min60")
            //{
            //    if (monitorData.Count < (portIds.Length * Convert.ToInt32(ddlTotala.SelectedText)))
            //    {

            //        foreach (string portid in portIds)
            //        {
            //            DataRow[] dr = dt.Select("PointId='" + portid + "'");
            //            if (dr.Count() == ddlTotala.Items.Count)
            //            {

            //            }
            //            else
            //            {
            //                for (int i = 0; i < Convert.ToInt32(ddlTotala.SelectedText); i++)
            //                {
            //                    string strDt = DateTime.Now.AddHours(-i).ToString("yyyy-MM-dd HH:mm:mm");
            //                    DataRow[] drnew = dt.Select("PointId='" + portid + "'and  Tstamp='" + Convert.ToDateTime(strDt) + "'");
            //                    if (drnew.Count() == 0)
            //                    {
            //                        DataRow mdr = dt.NewRow();
            //                        mdr["PointId"] = portid;
            //                        mdr["Tstamp"] = Convert.ToDateTime(strDt);
            //                        dt.Rows.Add(mdr);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //else if (radlDataType.SelectedValue == "Min1")
            //{
            //    if (monitorData.Count < (portIds.Length * Convert.ToInt32(ddlTotalb.SelectedText)))
            //    {

            //        foreach (string portid in portIds)
            //        {
            //            DataRow[] dr = dt.Select("PointId='" + portid + "'");
            //            if (dr.Count() == ddlTotalb.Items.Count)
            //            {

            //            }
            //            else
            //            {
            //                for (int i = 0; i < Convert.ToInt32(ddlTotalb.SelectedText); i++)
            //                {
            //                    string strDt = DateTime.Now.AddMinutes(-i).ToString("yyyy-MM-dd HH:mm:mm");
            //                    DataRow[] drnew = dt.Select("PointId='" + portid + "'and  Tstamp='" + Convert.ToDateTime(strDt) + "'");
            //                    if (drnew.Count() == 0)
            //                    {
            //                        DataRow mdr = dt.NewRow();
            //                        mdr["PointId"] = portid;
            //                        mdr["Tstamp"] = Convert.ToDateTime(strDt);
            //                        dt.Rows.Add(mdr);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //else if (radlDataType.SelectedValue == "Min5")
            //{
            //    if (monitorData.Count < (portIds.Length * Convert.ToInt32(ddlTotalb.SelectedText)))
            //    {

            //        foreach (string portid in portIds)
            //        {
            //            DataRow[] dr = dt.Select("PointId='" + portid + "'");
            //            if (dr.Count() == ddlTotalb.Items.Count)
            //            {

            //            }
            //            else
            //            {
            //                for (int i = 0; i < Convert.ToInt32(ddlTotalb.SelectedText); i++)
            //                {
            //                    int Mint = (DateTime.Now.AddMinutes(-(5 * i)).Minute) % 10;
            //                    string strDt = "";
            //                    if (Mint >= 5)
            //                    {
            //                        strDt = DateTime.Now.AddMinutes(-Mint + 5 - (5 * i)).ToString("yyyy-MM-dd HH:mm:mm");
            //                    }
            //                    else
            //                    {
            //                        strDt = DateTime.Now.AddMinutes(-Mint - (5 * i)).ToString("yyyy-MM-dd HH:mm:mm");
            //                    }
            //                    DataRow[] drnew = dt.Select("PointId='" + portid + "'and  Tstamp='" + Convert.ToDateTime(strDt) + "'");
            //                    if (drnew.Count() == 0)
            //                    {
            //                        DataRow mdr = dt.NewRow();
            //                        mdr["PointId"] = portid;
            //                        mdr["Tstamp"] = Convert.ToDateTime(strDt);
            //                        dt.Rows.Add(mdr);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            gridRT.VirtualItemCount = recordTotal;
        }

        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 页面隐藏域控件赋值，将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, IList<IPollutant> factors, DateTime dtBegin, DateTime dtEnd)
        {
            if (factors.Count > 0 && portIds != null)
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Air";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRT_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //string DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
            //string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            //string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            //IQueryable<ExcessiveSettingInfo> Excessive = m_ExcessiveSettingService.
            //    RetrieveListByPointAndFactor(ApplicationValue.Air, factorCodes, portIds, DataTypeUid);
            //DataTable dtExcessive = ConvertToDataTable(Excessive);
            ////国家数据标记位
            //IQueryable<V_CodeMainItemEntity> siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "国家数据标记");

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                points = pointCbxRsm.GetPoints();
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    var m = pointCell.Text.Trim();
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                    if (points != null)
                        pointCell.Text = point.PointName;
                }
                for (int iRow = 0; iRow < factors.Count; iRow++)
                {
                    string siteTypeName = "--";//标记位名称
                    IPollutant factor = factors[iRow];
                    GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                    string factorStatus = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                    string factorMark = drv[factor.PollutantCode + "_Mark"] != DBNull.Value ? drv[factor.PollutantCode + "_Mark"].ToString() : string.Empty;
                    decimal value = 0M;
                    if (decimal.TryParse(cell.Text, out value))
                    {
                        if (factor.PollutantMeasureUnit == "μg/m3"&&!LZSPfactor.Contains(factor.PollutantCode))
                        {
                            value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);

                        }
                        else
                        {
                            value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                        }
                        //value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                        cell.Text = value.ToString("");
                        if ((factorStatus != "N" && !string.IsNullOrEmpty(factorStatus)) || !string.IsNullOrEmpty(factorMark))
                        {
                            string markContent = string.Empty;
                            if (factorStatus != "N" && !string.IsNullOrEmpty(factorStatus))
                            {
                                markContent += factorStatus + "|";
                                siteTypeName = factorStatus + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorStatus))
                 .Select(t => t.ItemText).FirstOrDefault() + ")" + "|";//标记位名称
                            }
                            if (!string.IsNullOrEmpty(factorMark))
                            {
                                markContent += factorMark;
                                if (siteTypeName == "--")
                                {
                                    siteTypeName = factorMark + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorMark))
                          .Select(t => t.ItemText).FirstOrDefault() + ")";//标记位名称
                                }
                                else
                                {
                                    siteTypeName += factorMark + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorMark))
                          .Select(t => t.ItemText).FirstOrDefault() + ")";//标记位名称
                                }
                            }
                            markContent = markContent.TrimEnd('|');
                            cell.Text = cell.Text + "(" + markContent + ")";
                            cell.ForeColor = System.Drawing.Color.Red;
                            cell.Font.Bold = true;
                        }
                        if (dtExcessive.DefaultView.Count > 0)
                        {
                            if (item["PointId"] != null)
                            {
                                GridTableCell pointCell = (GridTableCell)item["PointId"];
                                IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                                if (drExcessive.Count() > 0)
                                {
                                    cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"] + "\n标记位：" + siteTypeName.TrimEnd('|');
                                }
                                else
                                {
                                    cell.ToolTip = "上限：-- \n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                                }
                            }
                        }
                        else
                        {
                            cell.ToolTip = "上限： --\n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(ExcessiveSettingInfo)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (ExcessiveSettingInfo item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(ExcessiveSettingInfo)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
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
            gridRT.CurrentPageIndex = 0;
            gridRT.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                RegisterScript("RefreshChart();");
            }
            else
            {
                FirstLoadChart.Value = "1";
            }

        }

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
                BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", out LZSfactorName, out LZSPfactor);
                string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                factors = factorCbxRsm.GetFactors();
                DateTime dtBegion = DateTime.Now.AddHours(-24);
                DateTime dtEnd = DateTime.Now;
                if (radlDataType.SelectedValue == "Min5")
                {
                    dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:mm"));
                    dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:mm"));
                }
                else if (radlDataType.SelectedValue == "Min60")
                {
                    dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:mm"));
                    dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:mm"));
                }
                DataView dv = g_IInfectantDALService.GetExportData(portIds, factors, dtBegion, dtEnd);

                DataTable dtNew = dv.ToTable();
                dtNew.Columns.Add("PointName", typeof(string));
                if (IsStatistical.Checked && factors.Count != 0)
                {
                    dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
                }

                DataTable dt = UpdateExportColumnName(dtNew, dvStatistical);
 
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PointId"] != DBNull.Value)
                    {
                        string pointid = dr["PointId"].ToString();
                        IPoint point = pointCbxRsm.GetPoints().FirstOrDefault(x => x.PointID.Equals(pointid));
                        dr["PointName"] = point.PointName;
                    }

                    foreach (IPollutant item in factors)
                    {
                        if (item.PollutantMeasureUnit == "μg/m3" && !LZSPfactor.Contains(item.PollutantCode))
                        {
                            if (!string.IsNullOrWhiteSpace(dr[item.PollutantName + "(" + item.PollutantMeasureUnit + ")"].ToString()))
                            {
                                dr[item.PollutantName + "(" + item.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(decimal.Parse(dr[item.PollutantName + "(" + item.PollutantMeasureUnit + ")"].ToString()) * 1000, string.IsNullOrEmpty(item.PollutantDecimalNum) ? 0 : Convert.ToInt32(item.PollutantDecimalNum) - 3);
                            }
                        }
                    }
                   
                }

                dt.Columns["PointName"].SetOrdinal(0);
                dt.Columns["PointName"].ColumnName = "测点名称";
                dt.Columns.Remove("PointId");
                dt.Columns.Remove("序号");
                //if (dt.C == "PointName")
                //{
                //    dcNew.SetOrdinal(0);
                //    dcNew.ColumnName = "测点名称";
                //}
                //DataTable dt = UpdateExportColumnName(dv);
                //dv = dt.DefaultView;
                ExcelHelper.DataTableToExcel(dt, "数据查询", "数据查询", this.Page);
            }
        }

        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <param name="dvStatistical">合计行数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataTable dt, DataView dvStatistical)
        {
            DataTable dtNew = dt;
            if (dvStatistical != null && dvStatistical.Table.Rows.Count > 0)
            {
                DataTable dtStatistical = dvStatistical.Table;
                DataRow drMaxRow = dtNew.NewRow();
                drMaxRow["PointName"] = "最大值";
                DataRow drMinRow = dtNew.NewRow();
                drMinRow["PointName"] = "最小值";
                DataRow drAvgRow = dtNew.NewRow();
                drAvgRow["PointName"] = "平均值";
                for (int i = 0; i < dtStatistical.Rows.Count; i++)
                {
                    DataRow drStatistical = dtStatistical.Rows[i];
                    if (drStatistical["PollutantCode"] != DBNull.Value && drStatistical["PollutantCode"].ToString() != "")
                    {
                        IPollutant factor = factorCbxRsm.GetFactors().FirstOrDefault(x => x.PollutantCode.Equals(drStatistical["PollutantCode"].ToString()));
                        int pdn = 0;
                        if (factor != null)
                        {
                            pdn = Convert.ToInt32(string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                        }
                        if (dtNew.Columns.Contains(factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"))
                        {
                            drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Max"] != DBNull.Value ? drStatistical["Value_Max"] : "--";
                            drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Min"] != DBNull.Value ? drStatistical["Value_Min"] : "--";
                            drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Avg"] != DBNull.Value ? drStatistical["Value_Avg"] : "--";

                            if (drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal AVG = Convert.ToDecimal(drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(AVG, pdn).ToString();
                            }
                            if (drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal MAX = Convert.ToDecimal(drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(MAX, pdn).ToString();
                            }
                            if (drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal MIN = Convert.ToDecimal(drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(MIN, pdn).ToString();
                            }
                        }
                    }
                }
                dtNew.Rows.Add(drAvgRow);
                dtNew.Rows.Add(drMaxRow);
                dtNew.Rows.Add(drMinRow);
            }
            return dtNew;
        }
        /// <summary>
        /// 图表类型选择（折线图、柱形图、点状图）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenChartType.Value = ChartType.SelectedValue;
            RegisterScript("ChartTypeChanged('" + ChartType.SelectedValue + "');");
        }
        #endregion

        /// <summary>
        /// grid 创建列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRT_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField == "PointId")
                {
                    col.HeaderText = "测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterText = "平均值<br>最大值<br>最小值";
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Tstamp")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Min60)
                        tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Min5)
                        tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Min1)
                        tstcolformat = "{0:yyyy-MM-dd HH:mm:ss}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    string unit = factor.PollutantMeasureUnit;
                    string strName = factor.PollutantName;
                    if (strName == "PM2.5")
                    {
                        strName = "PM<sub>2.5</sub>";
                    }
                    if (strName == "PM10")
                    {
                        strName = "PM<sub>10</sub>";
                    }
                    if (unit == "mg/m3")
                    {
                        unit = "mg/m<sup>3</sup>";
                    }
                    if (unit == "μg/m3")
                    {
                        unit = "μg/m<sup>3</sup>";
                    }
                    col.HeaderText = string.Format("{0}<br>({1})", strName, unit);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    SetGridFooterText(col);
                }
                else
                {
                    e.Column.Visible = false;
                }

            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 统计行事件
        /// </summary>
        /// <param name="col"></param>
        public void SetGridFooterText(GridBoundColumn col)
        {
            //统计行
            if (dvStatistical != null)
            {
                string avg = string.Empty;
                string max = string.Empty;
                string min = string.Empty;
                dvStatistical.RowFilter = string.Format("PollutantCode='{0}'", col.DataField);
                if (dvStatistical.Count > 0)
                {
                    avg = dvStatistical[0]["Value_Avg"] != DBNull.Value ? dvStatistical[0]["Value_Avg"].ToString() : "--";
                    max = dvStatistical[0]["Value_Max"] != DBNull.Value ? dvStatistical[0]["Value_Max"].ToString() : "--";
                    min = dvStatistical[0]["Value_Min"] != DBNull.Value ? dvStatistical[0]["Value_Min"].ToString() : "--";
                    if (factors != null)
                    {
                        IPollutant factor = factors[0];
                        if (avg != "--")
                        {
                            decimal AVG = Convert.ToDecimal(avg);
                            avg = DecimalExtension.GetPollutantValue(AVG, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                        }
                        if (max != "--")
                        {
                            decimal MAX = Convert.ToDecimal(max);
                            max = DecimalExtension.GetPollutantValue(MAX, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                        }
                        if (min != "--")
                        {
                            decimal MIN = Convert.ToDecimal(min);
                            min = DecimalExtension.GetPollutantValue(MIN, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                        }
                    }
                }
                col.FooterText = string.Format("{0}<br>{1}<br>{2}", avg, max, min);
                col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            }
        }
        /// <summary>
        /// 站点因子联动
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            points = pointCbxRsm.GetPoints();
            InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
            IList<string> list = new List<string>();
            string[] factor;
            string factors = string.Empty;
            foreach (IPoint point in points)
            {
                IQueryable<PollutantCodeEntity> p = m_InstrumentChannelService.RetrieveChannelListByPointUid(point.PointGuid);
                list = list.Union(p.Select(t => t.PollutantName)).ToList();
            }
            factor = list.ToArray();
            foreach (string f in factor)
            {
                factors += f + ";";
            }
            factorCbxRsm.SetFactorValuesFromNames(factors);
        }
        /// <summary>
        /// 查询类型切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (radlDataType.SelectedValue == "Min60")
            //{
            //    dvTotalb.Visible = false;
            //    dvTotala.Visible = true;
            //}
            //else
            //{
            //    dvTotalb.Visible = true;
            //    dvTotala.Visible = false;
            //}
        }
        /// <summary>
        /// 记录条数切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTotala_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            gridRT.Rebind();
        }

        protected void ddlTotalb_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            gridRT.Rebind();
        }


    }

}