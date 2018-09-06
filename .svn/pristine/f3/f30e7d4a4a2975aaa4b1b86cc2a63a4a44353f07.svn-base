using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Utilities.Web.WebServiceHelper;

namespace SmartEP.Service.DataAnalyze.HomePageElements
{
    /// <summary>
    /// 名称：QualityRunningService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：首页元件质控运维类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class QualityRunningService
    {
        /// <summary>
        /// 运维平台获取数据WebService路径
        /// </summary>
        private string m_OperationOMMPGetDataWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationOMMPGetDataWebServiceUrl"].ToString();

        /// <summary>
        /// 运维平台任务处理WebService路径
        /// </summary>
        private string m_OperationTaskWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationTaskWebServiceUrl"].ToString();

        /// <summary>
        /// 获取质控运维信息
        /// </summary>
        /// <param name="dateStart">开始时间（年，周）</param>
        /// <param name="dateEnd">结束时间（年，周）</param>
        /// <returns>
        /// DateStart：开始时间
        /// DateEnd：结束时间
        /// SitesTotal：站点总数
        /// ArriveRate：到站率
        /// QualifiedRate：合格率
        /// </returns>
        public DataView GetQualityRunningInfo(int[,] dateStart, int[,] dateEnd)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Year", typeof(string));
            dt.Columns.Add("WeekOfYear", typeof(string));
            dt.Columns.Add("DateStart", typeof(string));
            dt.Columns.Add("DateEnd", typeof(string));
            dt.Columns.Add("SitesTotal", typeof(string));
            dt.Columns.Add("ArriveRate", typeof(string));
            dt.Columns.Add("QualifiedRate", typeof(string));

            #region 假数据
            //DataRow dr = dt.NewRow();
            //dr["Year"] = "2015";
            //dr["WeekOfYear"] = "10";
            //dr["DateStart"] = "2015-08-31";
            //dr["DateEnd"] = "2015-09-06";
            //dr["SitesTotal"] = "128";
            //dr["ArriveRate"] = "60";
            //dr["QualifiedRate"] = "70";
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["Year"] = "2015";
            //dr["WeekOfYear"] = "11";
            //dr["DateStart"] = "2015-09-7";
            //dr["DateEnd"] = "2015-09-13";
            //dr["SitesTotal"] = "128";
            //dr["ArriveRate"] = "50";
            //dr["QualifiedRate"] = "75";
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["Year"] = "2015";
            //dr["WeekOfYear"] = "12";
            //dr["DateStart"] = "2015-09-14";
            //dr["DateEnd"] = "2015-09-20";
            //dr["SitesTotal"] = "128";
            //dr["ArriveRate"] = "30";
            //dr["QualifiedRate"] = "60";
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["Year"] = "2015";
            //dr["WeekOfYear"] = "13";
            //dr["DateStart"] = "2015-09-21";
            //dr["DateEnd"] = "2015-09-27";
            //dr["SitesTotal"] = "128";
            //dr["ArriveRate"] = "65";
            //dr["QualifiedRate"] = "75";
            //dt.Rows.Add(dr);
            #endregion

            #region 真实方法
            object objTastSolution = WebServiceHelper.InvokeWebService(m_OperationTaskWebServiceUrl,
                        "GetTastSolutionByWeek", new object[] { dateStart[0, 0].ToString(), dateStart[0, 1].ToString(),
                                                                dateEnd[0, 0].ToString(), dateEnd[0, 1].ToString(), "1" });
            DataTable dtTastSolution = objTastSolution as DataTable;
            object objSignIn = WebServiceHelper.InvokeWebService(m_OperationTaskWebServiceUrl,
                       "GetSignInSolution", new object[] { dateStart[0, 0].ToString(), dateStart[0, 1].ToString(),
                                                           dateEnd[0, 0].ToString(), dateEnd[0, 1].ToString() });
            DataTable dtSignIn = objSignIn as DataTable;
            DateTime dtime = DateTime.Today;
            int curWeek = GetWeekOfYear(dtime);
            int startYear = dateStart[0, 0];
            int startWeek = dateStart[0, 1];
            int endYear = dateEnd[0, 0];
            int endWeek = dateEnd[0, 1];
            IList<string> yearWeekList = new List<string>();
            if (startYear != endYear)
            {
                DateTime dtimeLastYear = new DateTime(startYear, 12, 31);
                int weekLaskYear = GetWeekOfYear(dtimeLastYear);
                for (int week = startWeek; week <= weekLaskYear; week++)
                {
                    string yearWeek = startYear.ToString() + "-" + week.ToString();
                    if (!yearWeekList.Contains(yearWeek))
                    {
                        yearWeekList.Add(yearWeek);
                    }
                }
                for (int week = 1; week <= endWeek; week++)
                {
                    string yearWeek = endYear.ToString() + "-" + week.ToString();
                    if (!yearWeekList.Contains(yearWeek))
                    {
                        yearWeekList.Add(yearWeek);
                    }
                }
            }
            else
            {
                for (int week = startWeek; week <= endWeek; week++)
                {
                    string yearWeek = startYear.ToString() + "-" + week.ToString();
                    if (!yearWeekList.Contains(yearWeek))
                    {
                        yearWeekList.Add(yearWeek);
                    }
                }
            }
            //for (int week = startWeek; week <= curWeek; week++)
            for (int i = 0; i < yearWeekList.Count; i++)
            {
                string yearWeek = yearWeekList[i];
                string[] yearWeekArray = yearWeek.Split('-');
                int year = int.TryParse(yearWeekArray[0], out year) ? year : dtime.Year;
                int week = int.TryParse(yearWeekArray[1], out week) ? week : curWeek;
                DataRow drNew = dt.NewRow();
                DataRow[] drsTastSolution = dtTastSolution.Select(string.Format("beginyear='{0}' and beginDate='{1}' ", year, week));
                DataRow[] drsSignIn = dtSignIn.Select(string.Format("Year='{0}' and Date='{1}' ", year, week));
                int allPassingCount = 0;//总合格数
                int allTotalCount = 0;//总任务次数
                int allActualTimes = 0;//总实际签到次数
                int allSureTime = 0;//总需要签到次数
                int sitesTotal = 0;
                drNew["Year"] = year;
                drNew["WeekOfYear"] = week;
                //drNew["DateStart"] = "2015-08-31";
                //drNew["DateEnd"] = "2015-09-06";
                drNew["QualifiedRate"] = 0;
                drNew["ArriveRate"] = 0;
                dt.Rows.Add(drNew);

                foreach (DataRow drTastSolution in drsTastSolution)
                {
                    int passingCount = int.TryParse(drTastSolution["PassingCount"].ToString(), out passingCount) ? passingCount : 0;
                    allPassingCount += passingCount;
                    int totalCount = int.TryParse(drTastSolution["TotalCount"].ToString(), out totalCount) ? totalCount : 0;
                    allTotalCount += totalCount;
                    sitesTotal++;
                }
                if (allTotalCount > 0)
                {
                    allPassingCount = (allPassingCount > allTotalCount) ? allTotalCount : allPassingCount;
                    decimal passingRate = Math.Round((decimal)allPassingCount * 100 / allTotalCount, 2);
                    drNew["QualifiedRate"] = passingRate;
                }
                foreach (DataRow drSignIn in drsSignIn)
                {
                    int actualTimes = int.TryParse(drSignIn["ActualTimes"].ToString(), out actualTimes) ? actualTimes : 0;
                    allActualTimes += actualTimes;
                    int sureTime = int.TryParse(drSignIn["SureTime"].ToString(), out sureTime) ? sureTime : 0;
                    allSureTime += sureTime;
                }
                if (allSureTime > 0)
                {
                    allActualTimes = (allActualTimes > allSureTime) ? allSureTime : allActualTimes;
                    decimal signRate = Math.Round((decimal)allActualTimes * 100 / allSureTime, 2);
                    drNew["ArriveRate"] = signRate;
                }
                drNew["SitesTotal"] = sitesTotal;
            }
            #endregion

            return dt.DefaultView;
        }

        private int GetWeekOfYear(DateTime dtime)
        {
            //一.找到第一周的最后一天（先获取1月1日是星期几，从而得知第一周周末是几）
            int firstWeekend = 7 - Convert.ToInt32(DateTime.Parse(dtime.Year + "-1-1").DayOfWeek);

            //二.获取当天是一年当中的第几天
            int currentDay = dtime.DayOfYear;
            //三.（当天 减去 第一周周末）/7 等于 距第一周有多少周 再加上第一周的1 就是当天是今年的第几周了
            //    刚好考虑了惟一的特殊情况就是，当天刚好在第一周内，那么距第一周就是0 再加上第一周的1 最后还是1
            return Convert.ToInt32(Math.Ceiling((currentDay - firstWeekend) / 7.0)) + 1;
        }
    }
}
