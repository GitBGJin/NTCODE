using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：HourReportForUpLoadDAL.cs
    /// 创建人：朱佳伟
    /// 创建日期：2016-01-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：上传审核数据到临时表
    /// 环境空气发布：
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class HourReportForUpLoadDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 存储过程处理类
        /// </summary>
        BaseDAHelper g_BaseDAHelper = Singleton<BaseDAHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Hour);
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "AirReport.TB_HourReport_UpLoad";

        /// <summary>
        /// 获取用户上传的数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="UserGuid">上传用户Uid</param>
        /// <returns>DataTable</returns>
        public DataTable GetUpLoadData(List<int> PointIds, List<string> PollutantCodes, int DataType, string UserGuid)
        {
            if (PollutantCodes == null || PollutantCodes.Count == 0)
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t.*,mp.MonitoringPointName as PointName from (");
            strSql.Append("select PointId,Tstamp ");
            foreach (string code in PollutantCodes)
            {
                strSql.Append(",MAX(CASE PollutantCode when '" + code + "' then CAST(PollutantValue as decimal(18,4)) end ) as '" + code + "' ");
                strSql.Append(",MAX(CASE PollutantCode when '" + code + "' then AuditFlag end ) as '" + code + "_Flag' ");
            }
            strSql.Append("from " + tableName + " ");
            strSql.Append("where DataType='" + DataType + "' and CreatUser='" + UserGuid + "' ");
            if (PointIds != null && PointIds.Count > 0)
            {
                strSql.Append("and ( ");
                for (int j = 0; j < PointIds.Count; j++)
                {
                    if (j == 0)
                    {
                        strSql.Append("PointId='" + PointIds[j] + "' ");
                    }
                    else
                    {
                        strSql.Append("or PointId='" + PointIds[j] + "' ");
                    }
                }
                strSql.Append(" ) ");
            }
            strSql.Append("group by PointId,Tstamp ");
            strSql.Append(") as t ");
            strSql.Append("inner join dbo.SY_MonitoringPoint mp on t.PointId=mp.PointId ");
            strSql.Append("order by mp.OrderByNum desc,t.Tstamp asc ");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 删除用户上传数据
        /// </summary>
        /// <param name="DataType">数据类型</param>
        /// <param name="UserGuid">上传用户Uid</param>
        public void DeleteByUser(int DataType, string UserGuid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from " + tableName + " where DataType='" + DataType + "' and CreatUser='" + UserGuid + "'");
            g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="models">导入的数据实体数组</param>
        public void InsertAll(List<AirHourReportForUpLoadEntity> models)
        {
            if (models != null && models.Count > 0)
            {
                List<CommandInfo> sqllist = new List<CommandInfo>();
                foreach (AirHourReportForUpLoadEntity model in models)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(" insert into [AirReport].[TB_HourReport_UpLoad] ([PointId],[Tstamp],[HourOfDay],[PollutantCode],[PollutantValue],[DataType] ");
                    strSql.Append(" ,[Description],[CreatUser],[CreatDateTime]) ");
                    strSql.Append(" values (@PointId,@Tstamp,@HourOfDay,@PollutantCode,@PollutantValue,@DataType ");
                    strSql.Append(" ,@Description,@CreatUser,@CreatDateTime) ");
                    SqlParameter[] parameters = { 
                                new SqlParameter("@PointId", SqlDbType.Int),
                                new SqlParameter("@Tstamp", SqlDbType.DateTime),
                                new SqlParameter("@HourOfDay", SqlDbType.Int),
                                new SqlParameter("@PollutantCode", SqlDbType.NVarChar, 20),
                                new SqlParameter("@PollutantValue", SqlDbType.Decimal),
                                new SqlParameter("@DataType", SqlDbType.Int),
                                new SqlParameter("@Description", SqlDbType.NVarChar, 500),
                                new SqlParameter("@CreatUser", SqlDbType.NVarChar, 10),
                                new SqlParameter("@CreatDateTime", SqlDbType.DateTime)};
                    parameters[0].Value = model.PointId;
                    parameters[1].Value = model.Tstamp;
                    parameters[2].Value = model.HourOfDay;
                    parameters[3].Value = model.PollutantCode;
                    parameters[4].Value = model.PollutantValue;
                    parameters[5].Value = model.DataType;
                    parameters[6].Value = model.Description;
                    parameters[7].Value = model.CreatUser;
                    parameters[8].Value = model.CreatDateTime;
                    CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                    sqllist.Add(cmd);
                }
                g_DatabaseHelper.ExecuteSqlTranWithIndentity(sqllist, connection);
            }
        }

        /// <summary>
        /// 执行存储过程，批量上报区县数据
        /// </summary>
        /// <param name="UserGuid">操作用户标识</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="ApplicationUid">系统Uid</param>
        public void BatchAddAirHourReport(string UserGuid, int DataType, string ApplicationUid)
        {
            g_BaseDAHelper.ClearParameters();

            SqlParameter pramUserGuid = new SqlParameter();
            pramUserGuid = new SqlParameter();
            pramUserGuid.SqlDbType = SqlDbType.NVarChar;
            pramUserGuid.ParameterName = "@UserGuid";
            pramUserGuid.Value = UserGuid;
            g_BaseDAHelper.SetProcedureParameters(pramUserGuid);

            SqlParameter pramDataType = new SqlParameter();
            pramDataType = new SqlParameter();
            pramDataType.SqlDbType = SqlDbType.Int;
            pramDataType.ParameterName = "@DataType";
            pramDataType.Value = DataType;
            g_BaseDAHelper.SetProcedureParameters(pramDataType);

            SqlParameter pramApplicationUid = new SqlParameter();
            pramApplicationUid = new SqlParameter();
            pramApplicationUid.SqlDbType = SqlDbType.NVarChar;
            pramApplicationUid.ParameterName = "@ApplicationUid";
            pramApplicationUid.Value = ApplicationUid;
            g_BaseDAHelper.SetProcedureParameters(pramApplicationUid);

            //执行存储过程
            g_BaseDAHelper.ExecuteProcNonQuery("BatchAddAirHourReport_UpLoad", connection);
        }
    }
}
