using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Utilities.AdoData;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Water
{
    public class AutoPointMonthReportDAL
    {
        private string AMS_MonitorBusiness_Conn = EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);

        private string APP_UID = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Water);

        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper dbHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 根据时间获取平均有效运行率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns></returns>
        public double GetAvgValidRunRate(string dateTime)
        {
            return 59.89;
        }

        /// <summary>
        /// 根据时间获取平均标样考核合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns></returns>
        public double GetAvgSampleEevaluationRate(string dateTime)
        {
            return 59.89;
        }

        /// <summary>
        /// 根据时间获取平均水样比对合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns></returns>
        public double GetAvgCompareRate(string dateTime)
        {
            return 59.89;
        }

        /// <summary>
        /// 根据时间获取标样考核合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId：站点ID、Rate：有效率</returns>
        public DataTable GetPointSampleEevaluationRate(string dateTime)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PointId", typeof(string));
            dt.Columns.Add("Tstamp", typeof(DateTime));
            dt.Columns.Add("Rate", typeof(string));

            return dt;
        }

        /// <summary>
        /// 根据时间获取水样比对合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId：站点ID、Rate：有效率</returns>
        public DataTable GetPointCompareRate(string dateTime)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PointId", typeof(string));
            dt.Columns.Add("tstamp", typeof(DateTime));
            dt.Columns.Add("Rate", typeof(string));

            return dt;
        }

        /// <summary>
        /// 根据时间获取区域水站数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>AreaName: 区域名称、Rate：有效率</returns>
        public DataTable GetAreaDataValidRate(string dateTime)
        {
            try
            {
                string querySql = string.Format(@"
                       select svc.itemtext as AreaName,
                              temp.Rate  
                         from dbo.SY_View_CodeMainItem svc
                        inner join (
							      select replace(symp.RegionUid, '4d00bb50-177d-435b-b05d-58a5ef473920', '884007dc-d877-4e4e-8ec5-67a0d84c902c') as RegionUid,
									     dbo.F_Round(SUM(QualifiedNumber)/(SUM(CollectionNumber) + 0.0)*100, 1) as [Rate]
								    from dbo.TB_ReportQualifiedRateByDay trq
							       inner join dbo.SY_MonitoringPoint symp
								      on trq.PointId = symp.PointId
								     and trq.ApplicationUid = '{0}'
								     and convert(char(7), [ReportDateTime], 20) = '{1}' 
							       group by replace(symp.RegionUid, '4d00bb50-177d-435b-b05d-58a5ef473920', '884007dc-d877-4e4e-8ec5-67a0d84c902c')
	                               ) as temp
	                       on svc.itemguid = temp.RegionUid
	                    order by svc.SortNumber desc", APP_UID, dateTime);

                // 将YSI替换为市区显示
                // replace(symp.RegionUid, '4d00bb50-177d-435b-b05d-58a5ef473920', '884007dc-d877-4e4e-8ec5-67a0d84c902c')

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }

        /// <summary>
        /// 根据站点ID获取区域列表信息
        /// </summary>
        /// <param name="pointIds">站点ID(多个站点以英文,分割)</param>
        /// <returns>AreaName: 区域名称</returns>
        public DataTable GetAreasByPointIds(string pointIds)
        {
            try
            {
                string querySql = string.Format(@"
                       select svc.itemtext as AreaName
                         from dbo.SY_View_CodeMainItem svc
                        inner join (
							      select replace(symp.RegionUid, '4d00bb50-177d-435b-b05d-58a5ef473920', '884007dc-d877-4e4e-8ec5-67a0d84c902c') as RegionUid									     
								    from dbo.SY_MonitoringPoint symp
								   where symp.ApplicationUid = '{0}'
								     and pointId in ({1})
							       group by replace(symp.RegionUid, '4d00bb50-177d-435b-b05d-58a5ef473920', '884007dc-d877-4e4e-8ec5-67a0d84c902c')
	                               ) as temp
	                       on svc.itemguid = temp.RegionUid
	                    order by svc.SortNumber desc", APP_UID, pointIds);

                // 将YSI替换为市区显示
                // replace(symp.RegionUid, '4d00bb50-177d-435b-b05d-58a5ef473920', '884007dc-d877-4e4e-8ec5-67a0d84c902c')

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }

        /// <summary>
        /// 根据时间获取水站数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId: 站点ID、Rate：有效率</returns>
        public DataTable GetPointDataValidRate(string dateTime)
        {
            try
            {
                string querySql = string.Format(@"
                          select PointId,
                                 ISNULL(dbo.F_Round(SUM(QualifiedNumber)/(SUM(CollectionNumber) + 0.0)*100, 2) + '%', '--') as [Rate]
                            from dbo.TB_ReportQualifiedRateByDay
                           where ApplicationUid = '{0}'
                             and convert(char(7), [ReportDateTime], 20) = '{1}'
                           group by PointId", APP_UID, dateTime);

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }

        /// <summary>
        /// 根据时间、区域ID获取区域数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="areaId">区域ID</param>
        /// <returns></returns>
        public double GetAreaDataValidRate(string dateTime, string areaId)
        {
            return 23.67;
        }

        /// <summary>
        /// 根据时间获取运营商数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>Supplier: 运营商名称、Rate：有效率</returns>
        public DataTable GetSupplierDataValidRate(string dateTime)
        {
            try
            {
                string querySql = string.Format(@"
                           select svc.itemtext as Supplier,
                                  temp.Rate  
                             from dbo.SY_View_CodeMainItem svc
                            inner join (
							          select symp.OperatorsUid, 
									         dbo.F_Round(SUM(QualifiedNumber)/(SUM(CollectionNumber) + 0.0)*100, 1) as [Rate]
								        from dbo.TB_ReportQualifiedRateByDay trq
							           inner join dbo.SY_MonitoringPoint symp
								          on trq.PointId = symp.PointId
								         and trq.ApplicationUid = '{0}'
								         and convert(char(7), [ReportDateTime], 20) = '{1}' 
							           group by symp.OperatorsUid
	                                   ) as temp
	                           on svc.itemguid = temp.OperatorsUid
	                        order by svc.SortNumber desc", APP_UID, dateTime);

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }

        /// <summary>
        /// 根据站点ID获取运营商列表信息
        /// </summary>
        /// <param name="pointIds">站点ID(多个站点以英文,分割)</param>
        /// <returns>Supplier: 运营商名称、SupplierId：运营商ID</returns>
        public DataTable GetSupplierByPointIds(string pointIds)
        {
            try
            {
                string querySql = string.Format(@"
                       select svc.itemtext as Supplier,
                              temp.OperatorsUid as SupplierId
                         from dbo.SY_View_CodeMainItem svc
                        inner join (
							      select OperatorsUid
								    from dbo.SY_MonitoringPoint symp
								   where symp.ApplicationUid = '{0}'
								     and pointId in ({1})
							       group by OperatorsUid
	                               ) as temp
	                       on svc.itemguid = temp.OperatorsUid
	                    order by svc.SortNumber desc", APP_UID, pointIds);

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }

        /// <summary>
        /// 根据站点ID和运营商Id筛选站点信息
        /// </summary>
        /// <param name="pointIds">站点ID(多个站点以英文,分割)</param>
        /// <param name="supplierId">运营商ID</param>
        /// <returns>PointName: 站点名称</returns>
        public DataTable GetPointNameByCondition(string pointIds, string supplierId)
        {
            try
            {
                string querySql = string.Format(@"
	                     select MonitoringPointName as PointName
					       from dbo.SY_MonitoringPoint
					      where ApplicationUid = '{0}'
					        and pointId in ({1})
					        and OperatorsUid = '{2}'
					      order by OrderByNum desc", APP_UID, pointIds, supplierId);

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }

        /// <summary>
        /// 根据时间获取因子数据合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pollutantCode">因子编码</param>
        /// <returns></returns>
        public double GetPollutantValidRate(string dateTime, string pollutantCode, string pointIds)
        {
            return 59.89;
        }

        /// <summary>
        /// 根据时间获取因子比对合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pollutantCode">因子编码</param>
        /// <returns></returns>
        public double GetPollutantCompareValidRate(string dateTime, string pollutantCode, string pointIds)
        {
            return 59.89;
        }

        /// <summary>
        /// 根据时间获取点位各类水质等级数量(剔除总氮的计算)
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>Level: 水质等级、Count：个数</returns>
        public DataTable GetPointLevelCountExcludeTN(string dateTime)
        {
            try
            {
                string querySql = string.Format(@"
                            select sye.Class as Level,
                                   COUNT(sye.Class) as Count
                              from [WaterReport].[TB_MonthReport] wtm
                             inner join dbo.SY_EQI sye 
                                on wtm.Grade = sye.IEQI
                             where wtm.Grade = sye.IEQI
                               and pollutantcode='WaterQuality'
                               and convert(char(7), [ReportDateTime], 20) = '{0}'
                               and sye.ApplicationUid = '{1}'
                             group by sye.Class
                             order by Count desc", dateTime, APP_UID);

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }

        /// <summary>
        /// 根据时间获取点位各类水质等级数量
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>Level: 水质等级、Count：个数</returns>
        public DataTable GetPointLevelCount(string dateTime, string pointIds)
        {
            try
            {
                string querySql = string.Format(@"
                                    select Level,
                                           COUNT(Level) as Count
                                      from (
									        select ISNULL(dbo.F_GetPointAllPollutantMonthWQ_LEVEL(PointId, '{0}-01 00:00:00'), '--') as Level 
									          from dbo.SY_MonitoringPoint 
									         where ApplicationUid = '{1}'
                                               and PointId in ({2})) as lel
							         where Level is not null
							         group by Level", dateTime, APP_UID, pointIds);

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }

        /// <summary>
        /// 根据时间获取点位水质等级(剔除总氮的计算)
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointName: 点位名称、Level: 水质等级</returns>
        public DataTable GetPointLevelExcludeTN(string dateTime)
        {
            try
            {
                string querySql = string.Format(@"
                            select sym.PointId,
                                   sym.MonitoringPointName as PointName,
                                   convert(char(7), [ReportDateTime], 20) as Tstamp,
                                   sye.Class as Level 
                              from [WaterReport].[TB_MonthReport] wtm
                             inner join dbo.SY_MonitoringPoint sym 
                                on wtm.PointId = sym.PointId
                             inner join dbo.SY_EQI sye 
                                on wtm.Grade = sye.IEQI
                             where wtm.Grade = sye.IEQI
                               and pollutantcode='WaterQuality'
                               and convert(char(7), [ReportDateTime], 20) = '{0}'
                               and sye.ApplicationUid = '{1}'", dateTime, APP_UID);

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }


        /// <summary>
        /// 根据时间获取点位水质等级
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointName: 点位名称、Level: 水质等级</returns>
        public DataTable GetPointLevel(string dateTime)
        {
            try
            {
                string querySql = string.Format(@"
                            select PointId,
                                   MonitoringPointName as PointName,
                                   isnull(dbo.F_GetPointAllPollutantMonthWQ_LEVEL(PointId, '{0}-01 00:00:00'), '--') as Level 
                              from dbo.SY_MonitoringPoint
                             where ApplicationUid = '{1}'", dateTime, APP_UID);

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }

        /// <summary>
        /// 根据时间、点位获取点位水质等级(剔除总氮的计算)
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pointId">测点ID</param>
        /// <returns>水质等级</returns>
        public string GetPointLevelExcludeTN(string dateTime, string pointId)
        {
            try
            {
                string querySql = string.Format(@"
                            select sye.Class
                              from [WaterReport].[TB_MonthReport] wtm, dbo.SY_EQI sye
                             where wtm.Grade = sye.IEQI
                               and PointId={0}
                               and pollutantcode='WaterQuality'
                               and convert(char(7), [ReportDateTime], 20) = '{1}'
                               and sye.ApplicationUid = '{2}'"
                                , pointId
                                , dateTime
                                , APP_UID);

                DataTable dataDt = dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
                if (dataDt.Rows.Count > 0)
                {
                    return dataDt.Rows[0]["Class"].ToString();
                }
            }
            catch (Exception e)
            {
            }

            return "--";
        }

        /// <summary>
        /// 根据时间、点位获取点位水质等级
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pointId">测点ID</param>
        /// <returns>水质等级</returns>
        public string GetPointLevel(string dateTime, string pointId)
        {
            try
            {
                string querySql = string.Format(@"
                            select isnull(dbo.F_GetPointAllPollutantMonthWQ_LEVEL({0}, '{1}-01 00:00:00') + '类', '--') as Level"
                                , pointId
                                , dateTime);

                DataTable dataDt = dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
                if (dataDt.Rows.Count > 0)
                {
                    return dataDt.Rows[0]["Level"].ToString();
                }
            }
            catch (Exception e)
            {
            }

            return "--";
        }

        /// <summary>
        /// 根据时间、点位获取点位水质状况
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pointId">测点ID</param>
        /// <returns>水质状况(优、良好)</returns>
        public string GetPointGrade(string dateTime, string pointId)
        {
            try
            {
                string querySql = string.Format(@"
                            		 select Grade
							           from dbo.SY_EQI
							          where IEQI = dbo.F_GetPointAllPollutantMonthWQ_LEVEL_IEQI({0}, '{1}-01 00:00:00')
							            and ApplicationUid='{2}'"
                                        , pointId
                                        , dateTime
                                        , APP_UID);

                DataTable dataDt = dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
                if (dataDt.Rows.Count > 0)
                {
                    return dataDt.Rows[0]["Grade"].ToString();
                }
            }
            catch (Exception e)
            {
            }

            return "--";
        }

        /// <summary>
        /// 根据条件获取点位因子浓度值
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pointId">测点ID</param>
        /// <param name="pollutantCode">因子编码</param>
        /// <returns></returns>
        public decimal GetPointPollutantValue(string dateTime, string pointId, string pollutantCode)
        {
            try
            {
                string querySql = string.Format(@"
                         select cast(ISNULL(dbo.F_Round(PollutantValue, 2),-1) as numeric(18, 2)) as PollutantValue
                           from WaterReport.TB_MonthReport
		                  where PointId = {0}
                            and PollutantCode = '{1}'
                            and convert(char(7), [ReportDateTime], 20) = '{2}'"
                            , pointId
                            , pollutantCode
                            , dateTime);

                DataTable dataDt = dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
                if (dataDt.Rows.Count > 0)
                {
                    return Convert.ToDecimal(dataDt.Rows[0]["PollutantValue"]);
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        /// <summary>
        /// 根据时间获取点位月度因子平均浓度值
        /// </summary>
        /// <param name="startTime">开始时间(格式：yyyy-MM)</param>
        /// <param name="endTime">结束时间(格式：yyyy-MM)</param>
        /// <returns>
        /// PointName: 点位名称
        /// Tstamp: 数据时间
        /// do：溶解氧
        /// mnO4 ：高锰酸盐指数
        /// nh: 氨氮
        /// tp：总磷
        /// tn：总氮
        /// algalDensity：藻密度
        /// </returns>
        public DataTable GetPointListMonthData(string startTime, string endTime)
        {
            try
            {
                string querySql = string.Format(@"
                        select sym.monitoringPointName as PointName,
                               Tstamp,
                               do,
                               mnO4,
                               nh,
                               tp,
                               tn,
                               algalDensity
                          from (
		                        select PointId,
			                           convert(char(7), [ReportDateTime], 20) as Tstamp,
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w01009' then PollutantValue end), 2) as numeric(18, 2)) as 'do',
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w01019' then PollutantValue end), 2) as numeric(18, 2)) as 'mnO4',
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w21003' then PollutantValue end), 2) as numeric(18, 2)) as 'nh',
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w21011' then PollutantValue end), 2) as numeric(18, 2)) as 'tp',
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w21001' then PollutantValue end), 2) as numeric(18, 2)) as 'tn',
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w19011' then PollutantValue end), 0) as numeric(18, 2)) as 'algalDensity'       
		                          from WaterReport.TB_MonthReport
		                         where convert(char(7), [ReportDateTime], 20) >= '{0}'
		                           and convert(char(7), [ReportDateTime], 20) <= '{1}'
		                         group by PointId, convert(char(7), [ReportDateTime], 20)) as wti
                          inner join dbo.SY_MonitoringPoint sym
                             on wti.PointId = sym.PointId 
                          order by PointName, Tstamp desc", startTime, endTime);

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }

        /// <summary>
        /// 根据站点和时间获取点位(当月、上个月、去年当前月)月度因子平均浓度值
        /// </summary>
        /// <param name="pointIds">测点ID列表</param>
        /// <param name="currMonthTime">当前月时间</param>
        /// <returns>
        /// PointName: 点位名称
        /// Tstamp: 数据时间
        /// do：溶解氧
        /// mnO4 ：高锰酸盐指数
        /// nh: 氨氮
        /// tp：总磷
        /// tn：总氮
        /// algalDensity：藻密度
        /// </returns>
        public DataTable GetPointListMonthData(string[] pointIds, DateTime currMonthTime)
        {
            try
            {
                string pointId = string.Join(",", pointIds);
                StringBuilder times = new StringBuilder();
                times.AppendFormat(@"'{0}', '{1}', '{2}'"
                                   , currMonthTime.ToString("yyyy-MM")
                                   , currMonthTime.AddMonths(-1).ToString("yyyy-MM")
                                   , currMonthTime.AddYears(-1).ToString("yyyy-MM"));

                string querySql = string.Format(@"
                        select sym.monitoringPointName as PointName,
                               Tstamp,
                               do,
                               mnO4,
                               nh,
                               tp,
                               tn,
                               algalDensity 
                          from (
		                        select PointId,
			                           convert(char(7), [ReportDateTime], 20) as Tstamp,
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w01009' then PollutantValue end), 2) as numeric(18, 2)) as 'do',
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w01019' then PollutantValue end), 2) as numeric(18, 2)) as 'mnO4',
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w21003' then PollutantValue end), 2) as numeric(18, 2)) as 'nh',
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w21011' then PollutantValue end), 2) as numeric(18, 2)) as 'tp',
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w21001' then PollutantValue end), 2) as numeric(18, 2)) as 'tn',
			                           cast(dbo.F_Round(AVG(case when PollutantCode = 'w19011' then PollutantValue end), 0) as numeric(18, 2)) as 'algalDensity'       
		                          from WaterReport.TB_MonthReport
		                         where PointId in ({0})
		                           and convert(char(7), [ReportDateTime], 20) in ({1})
		                         group by PointId, convert(char(7), [ReportDateTime], 20)) as wti
                          inner join dbo.SY_MonitoringPoint sym
                             on wti.PointId = sym.PointId 
                          order by PointName, Tstamp desc", pointId, times);

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return null;
        }

        /// <summary>
        /// 根据时间获取点位首要污染物
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId：站点ID、PointName: 点位名称、PrimaryPollutant: 首要污染物</returns>
        public DataTable GetPointPrimaryPollutant(string dateTime)
        {
            try
            {
                string querySql = string.Format(@"
                            select sym.PointId,
                                   sym.MonitoringPointName as PointName,
                                   wtm.Description as PrimaryPollutant
                              from [WaterReport].[TB_MonthReport] wtm
                             inner join dbo.SY_MonitoringPoint sym
                                on wtm.PointId = sym.PointId
                               and sym.ApplicationUid = '{0}'
                               and wtm.PollutantCode = 'WaterQuality'
                               and convert(char(7), [ReportDateTime], 20) = '{1}'", APP_UID, dateTime);

                return dbHelper.ExecuteDataTable(querySql, AMS_MonitorBusiness_Conn);
            }
            catch (Exception e)
            {
            }

            return new DataTable();
        }

        /// <summary>
        /// 取得太湖西部9条入湖河流当月、上月、上一年等级数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataView GetThreeMonthLevel(string[] portIds, int year, int month)
        {
            string date = year.ToString() + "-" + month.ToString().PadLeft(2, '0') + "-01";
            DateTime dtCur = Convert.ToDateTime(date);
            DateTime dtPre = dtCur.AddMonths(-1);
            DateTime dtPreYear = dtCur.AddYears(-1);


            //站点处理
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");

            string sql = string.Format(@"
                SELECT curMon.PointId
	                ,curMon.MonitoringPointName
	                ,curMon.Grade AS curGrade
                    ,curMon.Class
	                ,curMon.Description
	                ,preMon.Grade AS preGrade
	                ,preYearMon.Grade AS preYearGrade
                FROM 
                (
	                SELECT data.[PointId]
		                ,smp.MonitoringPointName
		                ,data.[EQI]
		                ,data.[Grade]
		                ,EQI.Class
		                ,data.Description
	                FROM [WaterReport].[TB_MonthReport] AS data
	                LEFT JOIN dbo.SY_EQI AS EQI
		                ON data.Grade = EQI.IEQI AND EQI.ApplicationUid='watrwatr-watr-watr-watr-watrwatrwatr'
	                INNER JOIN dbo.SY_MonitoringPoint AS smp
		                ON data.PointId = smp.PointId
	                WHERE PollutantCode='WaterQuality'
                        AND data.PointId in ({1})
	                    AND ReportDateTime='{0}'
                ) AS curMon
                LEFT JOIN 
                (
	                SELECT [PointId]
		                ,[EQI]
		                ,[Grade]
	                FROM [WaterReport].[TB_MonthReport]
	                WHERE PollutantCode='WaterQuality'
                        AND PointId in ({1})
	                    AND ReportDateTime='{2}'
                ) AS preMon
	                ON curMon.PointId = preMon.PointId
                LEFT JOIN 
                (
	                SELECT [PointId]
		                ,[EQI]
		                ,[Grade]
	                FROM [WaterReport].[TB_MonthReport]
	                WHERE PollutantCode='WaterQuality'
                        AND PointId in ({1})
	                    AND ReportDateTime='{3}'
                ) AS preYearMon
	                ON curMon.PointId = preYearMon.PointId ", date, portIdsStr, dtPre.ToString("yyyy-MM-dd"), dtPreYear.ToString("yyyy-MM-dd"));

            return dbHelper.ExecuteDataView(sql, AMS_MonitorBusiness_Conn);
        }

    }
}
