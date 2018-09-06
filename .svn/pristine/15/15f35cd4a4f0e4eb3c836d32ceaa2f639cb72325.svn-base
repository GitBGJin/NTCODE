 using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.DomainModel.BaseData;


namespace SmartEP.WebUI.Pages.EnvAir.RealTimeData
{
    public partial class AirFlowChart : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RunSvg();
            } 
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RunSvg();
        }

        private void RunSvg()
        {
            string[] portIds = pointCbxRsm.GetPointValues(Core.Enums.CbxRsmReturnType.ID);
            // portid in (9,33,34,35,36,38,39) 用新的
            string[] getSvgPointIDs = ConfigurationManager.AppSettings["svgAirPoint"].ToString().Split(',');

            MonitoringPointAirService airSvg = new MonitoringPointAirService();
            MonitoringPointEntity MoPointUid = new MonitoringPointEntity();
            AcquisitionInstrumentService mnNum = new AcquisitionInstrumentService();
            AcquisitionInstrumentEntity ae = new AcquisitionInstrumentEntity();
            string pointUid = "";
            string mn = "";

            //if (getSvgPointIDs.Contains(portIds[0]))
            //{
            //    frameFlowChart.Attributes.Add("src", "/WuXiSVG/AirFlow.html?portId=" + portIds[0] + "&ST=22");
            //}
            //else 
            //{
            //    frameFlowChart.Attributes.Add("src", "/suzhouSVG/AirFlow.html?portId=" + portIds[0] + "&ST=22");
            //}
            bool flag = false;
            for (int i = 0; i < getSvgPointIDs.Length; i++)
            {
                if (portIds[0] == getSvgPointIDs[i])
                {
                    flag = true;
                    break;
                }
            }

            if (flag == true)
            {
                //airSvg
                MoPointUid = airSvg.RetrieveEntityByID(Convert.ToInt32(portIds[0]));
                pointUid = MoPointUid.MonitoringPointUid;
                if(!string.IsNullOrWhiteSpace(pointUid))
                {
                    ae = mnNum.RetrieveEntityByMonitoringPointUid(pointUid);
                    mn = ae.MN;
                }

                frameFlowChart.Attributes.Add("src", "/WuXiSVG/AirFlow.html?mn=" + mn + "&ST=22");
                
            }
            else
            {
                frameFlowChart.Attributes.Add("src", "/suzhouSVG/AirFlow.html?portId=" + portIds[0] + "&ST=22");
            }

        }

        protected void pointCbxRsm_SelectedChanged()
        {
            RunSvg();
        }
    }
}