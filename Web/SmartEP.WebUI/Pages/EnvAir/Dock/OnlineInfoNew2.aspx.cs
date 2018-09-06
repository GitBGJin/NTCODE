using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Dock
{
    public partial class OnlineInfoNew2 : SmartEP.WebUI.Common.BasePage
    {
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
        /// 个性化配置服务
        /// </summary>
        PersonalizedSetService personalizedSet = new PersonalizedSetService();

        SmartEP.Service.DataAnalyze.Common.DataSamplingConditionService Service = new Service.DataAnalyze.Common.DataSamplingConditionService();

        /// <summary>
        /// 获取测点或因子的服务
        /// </summary>
        MonitoringPointAirService monitoringPointAir = new MonitoringPointAirService();

        AirRealTimeOnlineStateNewService AirRealTimeOnlineStateNewService = new AirRealTimeOnlineStateNewService();

        MonitoringPointService MonitoringPointService = new MonitoringPointService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }


        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {

            #region 根据用户的配置的userGuid 获取该测点

            string userGuid = this.Session["UserGuid"].ToString();
            IQueryable<PersonalizedSettingEntity> getPoint = personalizedSet.GetPersonalizedPoint(userGuid);
            string[] pointUid = getPoint.Select(it => it.ParameterUid).GroupBy(p => p).Select(p => p.Key).ToArray();//获取测点uid
            IQueryable<MonitoringPointEntity> monitoringPoint = monitoringPointAir.RetrieveListByPointUids(pointUid);
            string[] portIds = GetPointIdsByPointEntitys(monitoringPoint.ToArray()).GroupBy(p => p).Select(p => p.Key).ToArray();//根据测点数组获取测点Id列 用户配置的pointID 

            #endregion

            DataTable dt = Service.GetOfflinePointInfo("airaaira-aira-aira-aira-airaairaaira", portIds);
            DataTable sourceDT = new DataTable();
            sourceDT.Columns.Add("点位类型", typeof(string));
            sourceDT.Columns.Add("点位名称", typeof(string));
            sourceDT.Columns.Add("数据类型", typeof(string));
            sourceDT.Columns.Add("最新数据时间", typeof(string));
            sourceDT.Columns.Add("离线时间", typeof(string));
            sourceDT.Columns.Add("排序", typeof(Int32));
            string time = string.Empty;
            int year, month, day;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow dr = sourceDT.NewRow();
                    dr["点位类型"] = sdr["itemtext"].ToString();
                    dr["点位名称"] = sdr["monitoringpointname"].ToString();
                    dr["数据类型"] = "小时数据";
                    dr["最新数据时间"] = sdr["LastRecordTime"] == DBNull.Value ? "" : Convert.ToDateTime(sdr["LastRecordTime"]).ToString("yyyy-MM-dd hh:mm:ss");
                    int minute = Convert.ToInt32(sdr["OfflineTimes"]);

                    dr["离线时间"] = GetTimeStr(minute);
                    dr["排序"] = 1;

                    sourceDT.Rows.Add(dr);
                }
            }

            //五分钟数据
            DataTable dtDataOnlineState = AirRealTimeOnlineStateNewService.Get5MinOfflineInfo(portIds, "0", "Min5").ToTable();

            string pIds = string.Empty;
            foreach (string pId in portIds)
            {
                pIds += pId + ",";
            }
            pIds = pIds.Trim(',');
            DataView pointDV = MonitoringPointService.GetPointNameByID(pIds).DefaultView;

            foreach (DataRow sdr in dtDataOnlineState.Rows)
            {
                DataRow dr = sourceDT.NewRow();
                pointDV.RowFilter = "pointid = '" + sdr["pointid"].ToString() + "'";
                dr["点位类型"] = pointDV.Count > 0 ? pointDV[0]["itemtext"].ToString() : "";
                dr["点位名称"] = pointDV.Count > 0 ? pointDV[0]["monitoringpointname"].ToString() : "";
                dr["数据类型"] = "五分钟数据";
                dr["最新数据时间"] = sdr["Tstamp"] == DBNull.Value ? "" : Convert.ToDateTime(sdr["Tstamp"]).ToString("yyyy-MM-dd hh:mm:ss");
                dr["离线时间"] = sdr["NetWorkInfo"].ToString();
                dr["排序"] = 2;
                sourceDT.Rows.Add(dr);
            }
            DataView dv = sourceDT.DefaultView;
            dv.Sort = "排序,点位类型";
            RadGrid1.DataSource = sourceDT;
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

        protected void grid_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case GridItemType.AlternatingItem:
                    break;
                case GridItemType.ColGroup:
                    break;
                case GridItemType.ColItem:
                    break;
                case GridItemType.CommandItem:
                    #region CommandItem 初始化ToolBar
                    GridCommandItem CmdItem = (GridCommandItem)(e.Item);
                    if (CmdItem != null)
                    {
                        RadToolBar RTB = (RadToolBar)(CmdItem.FindControl("gridRTB"));
                        //InitRadToolBar(RTB);
                        RTB.Items[1].Visible = false;
                        RTB.Items[2].Visible = false;
                        RTB.Items[3].Visible = false;
                        RTB.Items[4].Visible = false;
                        RTB.Items[5].Visible = false;
                        RTB.Items[6].Visible = false;
                        RTB.Items[12].Visible = false;//导出EXCEL
                        RTB.Items[13].Visible = false;//导出WORD
                    }
                    break;
                    #endregion
                case GridItemType.EditFormItem:
                    break;
                case GridItemType.EditItem:
                    break;
                case GridItemType.FilteringItem:
                    break;
                case GridItemType.Footer:
                    break;
                case GridItemType.GroupFooter:
                    break;
                case GridItemType.GroupHeader:
                    break;
                case GridItemType.Header:
                    break;
                case GridItemType.Item:
                    break;
                case GridItemType.NestedView:
                    break;
                case GridItemType.NoRecordsItem:
                    break;
                case GridItemType.Pager:
                    break;
                case GridItemType.SelectedItem:
                    break;
                case GridItemType.Separator:
                    break;
                case GridItemType.StatusBar:
                    break;
                case GridItemType.TFoot:
                    break;
                case GridItemType.THead:
                    break;
                case GridItemType.Unknown:
                    break;
                default:
                    break;
            }

        }
        #endregion

        public string GetTimeStr(int minute)
        {
            string returnStr = string.Empty;
            if (minute == 0)
            { returnStr = ""; }
            else if (minute > 0)
            {
                int y, m, d, h, min;
                y = minute / 525600;
                m = (minute % 525600) / 43200;
                d = (minute % 43200) / 1440;
                h = (minute % 1440) / 60;
                min = (minute % 60);

                if (y != 0) returnStr += y.ToString() + "年";
                if (m != 0) returnStr += m.ToString() + "月";
                if (d != 0) returnStr += d.ToString() + "日";
                if (h != 0) returnStr += h.ToString() + "时";
                if (min != 0) returnStr += min.ToString() + "分";
            }



            return returnStr;
        }

    }
}