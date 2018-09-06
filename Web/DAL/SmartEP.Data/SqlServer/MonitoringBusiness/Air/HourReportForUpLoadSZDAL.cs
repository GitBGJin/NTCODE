using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：HourReportForUpLoadSZDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2016-01-27
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：上传审核数据到临时表
    /// 环境空气发布：
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class HourReportForUpLoadSZDAL
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
        private string connectionBaseStr = "AMS_BaseDataConnection";
        /// <summary>
        /// 数据库表名（苏州）
        /// </summary>
        private string tableNameSZ = "AirReport.TB_HourReport_UpLoad_SZ";

        /// <summary>
        /// 导入区县数据记录
        /// </summary>
        /// <param name="ProblemData"></param>
        /// <param name="PointGuid"></param>
        /// <param name="fileName"></param>
        public void insertQuXianData(string ProblemData, string PointGuid, string fileName)
        {
            StringBuilder builderUpData = new StringBuilder();
            builderUpData.Append("update [dbo].[TB_QuXianData]");
            if (ProblemData != "")
                builderUpData.Append("set IsSuccess=" + "1" + ",ProblemData='" + ProblemData + "',InsertTime='" + DateTime.Now + "'");
            else
                builderUpData.Append("set IsSuccess=" + "1" + ",ProblemData=null,InsertTime='" + DateTime.Now + "'");
            builderUpData.Append(" where PointGuid='" + PointGuid + "' AND Tstamp='" + fileName + "'");
            string strSqlUp = builderUpData.ToString();
            g_DatabaseHelper.ExecuteNonQuery(strSqlUp, "AMS_MonitoringBusinessConnection");
        }
        /// <summary>
        /// 导入区县数据记录
        /// </summary>
        /// <param name="ProblemData"></param>
        /// <param name="PointGuid"></param>
        /// <param name="fileName"></param>
        public void insertQuXianDataNew(string ProblemData, DateTime dtStart, DateTime dtEnd)
        {
            StringBuilder builderUpData = new StringBuilder();
            builderUpData.Append("update [dbo].[TB_QuXianData]");
            if (ProblemData != "")
                builderUpData.Append("set IsSuccess=" + "1" + ",ProblemData='" + ProblemData + "',InsertTime='" + DateTime.Now + "'");
            else
                builderUpData.Append("set IsSuccess=" + "1" + ",ProblemData=null,InsertTime='" + DateTime.Now + "'");
            builderUpData.Append(" WHERE Tstamp>='" + dtStart + "' AND Tstamp<='" + dtEnd + "' AND IsSuccess=0");
            string strSqlUp = builderUpData.ToString();
            g_DatabaseHelper.ExecuteNonQuery(strSqlUp, "AMS_MonitoringBusinessConnection");
        }
        /// <summary>
        /// 获取因子浓度限值
        /// </summary>
        /// <param name="PollutantCodes"></param>
        /// <returns></returns>
        public DataTable GetFactorLimt(List<string> PollutantCodes)
        {
            string strFactor = "'" + string.Join("','", PollutantCodes.ToArray()) + "'";
            //ArrayList TableParameters = null;
            //dtLimt = new DataTable();
            string strSql = string.Format(@"SELECT b.PointId
      ,[ExcessiveUid]
      ,c.PollutantCode as PollutantCode
      ,c.PollutantName as PollutantName
      ,[NotifyGradeUid]
      ,[DataTypeUid]
      ,[AdvanceLow]
      ,[AdvanceUpper]
      ,[AdvanceRange]
      ,[ExcessiveUpper]
      ,[ExcessiveLow]
      ,[ExcessiveRange]
      ,[ReplaceStatus]
      ,[StandardType]
      ,[ExcessiveRatio]
      ,[UseForUid]
      ,[NotifyOrNot]  

  FROM [BusinessRule].[TB_ExcessiveSetting] as a left join MPInfo.TB_MonitoringPoint as b on a.MonitoringPointUid=b.MonitoringPointUid
  left join InstrInfo.TB_InstrumentChannels as c on a.InstrumentChannelsUid=c.InstrumentChannelsUid
  where DataTypeUid='1b6367f1-5287-4c14-b120-7a35bd176db1' and a.EnableOrNot=1 and UseForUid='d10be26a-2001-4ac4-ba36-82d10d3c0156' and PollutantCode in ({0})", strFactor);
            return g_DatabaseHelper.ExecuteDataTable(strSql, connectionBaseStr);
        }
        /// <summary>
        /// 获取用户上传的数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="UserGuid">上传用户Uid</param>
        /// <returns>DataTable</returns>
        public DataTable GetUpLoadDataSZ(List<int> PointIds, List<string> PollutantCodes, int DataType, string UserGuid)
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
            strSql.Append("from " + tableNameSZ + " ");
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
        public void DeleteByUserSZ(int DataType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from " + tableNameSZ + " where DataType='" + DataType + "'");
            g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="models">导入的数据实体数组</param>
        public void InsertAllSZ(List<AirHourReportForUpLoadSZEntity> models)
        {
            if (models != null && models.Count > 0)
            {
                List<CommandInfo> sqllist = new List<CommandInfo>();
                foreach (AirHourReportForUpLoadSZEntity model in models)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(" insert into [AirReport].[TB_HourReport_UpLoad_SZ] ([PointId],[MonitoringRegionUid],[Tstamp],[HourOfDay],[PollutantCode],[PollutantValue],[DataType] ");
                    strSql.Append(" ,[Description],[CreatUser],[CreatDateTime],[Remark]) ");
                    strSql.Append(" values (@PointId,@MonitoringRegionUid,@Tstamp,@HourOfDay,@PollutantCode,@PollutantValue,@DataType ");
                    strSql.Append(" ,@Description,@CreatUser,@CreatDateTime,@Remark) ");
                    SqlParameter[] parameters = { 
                                new SqlParameter("@PointId", SqlDbType.Int),
                                new SqlParameter("@MonitoringRegionUid", SqlDbType.VarChar, 50),
                                new SqlParameter("@Tstamp", SqlDbType.DateTime),
                                new SqlParameter("@HourOfDay", SqlDbType.Int),
                                new SqlParameter("@PollutantCode", SqlDbType.NVarChar, 20),
                                new SqlParameter("@PollutantValue", SqlDbType.Decimal),
                                new SqlParameter("@DataType", SqlDbType.Int),
                                new SqlParameter("@Description", SqlDbType.NVarChar, 500),
                                new SqlParameter("@CreatUser", SqlDbType.NVarChar, 10),
                                new SqlParameter("@CreatDateTime", SqlDbType.DateTime),
                                new SqlParameter("@Remark", SqlDbType.NVarChar,500)};
                    parameters[0].Value = model.PointId;
                    parameters[1].Value = model.MonitoringRegionUid;
                    parameters[2].Value = model.Tstamp;
                    parameters[3].Value = model.HourOfDay;
                    parameters[4].Value = model.PollutantCode;
                    parameters[5].Value = model.PollutantValue;
                    parameters[6].Value = model.DataType;
                    parameters[7].Value = model.Description;
                    parameters[8].Value = model.CreatUser;
                    parameters[9].Value = model.CreatDateTime;
                    parameters[10].Value = model.Remark;
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
        public void BatchAddAirHourReportSZ(string UserGuid, int DataType, string ApplicationUid)
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
            g_BaseDAHelper.ExecuteProcNonQuery("BatchAddAirHourReport_UpLoad_SZ", connection);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public DataView GetData(string PointGuid, string fileName)
        {
            string tableName = "TB_QuXianData";
            try
            {
                string sql = string.Format(@"SELECT *
                    FROM {0}
                    WHERE PointGuid='{1}' AND Tstamp='{2}'", tableName, PointGuid, fileName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public DataView GetDataNew(DateTime dtStart, DateTime dtEnd)
        {
            string tableName = "TB_QuXianData";
            try
            {
                string sql = string.Format(@"SELECT *
                    FROM {0}
                    WHERE Tstamp>='{1}' AND Tstamp<='{2}' AND IsSuccess=0", tableName, dtStart, dtEnd);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public void GetAddData(string PointGuid, string PointName, DateTime dtTime, string User, string fileName)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("insert into [AMS_MonitorBusiness].[dbo].[TB_QuXianData](PointGuid,PointName,IsInsertService,InsertServiceTime,IsSuccess,InsertTime,ProblemData,CreaterUser,Tstamp)");
                builder.Append(" values('" + PointGuid + "','" + PointName + "'," + 1 + ",'" + dtTime + "'," + 1 + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',null,'" + User + "','" + fileName + "'");
                builder.Append(")");
                string strSql = builder.ToString();
                g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public void GetUpdateData(string PointGuid, string fileName, string ProblemData)
        {
            try
            {
                StringBuilder builderUpData = new StringBuilder();
                builderUpData.Append("update [AMS_MonitorBusiness].[dbo].[TB_QuXianData]");
                builderUpData.Append("set IsInsertService=1, IsSuccess=0,ProblemData='" + ProblemData + "',InsertTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                builderUpData.Append(" where PointGuid='" + PointGuid + "' AND Tstamp='" + fileName + "'");
                string strSqlUp = builderUpData.ToString();
                g_DatabaseHelper.ExecuteNonQuery(strSqlUp.ToString(), connection);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
