using SmartEP.DomainModel;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.WebControl.CbxRsm;
using SmartEP.WebUI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Core.Generic;
using SmartEP.Service.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Service.Frame;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class HeavyPollutantAnalyze : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DayAQIService m_DayAQIService = Singleton<DayAQIService>.GetInstance();
        DictionaryService dicService = new DictionaryService();

        protected void Page_Load(object sender, EventArgs e)
        {
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
            dtpFirstBegin.SelectedDate = DateTime.Now.AddMonths(-1);
            dtpFirstEnd.SelectedDate = DateTime.Now;
            //dtpSecondBegin.SelectedDate = DateTime.Now.AddMonths(-1);
            //dtpSecondEnd.SelectedDate = DateTime.Now;
            dtpThirdBegin.SelectedDate = DateTime.Now.AddYears(-1).AddMonths(-1);
            dtpThirdEnd.SelectedDate = DateTime.Now.AddYears(-1);
            ddlCityProper.DataSource = dicService.RetrieveList(DictionaryType.AMS, "行政区划");
            ddlCityProper.DataTextField = "ItemText";
            ddlCityProper.DataValueField = "ItemGuid";
            ddlCityProper.DataBind();
            //rcbFactors.Items.Add(new RadComboBoxItem("二氧化硫", IAQIType.SO2_IAQI.ToString()));
            //rcbFactors.Items.Add(new RadComboBoxItem("二氧化氮", IAQIType.NO2_IAQI.ToString()));
            //rcbFactors.Items.Add(new RadComboBoxItem("细颗粒物", IAQIType.PM25_IAQI.ToString()));
            //rcbFactors.Items.Add(new RadComboBoxItem("可吸入颗粒物", IAQIType.PM10_IAQI.ToString()));
            //rcbFactors.Items.Add(new RadComboBoxItem("一氧化碳", IAQIType.CO_IAQI.ToString()));
            //rcbFactors.Items.Add(new RadComboBoxItem("臭氧1小时", IAQIType.MaxOneHourO3_IAQI.ToString()));
            //rcbFactors.Items.Add(new RadComboBoxItem("臭氧8小时", IAQIType.Max8HourO3_IAQI.ToString()));
            //rcbFactors.Items[0].Checked = true;
            rcbPollution.Items.Add(new DropDownListItem("轻度污染", AQIClass.LightlyPolluted.ToString()));
            rcbPollution.Items.Add(new DropDownListItem("中度污染", AQIClass.ModeratelyPolluted.ToString()));
            rcbPollution.Items.Add(new DropDownListItem("重度污染", AQIClass.HeavilyPolluted.ToString()));
            rcbPollution.Items.Add(new DropDownListItem("严重污染", AQIClass.SeverelyPolluted.ToString()));
            //rcbPollution.Items[0].Checked = true;
            //gridHeavyPollutantAnalyze.MasterTableView.Columns[0].HeaderText = dtpFirstBegin.SelectedDate.Value.Year.ToString() + "(基准)";
            //gridHeavyPollutantAnalyze.MasterTableView.Columns[4].HeaderText = dtpThirdBegin.SelectedDate.Value.Year.ToString() + "(对比)";
            //gridHeavyPollutantAnalyze.MasterTableView.Columns[8].HeaderText = dtpFirstBegin.SelectedDate.Value.Year.ToString();
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            gridHeavyPollutantAnalyze.MasterTableView.Columns[0].HeaderText = dtpFirstBegin.SelectedDate.Value.Year.ToString() + "(基准)";
            gridHeavyPollutantAnalyze.MasterTableView.Columns[4].HeaderText = dtpThirdBegin.SelectedDate.Value.Year.ToString() + "(对比)";
            DateTime[,] dt = new DateTime[2, 2];

            if (dvFirst.Visible == true)
            {
                DateTime dtBegion1 = dtpFirstBegin.SelectedDate.Value;
                DateTime dtBegionA = new DateTime(dtBegion1.Year, dtBegion1.Month, 1);
                DateTime dtEnd1 = dtpFirstEnd.SelectedDate.Value;
                DateTime dtEndA = new DateTime(dtEnd1.Year, dtEnd1.Month, 1);
                DateTime dtEndB = Convert.ToDateTime(dtEndA.AddMonths(1).AddDays(-1).FormatToString("yyyy-MM-dd 23:59:59"));
                dt[0, 0] = dtBegionA;
                dt[0, 1] = dtEndB;
            }
            //if (dvSecond.Visible == true)
            //{
            //    DateTime dtBegion2 = dtpSecondBegin.SelectedDate.Value;
            //    DateTime dtBegion2A = new DateTime(dtBegion2.Year, dtBegion2.Month, 1);
            //    DateTime dtEnd2 = dtpSecondEnd.SelectedDate.Value;
            //    DateTime dtEnd2A = new DateTime(dtEnd2.Year, dtEnd2.Month, 1);
            //    DateTime dtEnd2B = Convert.ToDateTime(dtEnd2A.AddMonths(1).AddDays(-1).FormatToString("yyyy-MM-dd 23:59:59"));
            //    dt[1, 0] = dtBegion2A;
            //    dt[1, 1] = dtEnd2B;
            //}
            if (dvThird.Visible == true)
            {
                DateTime dtBegion3 = dtpThirdBegin.SelectedDate.Value;
                DateTime dtBegion3A = new DateTime(dtBegion3.Year, dtBegion3.Month, 1);
                DateTime dtEnd3 = dtpThirdEnd.SelectedDate.Value;
                DateTime dtEnd3A = new DateTime(dtEnd3.Year, dtEnd3.Month, 1);
                DateTime dtEnd3B = Convert.ToDateTime(dtEnd3A.AddMonths(1).AddDays(-1).FormatToString("yyyy-MM-dd 23:59:59"));
                dt[1, 0] = dtBegion3A;
                dt[1, 1] = dtEnd3B;
            }
            string[] portIds = null;
            string regionGuid = "";
            int portId = 0;

            int pollutionClass = 0;
            switch (rcbPollution.SelectedValue)
            {
                case "LightlyPolluted":
                    pollutionClass = (int)AQIClass.LightlyPolluted;
                    break;
                case "ModeratelyPolluted":
                    pollutionClass = (int)AQIClass.ModeratelyPolluted;
                    break;
                case "HeavilyPolluted":
                    pollutionClass = (int)AQIClass.HeavilyPolluted;
                    break;
                case "SeverelyPolluted":
                    pollutionClass = (int)AQIClass.SeverelyPolluted;
                    break;
            }
            //每页显示数据个数            
            int pageSize = gridHeavyPollutantAnalyze.PageSize;
            //当前页的序号
            int currentPageIndex = gridHeavyPollutantAnalyze.CurrentPageIndex + 1;
            //查询记录的开始序号
            int startRecordIndex = pageSize * currentPageIndex;

            int recordTotal = 0;
            var pollutantDate = new DataView();
            switch (rbtnlType.SelectedValue)
            {
                case "Port":
                    portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
                    if (portIds != null)
                    {
                        string str1 = string.Join(",", portIds);
                        portId = Convert.ToInt32(str1);
                        //获取不同站点污染情况分析数据
                        pollutantDate = m_DayAQIService.GetPortsPollution(portId, dt, pollutionClass);
                    }
                    else
                    {
                        pollutantDate = null;
                    }
                    break;
                case "CityProper":
                    regionGuid = ddlCityProper.SelectedValue;
                    //获取不同区域污染情况分析数据
                    pollutantDate = m_DayAQIService.GetRegionsPollution(regionGuid, dt, pollutionClass);
                    break;
            }


            if (pollutantDate != null)
            {
                DataTable dtpollutantDate = pollutantDate.ToTable();
                dtpollutantDate.Columns[dtpFirstBegin.SelectedDate.Value.Year.ToString() + "(0)"].ColumnName = "timeA";
                dtpollutantDate.Columns[dtpThirdBegin.SelectedDate.Value.Year.ToString() + "(1)"].ColumnName = "timeB";
                //dtpollutantDate.Columns[dtpThirdBegin.SelectedDate.Value.Year.ToString() + "(2)"].ColumnName = "timeC";
                pollutantDate = dtpollutantDate.DefaultView;
                gridHeavyPollutantAnalyze.DataSource = pollutantDate;//dataView;
                //数据分页的页数
                gridHeavyPollutantAnalyze.VirtualItemCount = pollutantDate.Count;
            }
            else
            {
                gridHeavyPollutantAnalyze.DataSource = null;
            }

        }

        #endregion

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridHeavyPollutantAnalyze.Rebind();
        }

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
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridHeavyPollutantAnalyze.Rebind();
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
                gridHeavyPollutantAnalyze.MasterTableView.Columns[0].HeaderText = dtpFirstBegin.SelectedDate.Value.Year.ToString();
                DateTime[,] dt = new DateTime[3, 2];

                if (dvFirst.Visible == true)
                {
                    DateTime dtBegion1 = dtpFirstBegin.SelectedDate.Value;
                    DateTime dtBegionA = new DateTime(dtBegion1.Year, dtBegion1.Month, 1);
                    DateTime dtEnd1 = dtpFirstEnd.SelectedDate.Value;
                    DateTime dtEndA = new DateTime(dtEnd1.Year, dtEnd1.Month, 1);
                    DateTime dtEndB = Convert.ToDateTime(dtEndA.AddMonths(1).AddDays(-1).FormatToString("yyyy-MM-dd 23:59:59"));
                    dt[0, 0] = dtBegionA;
                    dt[0, 1] = dtEndB;
                }
                //if (dvSecond.Visible == true)
                //{
                //    DateTime dtBegion2 = dtpSecondBegin.SelectedDate.Value;
                //    DateTime dtBegion2A = new DateTime(dtBegion2.Year, dtBegion2.Month, 1);
                //    DateTime dtEnd2 = dtpSecondEnd.SelectedDate.Value;
                //    DateTime dtEnd2A = new DateTime(dtEnd2.Year, dtEnd2.Month, 1);
                //    DateTime dtEnd2B = Convert.ToDateTime(dtEnd2A.AddMonths(1).AddDays(-1).FormatToString("yyyy-MM-dd 23:59:59"));
                //    dt[1, 0] = dtBegion2A;
                //    dt[1, 1] = dtEnd2B;
                //}
                if (dvThird.Visible == true)
                {
                    DateTime dtBegion3 = dtpThirdBegin.SelectedDate.Value;
                    DateTime dtBegion3A = new DateTime(dtBegion3.Year, dtBegion3.Month, 1);
                    DateTime dtEnd3 = dtpThirdEnd.SelectedDate.Value;
                    DateTime dtEnd3A = new DateTime(dtEnd3.Year, dtEnd3.Month, 1);
                    DateTime dtEnd3B = Convert.ToDateTime(dtEnd3A.AddMonths(1).AddDays(-1).FormatToString("yyyy-MM-dd 23:59:59"));
                    dt[2, 0] = dtBegion3A;
                    dt[2, 1] = dtEnd3B;
                }
                string[] portIds = null;
                string regionGuid = "";
                int portId = 0;

                int pollutionClass = 0;
                switch (rcbPollution.SelectedValue)
                {
                    case "LightlyPolluted":
                        pollutionClass = (int)AQIClass.LightlyPolluted;
                        break;
                    case "ModeratelyPolluted":
                        pollutionClass = (int)AQIClass.ModeratelyPolluted;
                        break;
                    case "HeavilyPolluted":
                        pollutionClass = (int)AQIClass.HeavilyPolluted;
                        break;
                    case "SeverelyPolluted":
                        pollutionClass = (int)AQIClass.SeverelyPolluted;
                        break;
                }
                var pollutantDate = new DataView();
                switch (rbtnlType.SelectedValue)
                {
                    case "Port":
                        portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
                        string str1 = string.Join(",", portIds);
                        portId = Convert.ToInt32(str1);
                        //获取不同站点污染情况分析数据
                        pollutantDate = m_DayAQIService.GetPortsPollution(portId, dt, pollutionClass);
                        break;
                    case "CityProper":
                        regionGuid = ddlCityProper.SelectedValue;
                        //获取不同区域污染情况分析数据
                        pollutantDate = m_DayAQIService.GetRegionsPollution(regionGuid, dt, pollutionClass);
                        break;
                }
                DataTable dtNew = pollutantDate.ToTable();
                for (int i = 0; i < dtNew.Columns.Count; i++)
                {
                    DataColumn dcNew = dtNew.Columns[i];
                    if (dcNew.ColumnName == dtpFirstBegin.SelectedDate.Value.Year.ToString() + "(0)")
                    {
                        dcNew.ColumnName = dtpFirstBegin.SelectedDate.Value.Year.ToString() + "(基准)";
                    }
                    else if (dcNew.ColumnName == "AQIValue(0)")
                    {
                        dcNew.ColumnName = "AQI(基准)";
                    }
                    else if (dcNew.ColumnName == ("PrimaryPollutant(0)"))
                    {
                        dcNew.ColumnName = "污染物(基准)";
                    }
                    else if (dcNew.ColumnName == ("Grade(0)"))
                    {
                        dcNew.ColumnName = "污染等级(基准)";
                    }
                    else if (dcNew.ColumnName == dtpThirdBegin.SelectedDate.Value.Year.ToString() + "(1)")
                    {
                        dcNew.ColumnName = dtpThirdBegin.SelectedDate.Value.Year.ToString() + "(对比)";
                    }
                    else if (dcNew.ColumnName == "AQIValue(1)")
                    {
                        dcNew.ColumnName = "AQI(对比)";
                    }
                    else if (dcNew.ColumnName == ("PrimaryPollutant(1)"))
                    {
                        dcNew.ColumnName = "污染物(对比)";
                    }
                    else if (dcNew.ColumnName == ("Grade(1)"))
                    {
                        dcNew.ColumnName = "污染等级(对比)";
                    }
                }
                ExcelHelper.DataTableToExcel(dtNew, "污染物分析", "污染物分析", this.Page);
            }
        }
        /// <summary>
        /// 查询类型切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //暂时写成这样，等提供数据源后再修改
            pointCbxRsm.Visible = false;
            ddlCityProper.Visible = false;
            switch (rbtnlType.SelectedValue)
            {
                case "Port":
                    pointCbxRsm.Visible = true;
                    break;
                case "CityProper":
                    ddlCityProper.Visible = true;
                    break;
                //case "City":
                //    comboCity.Visible = true;
                //    if (comboCity.Items.Count > 0)
                //    {
                //        comboCity.Items[0].Checked = true;
                //    }
                //    break;
                //case "CityModel":
                //    comboCityModel.Visible = true;
                //    if (comboCityModel.Items.Count > 0)
                //    {
                //        comboCityModel.Items[0].Checked = true;
                //    }
                //    break;
            }
        }

        #endregion

        protected void gridHeavy_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridHeavy_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected void gridPollutant_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridPollutant_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected void gridAnalyze_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridAnalyze_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

    }
}