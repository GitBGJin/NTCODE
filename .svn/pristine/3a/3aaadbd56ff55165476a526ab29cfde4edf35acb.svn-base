using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Core.Generic;


namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    /// <summary>
    /// 名称：MaintenanceDataDAL.cs
    /// 创建人：王琳
    /// 创建日期：2015-10-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：本周运维数据访问层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MaintenanceDataDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 根据运行周报本周运维概况数据
        /// </summary>
        /// <param name="portIds">点位Id</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public DataView GetMaintenanceData(string[] portIds,DateTime beginTime,DateTime endTime)
        {
            //站点处理
            string portIdsStr = string.Empty;
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                portIdsStr = " PointId =" + portIds[0];
            }
            else
            {
                portIdsStr = " PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
            }
            string sql = string.Format(@"select distinct PointId,PointName,DATENAME(YEAR,DateTime)+'年'+DATENAME(MONTH,DateTime)+'月'+DATENAME(DAY,DateTime)+'日' as MatanceDateTime,AbnormalItem+'-'+AbnormalName as Content,'' as Remark
                                        from TB_Abnormal where {0} 
                                        and CONVERT(char(10),DateTime,20)>='{1}' and CONVERT(char(10),DateTime,20)<='{2}'
                                        union
                                        select distinct PointId,PointName,DATENAME(YEAR,ChangeDate)+'年'+DATENAME(MONTH,ChangeDate)+'月'+DATENAME(DAY,ChangeDate)+'日' as MatanceDateTime,DATENAME(YEAR,ChangeDate)+'年'+DATENAME(MONTH,ChangeDate)+'月'+DATENAME(DAY,ChangeDate)+'日,'+InstrumentName+'更换'+PartName as content,'' as remark
                                        from TB_PartChange where {0}
                                        and  CONVERT(char(10),ChangeDate,20)>='{1}' and CONVERT(char(10),ChangeDate,20)<='{2}'
                                        union 
                                        select distinct PointId,PointName,DATENAME(YEAR,ChangeDate)+'年'+DATENAME(MONTH,ChangeDate)+'月'+DATENAME(DAY,ChangeDate)+'日' as MatanceDateTime,DATENAME(YEAR,ChangeDate)+'年'+DATENAME(MONTH,ChangeDate)+'月'+DATENAME(DAY,ChangeDate)+'日,'+InstrumentName+'更换'+StandardSolutionIdName as content,'' as remark
                                        from TB_StandardSolutionChange where {0}
                                        and CONVERT(char(10),ChangeDate,20)>='{1}' and CONVERT(char(10),ChangeDate,20)<='{2}'
                        ", portIdsStr, beginTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd"));
            return g_DatabaseHelper.ExecuteDataView(sql, "AMS_MonitoringBusinessConnection");
        }
    }
}
