using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Dock
{
    public partial class _24HSinglePollutantDataAnalyzeNew : SmartEP.WebUI.Common.BasePage
    {
        //数据处理服务
        InfectantBy60Service m_InfectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
        AirPollutantService m_AirPollutantService = Singleton<AirPollutantService>.GetInstance();
        /// <summary>
        /// 站点服务
        /// </summary>
        MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
                BindData();
            }
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //Update By Lifei On 2015-10-22 Start
            //取得国控点
            IQueryable<MonitoringPointEntity> ports = m_MonitoringPointAirService.RetrieveAirMPListByCountryControlled();
            rcbPoint.DataValueField = "PointId";
            rcbPoint.DataTextField = "MonitoringPointName";
            rcbPoint.DataSource = ports;
            rcbPoint.DataBind();
            for (int i = 0; i < rcbPoint.Items.Count; i++)
            {
                rcbPoint.Items[i].Checked = true;
            }
            //Update By Lifei On 2015-10-22 end

            IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveListByCalAQI();
            DataTable dt = ConvertToDataTable(Pollutant);
            rcbFactors.DataSource = dt;
            rcbFactors.DataTextField = "PollutantName";
            rcbFactors.DataValueField = "PollutantCode";
            rcbFactors.DataBind();
            BindData();

            string strPointName = "";
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    strPointName += (item.Value.ToString() + ";");
                }
            }
            hdPointNames.Value = strPointName;
            hdFactorName.Value = rcbFactors.SelectedValue;
        }

        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(PollutantCodeEntity)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (PollutantCodeEntity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(PollutantCodeEntity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                if (Row["GBCode"].ToString() != "")
                {
                    dataTable.Rows.Add(Row);
                }
            }
            return dataTable;
        }
        //绑定数据
        private void BindData()
        {
            //IList<IPoint> points = pointCbxRsm.GetPoints();//测点
            //string[] portIds = points.Select(t => t.PointID).ToArray();



            DateTime dtmEnd = DateTime.Now;
            DateTime dtmBegion = DateTime.Now.AddHours(-24);
            string Factor = rcbFactors.SelectedValue;
            SmartEP.Core.Interfaces.IPollutant Ifactor = m_AirPollutantService.GetPollutantInfo(Factor);
            string DecimalNum = Ifactor.PollutantDecimalNum;
            string[] factors = { Factor };
            string portId = "";

            //Update By Lifei On 2015-10-22 Start
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    portId += (item.Value.ToString() + ";");
                }
            }
            //Update By Lifei On 2015-10-22 end

            string[] portIds = portId.Trim(';').Split(';');
            hdPointNames.Value = portId;
            hdFactorName.Value = Factor;
            //【给隐藏域赋值，用于显示Chart】
            SetHiddenData(portIds, factors, dtmBegion, dtmEnd);
        }
        /// <summary>
        /// 页面隐藏域控件赋值，将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd)
        {
            if (factors.Length > 0 && portIds != null)
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors)
                                 + "|" + dtBegin + "|" + dtEnd + "|" + "Min60" + "|Air";
            }
        }
        /// <summary>
        /// 获取点位名称
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        private string GetPointName(string pageType, int pointid)
        {
            if (pageType.Equals("Air"))
            {
                MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                return g_MonitoringPointAir.RetrieveEntityByPointId(pointid).MonitoringPointName;
            }
            else if (pageType.Equals("Water"))
            {
                MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                return g_MonitoringPointWater.RetrieveEntityByPointId(pointid).MonitoringPointName;
            }
            else
            {
                return "";
            }

        }
        protected void rcbPoint_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string strPointName = "";
            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                if (item.Checked)
                {
                    strPointName += (item.Value.ToString() + ";");
                }
            }
            hdPointNames.Value = strPointName;
        }

        protected void rcbFactors_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            hdFactorName.Value = rcbFactors.SelectedValue;
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindData();
            RegisterScript("RefreshChart();");
        }
    }
}