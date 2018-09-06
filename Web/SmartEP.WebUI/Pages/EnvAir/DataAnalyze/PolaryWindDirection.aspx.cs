using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SmartEP.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.PolaryWind;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Core.Generic;
using SmartEP.Service.BaseData.Channel;
using System.Collections.ObjectModel;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest;
using Telerik.Web.UI;
using SmartEP.Core.Interfaces;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class PolaryWindDirection : SmartEP.WebUI.Common.BasePage
    {
        string factorCodeWindDir = "a01008";
        string factorCodeWindSpeed = "a01007";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.QueryString["PolaryType"] != null)
                //{
                //    if (Request.QueryString["PolaryType"].Equals("0"))
                //    {
                //        ViewState["PolaryType"] = "WindDirection";
                //    }
                //    else if (Request.QueryString["PolaryType"].Equals("1"))
                //    {
                //        ViewState["PolaryType"] = "WindSpeed";
                //    }
                //    else
                //    {
                //        ViewState["PolaryType"] = "Factor";
                //        tdContent.Visible = true;
                //        tdTitle.Visible = true;
                //    }
                //}
                //else
                //{
                //    ViewState["PolaryType"] = "WindDirection";
                //}
                InitControl();
            }
        }

        #region 初始化
        private void InitControl()
        {
            hourBegin.SelectedDate = DateTime.Now.Date.AddDays(-1);
            hourEnd.SelectedDate = DateTime.Now.Date.AddHours(-1);
            dayBegin.SelectedDate = DateTime.Now.Date.AddDays(-1);
            dayEnd.SelectedDate = DateTime.Now.Date.AddHours(-1);


            radlDataType.Items.Add(new ListItem("原始小时数据", PollutantDataType.Min60.ToString()));
            radlDataType.Items.Add(new ListItem("审核小时数据", PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("原始日数据", PollutantDataType.OriDay.ToString()));
            radlDataType.Items.Add(new ListItem("审核日数据", PollutantDataType.Day.ToString()));
            radlDataType.Items[0].Selected = true;

            WindDirRadioButton.Items.Add(new ListItem("十六风向", "Sixteen"));
            WindDirRadioButton.Items.Add(new ListItem("八风向", "Eight"));
            WindDirRadioButton.Items[0].Selected = true;

            //pointCbxRsm_SelectedChanged();
            //if (radWindPoint.Items.Count > 0)
            //{
            //    radWindPoint.SelectedIndex = 0;
            //}

            dbtDay.Visible = false;
            pointForWind.SetPointValuesFromNames("超级站");
            pointCbxRsm.SetPointValuesFromNames("超级站");
        }
        #endregion

        #region 事件
        /// <summary>
        /// 站点控件选择项变化，风向点位动态绑定值
        /// </summary>
        //protected void pointCbxRsm_SelectedChanged()
        //{
        //    List<IPoint> pointList = pointCbxRsm.GetPoints();
        //    radWindPoint.Items.Clear();
        //    foreach (IPoint p in pointList)
        //    {
        //        radWindPoint.Items.Add(new RadComboBoxItem(p.PointName, p.PointID));
        //    }
        //}

        /// <summary>
        /// 数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //小时数据
            if (radlDataType.SelectedValue == "Hour" || radlDataType.SelectedValue == "Min60")
            {
                dbtHour.Visible = true;
                dbtDay.Visible = false;
            }
            //日数据
            else if (radlDataType.SelectedValue == "Day" || radlDataType.SelectedValue == "OriDay")
            {
                dbtDay.Visible = true;
                dbtHour.Visible = false;
            }
        }

        /// <summary>
        /// 点击查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            //if (ViewState["PolaryType"].ToString().Equals("Factor") && factorCom.CheckedItems.Count <= 0) { Alert("请选择因子！"); return; }
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            if (portIds == null)
            {
                Alert("请选择因子站点");
                return;
            }
            string factorCode = string.Join(";", factorCom.CheckedItems.Select(x => x.Value).ToArray());
            if (factorCom.CheckedItems.Count == 0)
            {
                Alert("请选择因子");
                return;
            }
            polaryGrid.Rebind();
        }

        /// <summary>
        /// 绑定Grid和图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void polaryGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                PolaryWindDirectionAjax windajax = new PolaryWindDirectionAjax();
                #region 获取参数
                string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                string factorCode = string.Join(";", factorCom.CheckedItems.Select(x => x.Value).ToArray());
                DateTime dtBegin = DateTime.Now;
                DateTime dtEnd = DateTime.Now;
                //小时数据
                if (radlDataType.SelectedValue == "Hour" || radlDataType.SelectedValue == "Min60")
                {
                    dtBegin = hourBegin.SelectedDate.Value;
                    dtEnd = hourEnd.SelectedDate.Value;
                }
                //日数据
                else if (radlDataType.SelectedValue == "Day" || radlDataType.SelectedValue == "OriDay")
                {
                    dtBegin = dayBegin.SelectedDate.Value;
                    dtEnd = dayEnd.SelectedDate.Value;
                }

                string pointId = pointForWind.GetPointValuesStr(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                #endregion

                #region 绑定数据
                BindChart(string.Join(";", portIds), factorCode, dtBegin, dtEnd, "", pointId);//绑定图表
                List<PolaryData> list = windajax.GetDataSource(portIds, factorCode.Split(';'), radlDataType.SelectedValue, dtBegin, dtEnd, WindDirRadioButton.SelectedValue, "", pointId);//绑定Grid
                if (list != null)
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        PolaryData p = list[i];
                        if (p.FactorCount == 0)
                        {
                            list.Remove(p);
                        }
                    }
                    polaryGrid.DataSource = list;
                }
                else
                {
                    polaryGrid.DataSource = new List<PolaryData>();
                }
                //changeData();
                #endregion
            }
            catch
            {
            }
        }

        /// <summary>
        /// 初始化列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void polaryGrid_Load(object sender, EventArgs e)
        {
            AddColumnsOfGridAuditData();
        }

        #endregion

        #region 方法
        /// <summary>
        /// 绑定Chart
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void BindChart(string portIds, string factorCode, DateTime dtBegin, DateTime dtEnd, string PolaryType, string pointId)
        {
            RegisterScript(string.Format(@"AjaxLoadingPolary('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}','{7}');"
                            , portIds, factorCode, dtBegin, dtEnd, radlDataType.SelectedValue, WindDirRadioButton.SelectedValue, PolaryType, pointId));
        }

        /// <summary>
        /// 绑定Grid列
        /// </summary>
        private void AddColumnsOfGridAuditData()
        {
            polaryGrid.Columns.Clear();
            string HeaderFX = "风向";
            string HeaderFS = "风速(m/s)";
            string HeaderND = "浓度";
            GridBoundColumn column = new GridBoundColumn() { DataField = "FactorName", UniqueName = "FactorName", HeaderText = "因子", EmptyDataText = "--" };
            polaryGrid.Columns.Add(column);
            GridBoundColumn columnC = new GridBoundColumn() { DataField = "FactorCount", UniqueName = "FactorCount", HeaderText = "次数", EmptyDataText = "--" };
            polaryGrid.Columns.Add(columnC);
            GridBoundColumn temColumn = new GridBoundColumn() { DataField = "FX", UniqueName = "FX", HeaderText = HeaderFX, EmptyDataText = "--" };
            polaryGrid.Columns.Add(temColumn);
            GridBoundColumn temsColumn = new GridBoundColumn() { DataField = "FS", UniqueName = "FS", HeaderText = HeaderFS, EmptyDataText = "--" };
            polaryGrid.Columns.Add(temsColumn);
            GridBoundColumn temnColumn = new GridBoundColumn() { DataField = "ND", UniqueName = "ND", HeaderText = HeaderND, EmptyDataText = "--" };
            polaryGrid.Columns.Add(temnColumn);
        }
        #endregion

        protected void polaryGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                if (((GridTableCell)item["FactorName"]).Text.Equals("CO"))
                {
                    GridTableCell NDCell = (GridTableCell)item["ND"];
                    NDCell.Text = NDCell.Text + " mg/m3";
                }
                else
                {
                    GridTableCell NDCell = (GridTableCell)item["ND"];
                    NDCell.Text = NDCell.Text + " μg/m3";
                }
            }
        }
        //private void changeData()
        //{
        //    foreach (GridDataItem item in polaryGrid.Items)
        //    {
        //        if (item["FactorName"].ToString().Equals("一氧化碳"))
        //        {
        //            GridTableCell NDCell = (GridTableCell)item["ND"];
        //            NDCell.Text = NDCell.Text + " mg/m3";
        //        }
        //        else
        //        {
        //            GridTableCell NDCell = (GridTableCell)item["ND"];
        //            NDCell.Text = NDCell.Text + " μg/m3";
        //        }
        //    }
        //}

    }
}