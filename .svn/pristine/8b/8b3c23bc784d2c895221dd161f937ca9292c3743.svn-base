using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：DataEffectRateAnalyzeInfo.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-09-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：数据有效率统计详情
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DataEffectRateAnalyzeInfo : System.Web.UI.Page
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        AirDataEffectRateService airDataEffectRate = new AirDataEffectRateService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitForm();
            }
        }
        /// <summary>
        /// 界面数据初始化
        /// </summary>
        private void InitForm()
        {
            string effectRateInfoId = PageHelper.GetQueryString("effectRateInfoId");
            string parameterType = PageHelper.GetQueryString("parameterType");
            string startWeek = PageHelper.GetQueryString("startWeek");
            string endWeek = PageHelper.GetQueryString("endWeek");

            if (!string.IsNullOrWhiteSpace(effectRateInfoId))
            {
                this.ViewState["effectRateInfoId"] = effectRateInfoId;
                this.ViewState["parameterType"] = parameterType;
                this.ViewState["startWeek"] = startWeek;
                this.ViewState["endWeek"] = endWeek;
            }
        }
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid(string effectRateInfoId, string parameterType, string startWeek, string endWeek)
        {
            DateTime dtmBegin = DateTime.Now;
            DateTime dtmEnd = DateTime.Now;
            if (parameterType == "0")//日
            {
                dtmBegin = DateTime.TryParse(PageHelper.GetQueryString("dtmBegin"), out dtmBegin)
                            ? dtmBegin : DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                dtmEnd = DateTime.TryParse(PageHelper.GetQueryString("dtmEnd"), out dtmEnd)
                             ? dtmEnd : DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                //统计日期
                gridEffectRateInfo.MasterTableView.Caption = string.Format("统计日期：{0:yyyy-MM-dd}~{1:yyyy-MM-dd}", dtmBegin, dtmEnd);

            }
            else if (parameterType == "1")//月
            {
                DateTime mtBegin = DateTime.TryParse(PageHelper.GetQueryString("dtmBegin"), out dtmBegin)
                            ? dtmBegin : DateTime.Parse(DateTime.Now.ToString("yyyy-MM"));
                //本月第一天时间 
                dtmBegin = mtBegin.AddDays(-(mtBegin.Day) + 1);

                DateTime mtEnd = DateTime.TryParse(PageHelper.GetQueryString("dtmEnd"), out dtmEnd)
                             ? dtmEnd : DateTime.Parse(DateTime.Now.ToString("yyyy-MM"));
                //将本月月数+1 
                DateTime dt2 = mtEnd.AddMonths(1);
                //本月最后一天时间 
                dtmEnd = dt2.AddDays(-(mtEnd.Day));

                //统计日期
                gridEffectRateInfo.MasterTableView.Caption = string.Format("统计日期：{0:yyyy-MM}~{1:yyyy-MM}", dtmBegin, dtmEnd);

            }
            else if (parameterType == "2")//年
            {
                string yearBegin = PageHelper.GetQueryString("dtmBegin");
                string yearEnd = PageHelper.GetQueryString("dtmEnd");
                dtmBegin = Convert.ToDateTime(yearBegin + "/01/01");
                dtmEnd = Convert.ToDateTime(yearEnd + "/12/31");

                if (yearBegin.Equals(yearEnd))
                {
                    gridEffectRateInfo.MasterTableView.Caption = string.Format("统计日期：{0}", yearBegin);

                }
                else
                {
                    //统计日期
                    gridEffectRateInfo.MasterTableView.Caption = string.Format("统计日期：{0}~{1}", yearBegin, yearEnd);
                }
            }
            else if (parameterType == "3")//周
            {
                int weekBeginFrom = Convert.ToInt32(startWeek);//开始第几周

                DateTime weekTimeBegin = DateTime.TryParse(PageHelper.GetQueryString("dtmBegin"), out dtmBegin)
                            ? dtmBegin : DateTime.Parse(DateTime.Now.ToString("yyyy-MM"));//开始时间
                int year = weekTimeBegin.Year;
                int month = weekTimeBegin.Month;
                dtmBegin = Convert.ToDateTime(GetWeek(year, month, 0, weekBeginFrom));//总共几周

                int weekEndTo = Convert.ToInt32(endWeek);//结束第几周
                DateTime weekTimeEnd = DateTime.TryParse(PageHelper.GetQueryString("dtmEnd"), out dtmEnd)
                             ? dtmEnd : DateTime.Parse(DateTime.Now.ToString("yyyy-MM"));//开始时间

                dtmEnd = Convert.ToDateTime(GetWeek(weekTimeEnd.Year, weekTimeEnd.Month, 1, weekEndTo));//总共几周


                //统计日期
                gridEffectRateInfo.MasterTableView.Caption = string.Format("统计日期：{0:yyyy-MM}第{1}周~{2:yyyy-MM}第{3}周", PageHelper.GetQueryString("dtmBegin"), startWeek, PageHelper.GetQueryString("dtmEnd"), endWeek);
            }
            else if (parameterType == "4")
            {
                string quarterBegin = startWeek;//第几季
                string quarterYearBegin = PageHelper.GetQueryString("dtmBegin");//年
                switch (quarterBegin)
                {
                    case "1": dtmBegin = Convert.ToDateTime(quarterYearBegin + "/01/01");//开始时间
                        break;
                    case "2": dtmBegin = Convert.ToDateTime(quarterYearBegin + "/04/01");//开始时间
                        break;
                    case "3": dtmBegin = Convert.ToDateTime(quarterYearBegin + "/07/01");//开始时间
                        break;
                    case "4": dtmBegin = Convert.ToDateTime(quarterYearBegin + "/10/01");//开始时间
                        break;
                }

                string quarterEnd = endWeek;//第几季
                string quarterYearEnd = PageHelper.GetQueryString("dtmEnd");//年
                switch (quarterEnd)
                {
                    case "1":
                        DateTime quarter1 = Convert.ToDateTime(quarterYearEnd + "/03");//将本月月数+1 
                        DateTime dt1 = quarter1.AddMonths(1);
                        //本月最后一天时间 
                        dtmEnd = dt1.AddDays(-(quarter1.Day));//结束时间
                        break;
                    case "2":
                        DateTime quarter2 = Convert.ToDateTime(quarterYearEnd + "/06");//将本月月数+1 
                        DateTime dt2 = quarter2.AddMonths(1);
                        //本月最后一天时间 
                        dtmEnd = dt2.AddDays(-(quarter2.Day));//结束时间;
                        break;
                    case "3":
                        DateTime quarter3 = Convert.ToDateTime(quarterYearEnd + "/09");//将本月月数+1 
                        DateTime dt3 = quarter3.AddMonths(1);
                        //本月最后一天时间 
                        dtmEnd = dt3.AddDays(-(quarter3.Day));//结束时间;
                        break;

                    case "4":
                        DateTime quarter4 = Convert.ToDateTime(quarterYearEnd + "/12");//将本月月数+1 
                        DateTime dt4 = quarter4.AddMonths(1);
                        //本月最后一天时间 
                        dtmEnd = dt4.AddDays(-(quarter4.Day));//结束时间;
                        break;

                }


                //统计日期
                gridEffectRateInfo.MasterTableView.Caption = string.Format("统计日期：{0}~{1}", PageHelper.GetQueryString("dtmBegin") + "年第" + startWeek + "季", PageHelper.GetQueryString("dtmEnd") + "年第" + endWeek + "季");

            }
            string[] portIds = { effectRateInfoId };
            int pageSize = gridEffectRateInfo.PageSize;//每页显示数据个数  
            int pageNo = gridEffectRateInfo.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数

            //绑定数据                               
            var samplingRateData = airDataEffectRate.GetEffectRateDetailData(portIds, dtmBegin, dtmEnd, pageSize, pageNo, out recordTotal);
            if (samplingRateData == null)
            {
                gridEffectRateInfo.DataSource = new DataTable();
            }
            else
            {
                gridEffectRateInfo.DataSource = samplingRateData;
            }

            //数据总行数
            gridEffectRateInfo.VirtualItemCount = recordTotal;


        }


        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridEffectRateInfo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid(ViewState["effectRateInfoId"].ToString(), ViewState["parameterType"].ToString(), ViewState["startWeek"].ToString(), ViewState["endWeek"].ToString());
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridEffectRateInfo_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = e.Item as GridDataItem;
            //    DataRowView drv = e.Item.DataItem as DataRowView;
            //for (int iRow = 0; iRow < factors.Count; iRow++)
            //{
            //    RsmFactor factor = factors[iRow];
            //    GridTableCell cell = (GridTableCell)item[factor.FactorCode];
            //    string factorFlag = drv[factor.FactorCode + "_Status"] != DBNull.Value ? drv[factor.FactorCode + "_Status"].ToString() : string.Empty;
            //    cell.Text = cell.Text + factorFlag;
            //    //cell.Text = factorinfo.GetAlermString(drv[factorinfo.factorColumnName], drv["dataflag"], cell);
            //}
            //}
        }

        #region 计算一个月有几周
        /// <summary>
        /// 计算一个月有几周
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <returns></returns>
        private int ShowWeekSinMonth(int y, int m)
        {
            int days = DateTime.DaysInMonth(y, m);
            int weeks = 0;


            DateTime FirstDayOfMonth = DateTime.Parse(y + "-" + m + "-1");

            DateTime FirstDayOfWeek = FirstDayOfMonth;



            while (FirstDayOfWeek.Month == m)
            {
                weeks += 1;

                DateTime LastDayOfWeek;
                if (FirstDayOfWeek.Day + 7 > days)
                {
                    LastDayOfWeek = FirstDayOfWeek.AddDays(days - FirstDayOfWeek.Day);
                }
                else
                {
                    LastDayOfWeek = FirstDayOfWeek.AddDays(7 - getWeekDay(FirstDayOfWeek) - 1);
                }

                FirstDayOfWeek = LastDayOfWeek.AddDays(1);
            }
            return weeks;
        }

        /// <summary>
        /// 获取改周的时间（0：开始周的时间，1结束周的时间）
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="type">类型</param>
        /// <param name="w">第几周</param>
        /// <returns></returns>
        private string GetWeek(int y, int m, int type, int w)
        {
            int days = DateTime.DaysInMonth(y, m);
            int weeks = 0;


            DateTime FirstDayOfMonth = DateTime.Parse(y + "-" + m + "-1");

            DateTime FirstDayOfWeek = FirstDayOfMonth;

            while (FirstDayOfWeek.Month == m)
            {
                weeks += 1;
                if (weeks.Equals(w) && type.Equals(0))
                {
                    return FirstDayOfWeek.ToString("yyyy/MM/dd");
                }

                DateTime LastDayOfWeek;
                if (FirstDayOfWeek.Day + 7 > days)
                {
                    LastDayOfWeek = FirstDayOfWeek.AddDays(days - FirstDayOfWeek.Day);
                }
                else
                {
                    LastDayOfWeek = FirstDayOfWeek.AddDays(7 - getWeekDay(FirstDayOfWeek) - 1);
                }
                if (weeks.Equals(w) && type.Equals(1))
                {
                    return LastDayOfWeek.ToString("yyyy/MM/dd");

                }
                FirstDayOfWeek = LastDayOfWeek.AddDays(1);
            }
            return "";

        }

        private int getWeekDay(DateTime d)
        {
            return (d.Day + 2 * d.Month + 3 * (d.Month + 1) / 5 + d.Year + d.Year / 4 - d.Year / 100 + d.Year / 400 + 1) % 7;
        }
        #endregion
        #endregion
    }
}