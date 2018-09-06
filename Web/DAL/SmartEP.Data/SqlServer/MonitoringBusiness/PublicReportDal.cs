using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    public class PublicReportDal
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 虚拟分页类
        /// </summary>
        GridViewPagerDAL g_GridViewPager = Singleton<GridViewPagerDAL>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 获取系统运行有效率
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns> 
        public DataView GetRuningEffectRate(string ApplicationUid, List<int> PointIds, List<string> PollutantCodes, DateTime BeginTime, DateTime EndTime)
        {
            try
            {
                string strPointIds = "";
                for (int i = 0; i < PointIds.Count; i++)
                {
                    strPointIds += PointIds[i] + ",";
                }
                strPointIds = strPointIds.TrimEnd(',');
                string strPollutantCodes = "";
                for (int i = 0; i < PollutantCodes.Count; i++)
                {
                    strPollutantCodes += "'" + PollutantCodes[i] + "',";
                }
                strPollutantCodes = strPollutantCodes.TrimEnd(',');
                string strFields = " t1.PointId,t1.CollectionCount,t1.QualifiedCount,t1.DisqualificationCount,t1.QualifiedRate,t2.RunningDays ";
                string subWhere = string.Format(@" where ApplicationUid='{0}' and PointId in({1})
                                    and CONVERT(nvarchar(10),ReportDateTime,120)>='{3}' and CONVERT(nvarchar(10),ReportDateTime,120)<='{4}' "
                            , ApplicationUid, strPointIds, strPollutantCodes, BeginTime.ToString("yyyy-MM-dd"), EndTime.ToString("yyyy-MM-dd"));
                string strWhere = "";
                if (PollutantCodes.Count != 0)
                    strWhere = string.Format(@" {0} and PollutantCode in({1}) ", subWhere, strPollutantCodes);
                else
                    string.Format(" {0} ", subWhere);

                string subSql1 = string.Format(@"select  PointId,SUM(CollectionNumber) as CollectionCount
                                    ,SUM(QualifiedNumber) as QualifiedCount
                                    ,SUM(DisqualificationNumber) as DisqualificationCount
                                    ,CONVERT(decimal,SUM(QualifiedNumber))/CONVERT(decimal,SUM(CollectionNumber)) as QualifiedRate
                                    from dbo.TB_ReportQualifiedRateByDay
                                    {0}
                                    group by PointId", strWhere);
                string subSql2 = string.Format(@"select PointId,COUNT(*) as RunningDays from
                                    (select PointId,CONVERT(nvarchar(10),ReportDateTime,120) as RDT
                                    ,SUM(CollectionNumber) as Col,SUM(QualifiedNumber) as Qua
                                    from dbo.TB_ReportQualifiedRateByDay  
                                    {0}
                                    group by PointId,CONVERT(nvarchar(10),ReportDateTime,120) ) as t where Col > 0
                                    group by PointId", subWhere);
                string subSql = string.Format(@"select {0} from ({1}) as t1
                                    left join ({2}) as t2 on t1.PointId=t2.PointId
                                    ", strFields, subSql1, subSql2, ApplicationUid);
                string strSql = string.Format(@"{0} 
                                    union
                                    select 99999 as PointId ,SUM(CollectionCount) as CollectionCount
                                    ,SUM(QualifiedCount) as QualifiedCount
                                    ,SUM(DisqualificationCount) as DisqualificationCount
                                    ,CONVERT(decimal,SUM(QualifiedCount))/CONVERT(decimal,SUM(CollectionCount)) as QualifiedRate
                                    ,SUM(RunningDays) as RunningDays
                                    from ({0}) as t", subSql);
                DataView dv = g_DatabaseHelper.ExecuteDataView(strSql, connection);
                return dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取系统应测数据
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns> 
        public DataView GetShouldRuningEffectRate(string ApplicationUid, List<int> PointIds, List<string> PollutantCodes, DateTime BeginTime, DateTime EndTime)
        {
            try
            {
                string strPointIds = "";
                for (int i = 0; i < PointIds.Count; i++)
                {
                    strPointIds += PointIds[i] + ",";
                }
                strPointIds = strPointIds.TrimEnd(',');
                string strPollutantCodes = "";
                for (int i = 0; i < PollutantCodes.Count; i++)
                {
                    strPollutantCodes += "'" + PollutantCodes[i] + "',";
                }
                strPollutantCodes = strPollutantCodes.TrimEnd(',');
                string subWhere = string.Format(@" where ApplicationUid='{0}' and PointId in({1})
                                    and CONVERT(nvarchar(10),ReportDateTime,120)>='{3}' and CONVERT(nvarchar(10),ReportDateTime,120)<='{4}' "
                            , ApplicationUid, strPointIds, strPollutantCodes, BeginTime.ToString("yyyy-MM-dd"), EndTime.ToString("yyyy-MM-dd"));
                string strWhere = "";
                if (PollutantCodes.Count != 0)
                    strWhere = string.Format(@" {0} and PollutantCode in({1}) ", subWhere, strPollutantCodes);
                else
                    string.Format(" {0} ", subWhere);

                string strSql = string.Format(@"select PointId,SUM(SamplingNumber) as ShouldCount 
                                     from [AMS_MonitorBusiness].[dbo].[TB_ReportSamplingRateByDay]
                                     {0}
                                     group by PointId", strWhere);
                DataView dv = g_DatabaseHelper.ExecuteDataView(strSql, connection);
                return dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        /// <summary>
        /// 获取系统运行有效率（不确定因子）
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="dtPoints"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetRunningEffectRateByUncertainFactors(string ApplicationUid, DataTable dtPoints, DateTime BeginTime, DateTime EndTime)
        {
            try {
                if (dtPoints != null && dtPoints.Rows.Count > 0)
                {
                    StringBuilder strSql = new StringBuilder();
                    for (int i = 0; i < dtPoints.Rows.Count; i++)
                    {
                        int PointId = Convert.ToInt16(dtPoints.Rows[i]["PointId"]);
                        string[] PollutantCodes = dtPoints.Rows[i]["PollutantCodes"].ToString().Split(';');
                        string strPollutantCodes = "";
                        for (int f = 0; f < PollutantCodes.Length; f++)
                        {
                            strPollutantCodes += "'" + PollutantCodes[f] + "',";
                        }
                        strPollutantCodes = strPollutantCodes.TrimEnd(',');
                        string strFields = " t1.PointId,p.MonitoringPointName,t2.RunningDays,t1.CollectionCount,t1.QualifiedCount,t1.DisqualificationCount ";
                        string subWhere = string.Format(@" where ApplicationUid='{0}' and PointId = {1}
                                    and CONVERT(nvarchar(10),ReportDateTime,120)>='{3}' and CONVERT(nvarchar(10),ReportDateTime,120)<='{4}'"
                            , ApplicationUid, PointId, strPollutantCodes, BeginTime.ToString("yyyy-MM-dd"), EndTime.ToString("yyyy-MM-dd"));
                        string strWhere = string.Format(@" {0} and PollutantCode in({1}) "
                            , subWhere, strPollutantCodes);
                        string subSql1 = string.Format(@"select  PointId,SUM(CollectionNumber) as CollectionCount
                                    ,SUM(QualifiedNumber) as QualifiedCount
                                    ,SUM(DisqualificationNumber) as DisqualificationCount
                                    from dbo.TB_ReportQualifiedRateByDay
                                    {0}
                                    group by PointId", strWhere);
                        string subSql2 = string.Format(@"select PointId,COUNT(*) as RunningDays from
                                    (select PointId,CONVERT(nvarchar(10),ReportDateTime,120) as RDT
                                    ,SUM(CollectionNumber) as Col,SUM(QualifiedNumber) as Qua
                                    from dbo.TB_ReportQualifiedRateByDay  
                                    {0}
                                    group by PointId,CONVERT(nvarchar(10),ReportDateTime,120) ) as t where Col > 0
                                    group by PointId", subWhere);
                        string parentSql = string.Format(@"select {0} from ({1}) as t1
                                    left join ({2}) as t2 on t1.PointId=t2.PointId
                                    left join AMS_BaseData.MPInfo.TB_MonitoringPoint as p on ApplicationUid='{3}' and t1.PointId=p.PointId 
                                    ", strFields, subSql1, subSql2, ApplicationUid);
                        if (i == 0)
                            strSql.Append(parentSql);
                        else
                            strSql.AppendFormat(@"union
                                   {0}", parentSql);
                    }
                    return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);                    
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取系统应测记录数
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="dtPoints"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetShouldRecordByUncertainFactors(string ApplicationUid, DataTable dtPoints, DateTime BeginTime, DateTime EndTime)
        {
            try
            {
                if (dtPoints != null && dtPoints.Rows.Count > 0)
                {
                    StringBuilder strSql = new StringBuilder();
                    for (int i = 0; i < dtPoints.Rows.Count; i++)
                    {
                        int PointId = Convert.ToInt16(dtPoints.Rows[i]["PointId"]);
                        string[] PollutantCodes = dtPoints.Rows[i]["PollutantCodes"].ToString().Split(';');
                        string strPollutantCodes = "";
                        for (int f = 0; f < PollutantCodes.Length; f++)
                        {
                            strPollutantCodes += "'" + PollutantCodes[f] + "',";
                        }
                        strPollutantCodes = strPollutantCodes.TrimEnd(',');
                        string subWhere = string.Format(@" where ApplicationUid='{0}' and PointId = {1}
                                    and CONVERT(nvarchar(10),ReportDateTime,120)>='{3}' and CONVERT(nvarchar(10),ReportDateTime,120)<='{4}'"
                            , ApplicationUid, PointId, strPollutantCodes, BeginTime.ToString("yyyy-MM-dd"), EndTime.ToString("yyyy-MM-dd"));
                        string strWhere = string.Format(@" {0} and PollutantCode in({1}) "
                            , subWhere, strPollutantCodes);
                        string sql = string.Format(@"select PointId,SUM(SamplingNumber) as ShouldCount 
                                     from [AMS_MonitorBusiness].[dbo].[TB_ReportSamplingRateByDay]
                                     {0}
                                     group by PointId", strWhere);
                        if (i == 0)
                            strSql.Append(sql);
                        else
                            strSql.AppendFormat(@" union
                                   {0}", sql);
                    }
                    return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
//        public DataView GetRunningEffectRateByUncertainFactors(string ApplicationUid, DataTable dtPoints, DateTime BeginTime, DateTime EndTime)
//        {
//            try {
//                if (dtPoints != null && dtPoints.Rows.Count > 0)
//                {
//                    StringBuilder strSql = new StringBuilder();
//                    for (int i = 0; i < dtPoints.Rows.Count; i++)
//                    {
//                        int PointId = Convert.ToInt16(dtPoints.Rows[i]["PointId"]);
//                        string[] PollutantCodes = dtPoints.Rows[i]["PollutantCodes"].ToString().Split(';');
//                        string strPollutantCodes = "";
//                        for (int f = 0; f < PollutantCodes.Length; f++)
//                        {
//                            strPollutantCodes += "'" + PollutantCodes[f] + "',";
//                        }
//                        strPollutantCodes = strPollutantCodes.TrimEnd(',');
//                        string strFields = " t1.PointId,p.MonitoringPointName,t2.RunningDays,t1.CollectionCount,t1.QualifiedCount,t1.DisqualificationCount ";
//                        string subWhere = string.Format(@" where ApplicationUid='{0}' and PointId = {1}
//                                    and CONVERT(nvarchar(10),ReportDateTime,120)>='{3}' and CONVERT(nvarchar(10),ReportDateTime,120)<='{4}'"
//                            , ApplicationUid, PointId, strPollutantCodes, BeginTime.ToString("yyyy-MM-dd"), EndTime.ToString("yyyy-MM-dd"));
//                        string strWhere = string.Format(@" {0} and PollutantCode in({1}) "
//                            , subWhere, strPollutantCodes);
//                        string subSql1 = string.Format(@"select  PointId,SUM(CollectionNumber) as CollectionCount
//                                    ,SUM(QualifiedNumber) as QualifiedCount
//                                    ,SUM(DisqualificationNumber) as DisqualificationCount
//                                    from dbo.TB_ReportQualifiedRateByDay
//                                    {0}
//                                    group by PointId", strWhere);
//                        string subSql2 = string.Format(@"select PointId,COUNT(*) as RunningDays from
//                                    (select PointId,CONVERT(nvarchar(10),ReportDateTime,120) as RDT
//                                    ,SUM(CollectionNumber) as Col,SUM(QualifiedNumber) as Qua
//                                    from dbo.TB_ReportQualifiedRateByDay  
//                                    {0}
//                                    group by PointId,CONVERT(nvarchar(10),ReportDateTime,120) ) as t where Col > 0
//                                    group by PointId", subWhere);
//                        string parentSql = string.Format(@"select {0} from ({1}) as t1
//                                    left join ({2}) as t2 on t1.PointId=t2.PointId
//                                    left join AMS_BaseData.MPInfo.TB_MonitoringPoint as p on ApplicationUid='{3}' and t1.PointId=p.PointId
//                                    ", strFields, subSql1, subSql2, ApplicationUid);
//                        if (i == 0)
//                            strSql.Append(parentSql);
//                        else
//                            strSql.AppendFormat(@"union
//                                   {0}", parentSql);
//                    }
//                    return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
//                }
//                else
//                    return null;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

        /// <summary>
        /// 获取系统运行有效率
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="YearBegin"></param>
        /// <param name="MonthBegin"></param>
        /// <param name="YearEnd"></param>
        /// <param name="MonthEnd"></param>
        /// <returns></returns>
        public DataView GetRunningEffectRateByMonth(string ApplicationUid, List<int> PointIds, List<string> PollutantCodes, int YearBegin, int MonthBegin, int YearEnd, int MonthEnd)
        {
            try
            {
                string strPointIds = "";
                for (int i = 0; i < PointIds.Count; i++)
                {
                    strPointIds += PointIds[i] + ",";
                }
                strPointIds = strPointIds.TrimEnd(',');
                string strPollutantCodes = "";
                for (int i = 0; i < PollutantCodes.Count; i++)
                {
                    strPollutantCodes += "'" + PollutantCodes[i] + "',";
                }
                strPollutantCodes = strPollutantCodes.TrimEnd(',');
                string strWhere = string.Format(" where ApplicationUid='{0}' and PointId in ({1}) ", ApplicationUid, strPointIds);
                if (PointIds.Count != 0)
                    strWhere += string.Format(" and PollutantCode in({0}) ", strPollutantCodes);
                DateTime BeginTime = Convert.ToDateTime(YearBegin.ToString() + "-" + MonthBegin.ToString());
                DateTime EndTime = Convert.ToDateTime(YearEnd.ToString() + "-" + MonthEnd.ToString());
                StringBuilder parentSql = new StringBuilder();
                for (DateTime beginTime = BeginTime; beginTime <= EndTime; beginTime=beginTime.AddMonths(1))
                {
                    string subSql = string.Format(@"select  PointId,CONVERT(nvarchar(7),ReportDateTime,120) as ReportMonth,SUM(CollectionNumber) as CollectionCount
                                                ,SUM(QualifiedNumber) as QualifiedCount
                                                ,SUM(DisqualificationNumber) as DisqualificationCount
                                                ,case when SUM(CollectionNumber)=0then 0 else CONVERT(decimal,SUM(QualifiedNumber))/CONVERT(decimal,SUM(CollectionNumber)) end as QualifiedRate
                                                from dbo.TB_ReportQualifiedRateByDay
                                                {0} and CONVERT(nvarchar(7),ReportDateTime,120)='{1}' 
                                                group by PointId,CONVERT(nvarchar(7),ReportDateTime,120) ", strWhere, beginTime.ToString("yyyy-MM"));
                    if (beginTime == BeginTime)
                        parentSql.Append(subSql);
                    else
                        parentSql.Append(" union " + subSql);
                }
                string strSql = string.Format(@"select PointId ,AVG(QualifiedRate) as QualifiedRate
                                              from({0}) as t group by PointId ", parentSql.ToString());
                string Sql = string.Format(@"{0} union select 99999 as PointId,AVG(QualifiedRate)  from({0}) as ttt ", strSql);
                return g_DatabaseHelper.ExecuteDataView(Sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        /// <summary>
        /// 获取有效数据捕获率
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetDataSamplingRate(string ApplicationUid, List<int> PointIds, List<string> PollutantCodes, DateTime BeginTime, DateTime EndTime)
        {
            try
            {
                string strPointIds = "";
                for (int i = 0; i < PointIds.Count; i++)
                {
                    strPointIds += PointIds[i] + ",";
                }
                strPointIds = strPointIds.TrimEnd(',');
                string strPollutantCodes = "";
                for (int i = 0; i < PollutantCodes.Count; i++)
                {
                    strPollutantCodes += "'" + PollutantCodes[i] + "',";
                }
                strPollutantCodes = strPollutantCodes.TrimEnd(',');
                string subWhere = "";
                if (PollutantCodes.Count != 0)
                    subWhere = " AND PollutantCode IN(" + strPollutantCodes + ")";

                string subSql = string.Format(@"select PointId,COUNT(SamplingNumber) as SamplingCount
                                    ,COUNT(ActualCollectionNumber) as ActualCollectionCount
                                    ,COUNT(MissingCollectionNumber) as MissingCollectionCount
                                    from dbo.TB_ReportSamplingRateByDay
                                   where ApplicationUid='{0}' and PointId in({1}) 
                                    {2}
                                    and ReportDateTime>='{3}' and ReportDateTime<='{4}'
                                    group by PointId", ApplicationUid.Trim(), strPointIds, subWhere, BeginTime.ToString("yyyy-MM-dd"), EndTime.ToString("yyyy-MM-dd"));
                string strSql = string.Format(@"{0}
                                    union
                                    select 99999 as PointId ,SUM(SamplingCount) as SamplingCount
                                    ,SUM(ActualCollectionCount) as ActualCollectionCount
                                    ,SUM(MissingCollectionCount) as MissingCollectionCount
                                    from ({0}) as t", subSql);
                return g_DatabaseHelper.ExecuteDataView(strSql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取质控数据合格率
        /// </summary>
        /// <param name="MissionIds"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetQualityControlData(List<string> MissionIds, List<int> PointIds, List<string> PollutantCodes, DateTime BeginTime, DateTime EndTime)
        {
            try 
            {
                string strMissionIds = "";
                for (int i = 0; i < MissionIds.Count; i++)
                {
                    strMissionIds += "'" + MissionIds[i] + "',";
                }
                strMissionIds = strMissionIds.TrimEnd(',');
                string strPointIds = "";
                for (int i = 0; i < PointIds.Count; i++)
                {
                    strPointIds += PointIds[i] + ",";
                }
                strPointIds = strPointIds.TrimEnd(',');
                string strPollutantCodes = "";
                for (int i = 0; i < PollutantCodes.Count; i++)
                {
                    strPollutantCodes += "'" + PollutantCodes[i] + "',";
                }
                strPollutantCodes = strPollutantCodes.TrimEnd(',');
                string subWhere = "";
                if (PollutantCodes.Count != 0)
                    subWhere = " AND PollutantCode IN(" + strPollutantCodes + ")";

                string subSql = string.Format(@"select PointId,Evaluate,COUNT(*) as ECount from dbo.TB_StandardSolutionCheck
                                    where PointId IN({0})  AND MissionId IN({1})  
                                    {2}
                                    AND  Tstamp>='{3}' AND  Tstamp<='{4}'
                                    group by PointId,Evaluate", strPointIds, strMissionIds, subWhere, BeginTime.ToString("yyyy-MM-dd"), EndTime.ToString("yyyy-MM-dd"));
                string parentSql=string.Format(@"select PointId,MAX(CASE(Evaluate) when '合格' then ECount end) as Qualified
                                    ,MAX(CASE(Evaluate) when '不合格' then ECount end) as UnQualified
                                    ,MAX(CASE when isnull(Evaluate,'')='' then ECount end) as Invalid
                                    from({0} union 
                                    select 99999 as PointId,Evaluate,SUM(ECount) as ECount
                                    from 
                                    ({0}) as t1
                                    group by Evaluate) as t2 group by PointId", subSql);
                string strSql = string.Format(@"select PointId,(case when Qualified is not NULL then Qualified else 0 end) as Qualified
                                    ,(case when UnQualified is not NULL then UnQualified else 0 end) as UnQualified
                                    ,(case when Invalid is not NULL then Invalid else 0 end) as Invalid 
                                    from ({0}) as t3", parentSql);
                return g_DatabaseHelper.ExecuteDataView(strSql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// 获取异常情况处理率
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetExceptionHandingRate(List<int> PointIds, List<string> PollutantCodes, DateTime BeginTime, DateTime EndTime)
        {
            try
            {
                string strPointIds = "";
                for (int i = 0; i < PointIds.Count; i++)
                {
                    strPointIds += PointIds[i] + ",";
                }
                strPointIds = strPointIds.TrimEnd(',');
                string strPollutantCodes = "";
                for (int i = 0; i < PollutantCodes.Count; i++)
                {
                    strPollutantCodes += "'" + PollutantCodes[i] + "',";
                }
                strPollutantCodes = strPollutantCodes.TrimEnd(',');
                string strSql = "";
                //return g_DatabaseHelper.ExecuteDataView(strSql, connection);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
