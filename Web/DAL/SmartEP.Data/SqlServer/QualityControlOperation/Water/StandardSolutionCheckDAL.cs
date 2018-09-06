using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：StandardSolutionCheckDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 质控任务数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class StandardSolutionCheckDAL
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

        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_StandardSolutionCheck";

        /// <summary>
        /// 工作ID
        /// </summary>
        private string m_ActionID = string.Empty;
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public StandardSolutionCheckDAL()
        {
            //connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Hour);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">质控任务实体</param>
        /// <returns></returns>
        public bool Add(StandardSolutionCheckEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.MissionID != null)
                {
                    strSql1.Append("MissionID,");
                    strSql2.Append("'" + model.MissionID + "',");
                }
                if (model.ActionID != null)
                {
                    strSql1.Append("ActionID,");
                    strSql2.Append("'" + model.ActionID + "',");
                }
                if (model.SampleNumber != null)
                {
                    strSql1.Append("SampleNumber,");
                    strSql2.Append("'" + model.SampleNumber + "',");
                }
                if (model.PointId != null)
                {
                    strSql1.Append("PointId,");
                    strSql2.Append("" + model.PointId + ",");
                }
                if (model.PollutantCode != null)
                {
                    strSql1.Append("PollutantCode,");
                    strSql2.Append("" + model.PollutantCode + ",");
                }
                if (model.Unit != null)
                {
                    strSql1.Append("Unit,");
                    strSql2.Append("'" + model.Unit + "',");
                }
                if (model.PollutantValue != null)
                {
                    strSql1.Append("PollutantValue,");
                    strSql2.Append("'" + model.PollutantValue + "',");
                }
                if (model.StandardValue != null)
                {
                    strSql1.Append("StandardValue,");
                    strSql2.Append("" + model.StandardValue + ",");
                }
                if (model.AddValue != null)
                {
                    strSql1.Append("AddValue,");
                    strSql2.Append("" + model.AddValue + ",");
                }
                if (model.PollutantValueAdd != null)
                {
                    strSql1.Append("PollutantValueAdd,");
                    strSql2.Append("" + model.PollutantValueAdd + ",");
                }
                if (model.RelativeOffset != null)
                {
                    strSql1.Append("RelativeOffset,");
                    strSql2.Append("" + model.RelativeOffset + ",");
                }
                if (model.AbsoluteOffset != null)
                {
                    strSql1.Append("AbsoluteOffset,");
                    strSql2.Append("" + model.AbsoluteOffset + ",");
                }
                if (model.OffsetValue != null)
                {
                    strSql1.Append("OffsetValue,");
                    strSql2.Append("" + model.OffsetValue + ",");
                }
                if (model.Evaluate != null)
                {
                    strSql1.Append("Evaluate,");
                    strSql2.Append("'" + model.Evaluate + "',");
                }
                if (model.TestType != null)
                {
                    strSql1.Append("TestType,");
                    strSql2.Append("'" + model.TestType + "',");
                }
                if (model.DateStartTime != null)
                {
                    strSql1.Append("DateStartTime,");
                    strSql2.Append("'" + model.DateStartTime + "',");
                }
                if (model.DateEndTime != null)
                {
                    strSql1.Append("DateEndTime,");
                    strSql2.Append("'" + model.DateEndTime + "',");
                }
                if (model.Tstamp != null)
                {
                    strSql1.Append("Tstamp,");
                    strSql2.Append("'" + model.Tstamp + "',");
                }
                if (model.Tester != null)
                {
                    strSql1.Append("Tester,");
                    strSql2.Append("'" + model.Tester + "',");
                }
                if (model.PollutantName != null)
                {
                    strSql1.Append("PollutantName,");
                    strSql2.Append("'" + model.PollutantName + "',");
                }
                if (model.PointName != null)
                {
                    strSql1.Append("PointName,");
                    strSql2.Append("'" + model.PointName + "',");
                }
                if (model.OrderByNum != null)
                {
                    strSql1.Append("OrderByNum,");
                    strSql2.Append("'" + model.OrderByNum + "',");
                }
                if (model.Description != null)
                {
                    strSql1.Append("Description,");
                    strSql2.Append("'" + model.Description + "',");
                }
                if (model.CreatUser != null)
                {
                    strSql1.Append("CreatUser,");
                    strSql2.Append("'" + model.CreatUser + "',");
                }
                if (model.CreatDateTime != null)
                {
                    strSql1.Append("CreatDateTime,");
                    strSql2.Append("'" + model.CreatDateTime + "',");
                }
                if (model.UpdateUser != null)
                {
                    strSql1.Append("UpdateUser,");
                    strSql2.Append("'" + model.UpdateUser + "',");
                }
                if (model.UpdateDateTime != null)
                {
                    strSql1.Append("UpdateDateTime,");
                    strSql2.Append("'" + model.UpdateDateTime + "',");
                }
                if (model.TaskCode != null)
                {
                    strSql1.Append("TaskCode,");
                    strSql2.Append("'" + model.TaskCode + "',");
                }
                strSql.Append("insert into TB_StandardSolutionCheck(");
                strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                strSql.Append(")");
                strSql.Append(" values (");
                strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                strSql.Append(")");
                g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                //int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection));
                //if (rowsAffected > 0)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 批量增加数据
        /// </summary>
        /// <param name="model">质控任务实体数组</param>
        /// <returns></returns>
        public bool Add(params StandardSolutionCheckEntity[] models)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                foreach (StandardSolutionCheckEntity model in models)
                {
                    StringBuilder strSql1 = new StringBuilder();
                    StringBuilder strSql2 = new StringBuilder();
                    if (model.MissionID != null)
                    {
                        strSql1.Append("MissionID,");
                        strSql2.Append("'" + model.MissionID + "',");
                    }
                    if (model.ActionID != null)
                    {
                        strSql1.Append("ActionID,");
                        strSql2.Append("'" + model.ActionID + "',");
                    }
                    if (model.SampleNumber != null)
                    {
                        strSql1.Append("SampleNumber,");
                        strSql2.Append("'" + model.SampleNumber + "',");
                    }
                    if (model.PointId != null)
                    {
                        strSql1.Append("PointId,");
                        strSql2.Append("" + model.PointId + ",");
                    }
                    if (model.PollutantCode != null)
                    {
                        strSql1.Append("PollutantCode,");
                        strSql2.Append("" + model.PollutantCode + ",");
                    }
                    if (model.Unit != null)
                    {
                        strSql1.Append("Unit,");
                        strSql2.Append("'" + model.Unit + "',");
                    }
                    if (model.PollutantValue != null)
                    {
                        strSql1.Append("PollutantValue,");
                        strSql2.Append("'" + model.PollutantValue + "',");
                    }
                    if (model.StandardValue != null)
                    {
                        strSql1.Append("StandardValue,");
                        strSql2.Append("" + model.StandardValue + ",");
                    }
                    if (model.AddValue != null)
                    {
                        strSql1.Append("AddValue,");
                        strSql2.Append("" + model.AddValue + ",");
                    }
                    if (model.PollutantValueAdd != null)
                    {
                        strSql1.Append("PollutantValueAdd,");
                        strSql2.Append("" + model.PollutantValueAdd + ",");
                    }
                    if (model.RelativeOffset != null)
                    {
                        strSql1.Append("RelativeOffset,");
                        strSql2.Append("" + model.RelativeOffset + ",");
                    }
                    if (model.AbsoluteOffset != null)
                    {
                        strSql1.Append("AbsoluteOffset,");
                        strSql2.Append("" + model.AbsoluteOffset + ",");
                    }
                    if (model.OffsetValue != null)
                    {
                        strSql1.Append("OffsetValue,");
                        strSql2.Append("" + model.OffsetValue + ",");
                    }
                    if (model.Evaluate != null)
                    {
                        strSql1.Append("Evaluate,");
                        strSql2.Append("'" + model.Evaluate + "',");
                    }
                    if (model.TestType != null)
                    {
                        strSql1.Append("TestType,");
                        strSql2.Append("'" + model.TestType + "',");
                    }
                    if (model.DateStartTime != null)
                    {
                        strSql1.Append("DateStartTime,");
                        strSql2.Append("'" + model.DateStartTime + "',");
                    }
                    if (model.DateEndTime != null)
                    {
                        strSql1.Append("DateEndTime,");
                        strSql2.Append("'" + model.DateEndTime + "',");
                    }
                    if (model.Tstamp != null)
                    {
                        strSql1.Append("Tstamp,");
                        strSql2.Append("'" + model.Tstamp + "',");
                    }
                    if (model.Tester != null)
                    {
                        strSql1.Append("Tester,");
                        strSql2.Append("'" + model.Tester + "',");
                    }
                    if (model.PollutantName != null)
                    {
                        strSql1.Append("PollutantName,");
                        strSql2.Append("'" + model.PollutantName + "',");
                    }
                    if (model.PointName != null)
                    {
                        strSql1.Append("PointName,");
                        strSql2.Append("'" + model.PointName + "',");
                    }
                    if (model.OrderByNum != null)
                    {
                        strSql1.Append("OrderByNum,");
                        strSql2.Append("'" + model.OrderByNum + "',");
                    }
                    if (model.Description != null)
                    {
                        strSql1.Append("Description,");
                        strSql2.Append("'" + model.Description + "',");
                    }
                    if (model.CreatUser != null)
                    {
                        strSql1.Append("CreatUser,");
                        strSql2.Append("'" + model.CreatUser + "',");
                    }
                    if (model.CreatDateTime != null)
                    {
                        strSql1.Append("CreatDateTime,");
                        strSql2.Append("'" + model.CreatDateTime + "',");
                    }
                    if (model.UpdateUser != null)
                    {
                        strSql1.Append("UpdateUser,");
                        strSql2.Append("'" + model.UpdateUser + "',");
                    }
                    if (model.UpdateDateTime != null)
                    {
                        strSql1.Append("UpdateDateTime,");
                        strSql2.Append("'" + model.UpdateDateTime + "',");
                    }
                    if (model.TaskCode != null)
                    {
                        strSql1.Append("TaskCode,");
                        strSql2.Append("'" + model.TaskCode + "',");
                    }
                    strSql.Append(" insert into TB_StandardSolutionCheck(");
                    strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                    strSql.Append(")");
                    strSql.Append(" values (");
                    strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                    strSql.Append("); ");
                }
                g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                //int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection));
                //if (rowsAffected > 0)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">质控任务实体</param>
        /// <returns></returns>
        public bool Update(StandardSolutionCheckEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" update TB_StandardSolutionCheck set ");
                if (model.StandardValue != null)
                {
                    strSql.Append("StandardValue=" + model.StandardValue + ",");
                }
                else
                {
                    strSql.Append("StandardValue= null ,");
                }
                if (model.PollutantValue != null)
                {
                    strSql.Append("PollutantValue=" + model.PollutantValue + ",");
                }
                else
                {
                    strSql.Append("PollutantValue= null ,");
                }
                if (model.AddValue != null)
                {
                    strSql.Append("AddValue=" + model.AddValue + ",");
                }
                else
                {
                    strSql.Append("AddValue= null ,");
                }
                if (model.PollutantValueAdd != null)
                {
                    strSql.Append("PollutantValueAdd=" + model.PollutantValueAdd + ",");
                }
                else
                {
                    strSql.Append("PollutantValueAdd= null ,");
                }
                if (model.RelativeOffset != null)
                {
                    strSql.Append("RelativeOffset=" + model.RelativeOffset + ",");
                }
                else
                {
                    strSql.Append("RelativeOffset= null ,");
                }
                if (model.AbsoluteOffset != null)
                {
                    strSql.Append("AbsoluteOffset=" + model.AbsoluteOffset + ",");
                }
                else
                {
                    strSql.Append("AbsoluteOffset= null ,");
                }
                if (model.OffsetValue != null)
                {
                    strSql.Append("OffsetValue=" + model.OffsetValue + ",");
                }
                else
                {
                    strSql.Append("OffsetValue= null ,");
                }
                if (model.Evaluate != null)
                {
                    strSql.Append("Evaluate='" + model.Evaluate + "',");
                }
                else
                {
                    strSql.Append("Evaluate= null ,");
                }
                if (model.TaskCode != null)
                {
                    strSql.Append("TaskCode='" + model.TaskCode + "',");
                }
                else
                {
                    strSql.Append("TaskCode= null ,");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.AppendFormat(" where ActionID='{0}' and PointId='{1}' and PollutantCode='{2}' and Tstamp='{3}' and SampleNumber='{4}'; ",
                                    model.ActionID, model.PointId, model.PollutantCode, model.Tstamp, model.SampleNumber);
                g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                //int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection));
                //if (rowsAffected > 0)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="actionID">工作ID</param>
        /// <param name="pointId">测点Id</param>
        /// <param name="pollutantCode">因子代码</param>
        /// <param name="tstamp">时间戳</param>
        /// <returns></returns>
        public bool Delete(string actionID, string pointId, string pollutantCode, DateTime tstamp, string SampleNumber)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from TB_StandardSolutionCheck ");
                strSql.AppendFormat(" where ActionID='{0}' and PointId='{1}' and PollutantCode='{2}' and Tstamp='{3}' and SampleNumber='{4}'",
                    actionID, pointId, pollutantCode, tstamp, SampleNumber);
                g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                //int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection));
                //if (rowsAffected > 0)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 取得主表的虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string missionId, DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            recordTotal = 0;
            try
            {
                //string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), ",");
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string dateStr = string.Empty;
                string keyName = "PointId";
                string fieldName = "*";
                string where = string.Empty;//查询条件拼接

                //if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
                //{
                //    idsStr = " AND id =" + idsStr;
                //}
                //else if (!string.IsNullOrEmpty(idsStr))
                //{
                //    idsStr = " AND id IN(" + idsStr + ")";
                //}
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (!string.IsNullOrEmpty(missionId))
                {
                    missionId = " AND MissionId ='" + missionId + "'";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "Tstamp" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2}", portIdsStr, missionId, dateStr);
                return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据仪器类型取得主表的虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagers(string[] portIds, string taskCode, string missionId, string Type, DateTime dtmStart, DateTime dtmEnd,
        int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            recordTotal = 0;
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string type = Type;
                string dateStr = string.Empty;
                string keyName = "PointId";
                string fieldName = "*";
                string where = string.Empty;//查询条件拼接

                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (!string.IsNullOrEmpty(type))
                {
                    type = " AND InstrumentName ='" + type + "'";
                }
                if (!string.IsNullOrEmpty(taskCode))
                {
                    taskCode = " AND TaskCode ='" + taskCode + "'";
                }
                if (!string.IsNullOrEmpty(missionId))
                {
                    missionId = " AND MissionId ='" + missionId + "'";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "Tstamp" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2} {3} {4}", portIdsStr, taskCode, missionId, type, dateStr);
                return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 质控任务取得主表的虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagerQControl(string[] portIds, string[] missionIds, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string[] evaluate,
        int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            recordTotal = 0;
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string missionIdsStr = "";
                //string pollutantCodeStr = StringExtensions.GetArrayStrNoEmpty(pollutantCodes.ToList<string>(), ",");
                string pollutantCodeStr = "";
                for (int i = 0; i < missionIds.Length; i++)
                {
                    missionIdsStr += "'" + missionIds[i] + "'";
                    if (i != missionIds.Length - 1)
                    {
                        missionIdsStr += ",";
                    }

                }
                for (int i = 0; i < pollutantCodes.Length; i++)
                {
                    pollutantCodeStr += "'" + pollutantCodes[i] + "'";
                    if (i != pollutantCodes.Length - 1)
                    {
                        pollutantCodeStr += ",";
                    }

                }
                string dateStr = string.Empty;
                string keyName = "PointId";
                string fieldName = "*";
                string where = string.Empty;//查询条件拼接
                string evaluateStr = string.Empty;
                if (evaluate != null && evaluate.Length > 0 && evaluate[0].ToString() != "")
                    evaluateStr = " AND Evaluate IN ('" + StringExtensions.GetArrayStrNoEmpty(evaluate.ToList(), "','") + "')";
           

                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (missionIds.Length == 1 && !string.IsNullOrEmpty(missionIds[0]))
                {
                    missionIdsStr = " AND MissionId =" + missionIdsStr;
                }
                else if (!string.IsNullOrEmpty(missionIdsStr))
                {
                    missionIdsStr = " AND MissionId IN(" + missionIdsStr + ")";
                }
                if (pollutantCodes.Length == 1 && !string.IsNullOrEmpty(pollutantCodes[0]))
                {
                    pollutantCodeStr = " AND PollutantCode =" + pollutantCodeStr + "";
                }
                else if (!string.IsNullOrEmpty(pollutantCodeStr))
                {
                    pollutantCodeStr = " AND PollutantCode IN(" + pollutantCodeStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "Tstamp" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2} {3} {4}", portIdsStr, missionIdsStr, pollutantCodeStr, evaluateStr, dateStr);
                return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 质控任务取得主表的虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagerQControl(string[] portIds, string missionId, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string[] evaluate,
        int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            recordTotal = 0;
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string missionIdsStr = "";
                //string pollutantCodeStr = StringExtensions.GetArrayStrNoEmpty(pollutantCodes.ToList<string>(), ",");
                string pollutantCodeStr = "";
                for (int i = 0; i < pollutantCodes.Length; i++)
                {
                    pollutantCodeStr += "'" + pollutantCodes[i] + "'";
                    if (i != pollutantCodes.Length - 1)
                    {
                        pollutantCodeStr += ",";
                    }

                }
                string dateStr = string.Empty;
                string keyName = "PointId";
                string fieldName = "*";
                string where = string.Empty;//查询条件拼接
                //string taskcodeStr = string.Empty;
                string evaluateStr = string.Empty;
                if (evaluate != null && evaluate.Length > 0 && evaluate[0].ToString() != "")
                    evaluateStr = " AND Evaluate IN ('" + StringExtensions.GetArrayStrNoEmpty(evaluate.ToList(), "','") + "')";
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId ='" + portIdsStr + "'";
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                missionIdsStr = " AND MissionId ='" + missionId + "'";
                if (pollutantCodes.Length == 1 && !string.IsNullOrEmpty(pollutantCodes[0]))
                {
                    pollutantCodeStr = " AND PollutantCode ='" + pollutantCodeStr + "'";
                }
                else if (!string.IsNullOrEmpty(pollutantCodeStr))
                {
                    pollutantCodeStr = " AND PollutantCode IN(" + pollutantCodeStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                //taskcodeStr = " AND TaskCode IN(SELECT TaskCode FROM [EQMS_Framework].[dbo].[TB_OMMP_TaskDetail] as t LEFT JOIN [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] as m ON t.MainGuid=m.RowGuid) ";
                orderBy = string.IsNullOrEmpty(orderBy) ? "Tstamp" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2} {3} {4} ", portIdsStr, missionIdsStr, pollutantCodeStr, evaluateStr, dateStr);
                return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 质控任务取得主表的虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagerQControlNew(string[] portIds, string missionId, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string[] evaluate,
        int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            recordTotal = 0;
            try
            {
                string sql = "select ss.*,b.ObjectName from [AMS_MonitorBusiness].[dbo].[TB_StandardSolutionCheck] ss join (select td.*,c.ObjectName from [EQMS_Framework].[dbo].[TB_OMMP_TaskDetail] td join (select tm.RowGuid,mo.ObjectName from [EQMS_Framework].[dbo].[TB_OMMP_TaskMain] tm join [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] mo on tm.MaintenanceObjectGuid=mo.RowGuid) c on td.MainGuid=c.RowGuid ) b on ss.TaskCode=b.TaskCode";
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string missionIdsStr = "";
                //string pollutantCodeStr = StringExtensions.GetArrayStrNoEmpty(pollutantCodes.ToList<string>(), ",");
                string pollutantCodeStr = "";
                for (int i = 0; i < pollutantCodes.Length; i++)
                {
                    pollutantCodeStr += "'" + pollutantCodes[i] + "'";
                    if (i != pollutantCodes.Length - 1)
                    {
                        pollutantCodeStr += ",";
                    }

                }
                string dateStr = string.Empty;
                //string keyName = "PointId";
                //string fieldName = "*";
                string where = string.Empty;//查询条件拼接
                //string taskcodeStr = string.Empty;
                string evaluateStr = string.Empty;
                if (evaluate != null && evaluate.Length > 0 && evaluate[0].ToString() != "")
                {
                    //evaluateStr = " AND Evaluate IN ('" + StringExtensions.GetArrayStrNoEmpty(evaluate.ToList(), "','") + "')";
                    evaluateStr = " AND (1=0 ";
                    for (int i = 0; i < evaluate.Length; i++)
                    {
                        if ("NULL".Equals(evaluate[i]))
                        {
                            evaluateStr += "or Evaluate is null"; 
                        }
                        else
                        {
                            evaluateStr += "or Evaluate='"+evaluate[i]+"'"; 
                        }
                    }
                    evaluateStr += ")";
                }
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId ='" + portIdsStr + "'";
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                missionIdsStr = " AND MissionId ='" + missionId + "'";
                if (pollutantCodes.Length == 1 && !string.IsNullOrEmpty(pollutantCodes[0]))
                {
                    pollutantCodeStr = " AND PollutantCode ='" + pollutantCodeStr + "'";
                }
                else if (!string.IsNullOrEmpty(pollutantCodeStr))
                {
                    pollutantCodeStr = " AND PollutantCode IN(" + pollutantCodeStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                ////taskcodeStr = " AND TaskCode IN(SELECT TaskCode FROM [EQMS_Framework].[dbo].[TB_OMMP_TaskDetail] as t LEFT JOIN [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] as m ON t.MainGuid=m.RowGuid) ";
                //orderBy = string.IsNullOrEmpty(orderBy) ? "Tstamp" : orderBy;
                //where = string.Format(" 1=1 {0} {1} {2} {3} {4} {5}", portIdsStr, missionIdsStr, pollutantCodeStr, evaluateStr, dateStr, taskcodeStr);
                //return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
                sql += portIdsStr;
                sql += missionIdsStr;
                sql += pollutantCodeStr;
                sql += evaluateStr;
                sql += dateStr;
                sql += " order by Tstamp";
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 质控任务取得主表的虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagerQControlNew2(string[] portIds, string missionId, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd,
        int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            recordTotal = 0;
            try
            {
                string sql = "select ss.*,b.ObjectName from [AMS_MonitorBusiness].[dbo].[TB_StandardSolutionCheck] ss join (select td.*,c.ObjectName from [EQMS_Framework].[dbo].[TB_OMMP_TaskDetail] td join (select tm.RowGuid,mo.ObjectName from [EQMS_Framework].[dbo].[TB_OMMP_TaskMain] tm join [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] mo on tm.MaintenanceObjectGuid=mo.RowGuid) c on td.MainGuid=c.RowGuid ) b on ss.TaskCode=b.TaskCode";
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string missionIdsStr = "";
                //string pollutantCodeStr = StringExtensions.GetArrayStrNoEmpty(pollutantCodes.ToList<string>(), ",");
                string pollutantCodeStr = "";
                for (int i = 0; i < pollutantCodes.Length; i++)
                {
                    pollutantCodeStr += "'" + pollutantCodes[i] + "'";
                    if (i != pollutantCodes.Length - 1)
                    {
                        pollutantCodeStr += ",";
                    }

                }
                string dateStr = string.Empty;
                //string keyName = "PointId";
                //string fieldName = "*";
                string where = string.Empty;//查询条件拼接
                //string taskcodeStr = string.Empty;
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId ='" + portIdsStr + "'";
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                missionIdsStr = " AND MissionId ='" + missionId + "'";
                if (pollutantCodes.Length == 1 && !string.IsNullOrEmpty(pollutantCodes[0]))
                {
                    pollutantCodeStr = " AND PollutantCode ='" + pollutantCodeStr + "'";
                }
                else if (!string.IsNullOrEmpty(pollutantCodeStr))
                {
                    pollutantCodeStr = " AND PollutantCode IN(" + pollutantCodeStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                ////taskcodeStr = " AND TaskCode IN(SELECT TaskCode FROM [EQMS_Framework].[dbo].[TB_OMMP_TaskDetail] as t LEFT JOIN [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] as m ON t.MainGuid=m.RowGuid) ";
                //orderBy = string.IsNullOrEmpty(orderBy) ? "Tstamp" : orderBy;
                //where = string.Format(" 1=1 {0} {1} {2} {3} {4} {5}", portIdsStr, missionIdsStr, pollutantCodeStr, evaluateStr, dateStr, taskcodeStr);
                //return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
                sql += portIdsStr;
                sql += missionIdsStr;
                sql += pollutantCodeStr;
                sql += dateStr;
                sql += " order by Tstamp";
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetDecimalDigit(string PollutantName)
        {
            string sql = "select DecimalDigit from [AMS_BaseData].[Standard].[TB_PollutantCode] where PollutantName='" + PollutantName+"' and DecimalDigit is not null";
            DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
            if (dv.Count > 0)
            {
              return dv[0].Row["DecimalDigit"].ToString();
            }
            return null;
        }

        /// <summary>
        /// 获取导出Excel的性能考核虚拟分页数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="missionIds"></param>
        /// <param name="pollutantCodes"></param>
        /// <param name="dtmStart"></param>
        /// <param name="dtmEnd"></param>
        /// <param name="actionID"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataToExcel(string[] portIds, string[] missionIds, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd,
         string orderBy = "TaskCode,ActionID")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string missionIdsStr = "";
                //string pollutantCodeStr = StringExtensions.GetArrayStrNoEmpty(pollutantCodes.ToList<string>(), ",");
                string pollutantCodeStr = "";
                for (int i = 0; i < missionIds.Length; i++)
                {
                    missionIdsStr += "'" + missionIds[i] + "'";
                    if (i != missionIds.Length - 1)
                    {
                        missionIdsStr += ",";
                    }

                }
                for (int i = 0; i < pollutantCodes.Length; i++)
                {
                    pollutantCodeStr += "'" + pollutantCodes[i] + "'";
                    if (i != pollutantCodes.Length - 1)
                    {
                        pollutantCodeStr += ",";
                    }

                }
                string dateStr = string.Empty;
                string keyName = "PointId";
                string fieldName = "ActionID as 质控类型,Tstamp as 执行时间,PollutantName as 因子,PollutantValue as 标准溶液浓度,StandardValue as 测定因子值,UniversalValue1 as 平均值,UniversalValue2 as 标准偏差,UniversalValue3 as 精密度,UniversalValue4 as 准确度 ";
                string where = string.Empty;//查询条件拼接
                string ActionID = string.Empty;
                //ActionID = " AND ActionID = '"+actionID+"'";
                //for (int i = 0; i < actionID.Length; i++)
                //{
                //    ActionID = " AND ActionID = '" + actionID[i] + "'";
                //}
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (missionIds.Length == 1 && !string.IsNullOrEmpty(missionIds[0]))
                {
                    missionIdsStr = " AND MissionID =" + missionIdsStr;
                }
                else if (!string.IsNullOrEmpty(missionIdsStr))
                {
                    missionIdsStr = " AND MissionID IN(" + missionIdsStr + ")";
                }
                if (pollutantCodes.Length == 1 && !string.IsNullOrEmpty(pollutantCodes[0]))
                {
                    pollutantCodeStr = " AND PollutantCode =" + pollutantCodeStr + "";
                }
                else if (!string.IsNullOrEmpty(pollutantCodeStr))
                {
                    pollutantCodeStr = " AND PollutantCode IN(" + pollutantCodeStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "TaskCode,ActionID" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2} {3}", portIdsStr, missionIdsStr, pollutantCodeStr, dateStr);
                sqlStringBuilder.AppendFormat(@"SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", fieldName, tableName, where, orderBy);
                return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
                //return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得主表导出数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, string orderBy = "Tstamp")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                //string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), ",");
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string dateStr = string.Empty;
                //string keyName = "id";
                string fieldName = "*";
                string where = string.Empty;//查询条件拼接

                //if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
                //{
                //    idsStr = " AND id =" + idsStr;
                //}
                //else if (!string.IsNullOrEmpty(idsStr))
                //{
                //    idsStr = " AND id IN(" + idsStr + ")";
                //}
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "Tstamp" : orderBy;
                where = string.Format(" 1=1 {0} {1} ", portIdsStr, dateStr);
                sqlStringBuilder.AppendFormat(@"SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", fieldName, tableName, where, orderBy);
                return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取每个ActionID的数量
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="missionIds"></param>
        /// <param name="actionID"></param>
        /// <param name="pollutantCodes"></param>
        /// <param name="dtmStart"></param>
        /// <param name="dtmEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetNum(string[] portIds, string[] missionIds, string[] actionIDs, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string orderBy = "TaskCode,ActionID")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string missionIdsStr = "";
                //string pollutantCodeStr = StringExtensions.GetArrayStrNoEmpty(pollutantCodes.ToList<string>(), ",");
                string pollutantCodeStr = "";
                string ActionIDStr = "";
                for (int i = 0; i < missionIds.Length; i++)
                {
                    missionIdsStr += "'" + missionIds[i] + "'";
                    if (i != missionIds.Length - 1)
                    {
                        missionIdsStr += ",";
                    }

                }
                for (int i = 0; i < pollutantCodes.Length; i++)
                {
                    pollutantCodeStr += "'" + pollutantCodes[i] + "'";
                    if (i != pollutantCodes.Length - 1)
                    {
                        pollutantCodeStr += ",";
                    }

                }
                for (int i = 0; i < actionIDs.Length; i++)
                {
                    ActionIDStr += "'" + actionIDs[i] + "'";
                    if (i != actionIDs.Length - 1)
                    {
                        ActionIDStr += ",";
                    }

                }
                string dateStr = string.Empty;
                string keyName = "PointId";
                string fieldName = "count(*),ActionID,TaskCode";
                string where = string.Empty;//查询条件拼接
                
                //ActionID = " AND ActionID = '"+actionID+"'";
                //for (int i = 0; i < actionID.Length; i++)
                //{
                //    ActionID = " AND ActionID = '" + actionID[i] + "'";
                //}
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (missionIds.Length == 1 && !string.IsNullOrEmpty(missionIds[0]))
                {
                    missionIdsStr = " AND MissionID =" + missionIdsStr;
                }
                else if (!string.IsNullOrEmpty(missionIdsStr))
                {
                    missionIdsStr = " AND MissionID IN(" + missionIdsStr + ")";
                }
                if (pollutantCodes.Length == 1 && !string.IsNullOrEmpty(pollutantCodes[0]))
                {
                    pollutantCodeStr = " AND PollutantCode =" + pollutantCodeStr + "";
                }
                else if (!string.IsNullOrEmpty(pollutantCodeStr))
                {
                    pollutantCodeStr = " AND PollutantCode IN(" + pollutantCodeStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  Tstamp<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                ActionIDStr = " AND ActionID IN(" + ActionIDStr + ")";
                orderBy = string.IsNullOrEmpty(orderBy) ? "TaskCode,ActionID" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2} {3} {4}", portIdsStr, missionIdsStr, pollutantCodeStr, dateStr, ActionIDStr);
                sqlStringBuilder.AppendFormat(@"SELECT {0} FROM {1} WHERE {2} GROUP BY TaskCode,ActionID ORDER BY {3} ", fieldName, tableName, where, orderBy);
                return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
                //return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select ss.*,b.ObjectName from [AMS_MonitorBusiness].[dbo].[TB_StandardSolutionCheck] ss join (select td.*,c.ObjectName from [EQMS_Framework].[dbo].[TB_OMMP_TaskDetail] td join (select tm.RowGuid,mo.ObjectName from [EQMS_Framework].[dbo].[TB_OMMP_TaskMain] tm join [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] mo on tm.MaintenanceObjectGuid=mo.RowGuid) c on td.MainGuid=c.RowGuid ) b on ss.TaskCode=b.TaskCode ");
           
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
                strSql.Append(" order by Tstamp");
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetListNew(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select ss.*,b.ObjectName from [AMS_MonitorBusiness].[dbo].[TB_StandardSolutionCheck] ss join (select td.*,c.ObjectName from [EQMS_Framework].[dbo].[TB_OMMP_TaskDetail] td join (select tm.RowGuid,mo.ObjectName from [EQMS_Framework].[dbo].[TB_OMMP_TaskMain] tm join [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] mo on tm.MaintenanceObjectGuid=mo.RowGuid) c on td.MainGuid=c.RowGuid ) b on ss.TaskCode=b.TaskCode ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }


        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetLists(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select a.id [Sid],b.id [Rid],a.PollutantCode,PollutantName,d.MeasureUnitName Unit,a.PollutantValue,a.SampleNumber,a.PointName,a.Tstamp,a.Goal,a.BatchNumber,a.Position,a.InstrumentNum,b.Description,a.Tester,b.CompareItemName,b.TestValue,a.TestType SampleType,b.TestType RealSampleType 
                            ,b.RelativeOffset,b.AbsoluteOffset,c.CompareLimitValue,b.Evaluate
                            from TB_StandardSolutionCheck a
                            left join TB_RealSamples b on a.TaskCode=b.TaskCode and a.PollutantName=b.CompareItemName
                            left join [AMS_BaseData].[dbo].[TB_CompareConfig] c
                            on c.[CompareItem]=b.CompareItem
                            left join [AMS_BaseData].[dbo].[V_Factor_Water_SiteMap] d
                            on d.PID=a.PollutantCode
                            and isnull(a.TestType,'1')!=3 "
                          );
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(@"union
                            select b.id [Sid],a.id [Rid],b.PollutantCode,PollutantName,d.MeasureUnitName Unit,b.PollutantValue,a.SampleNumber,a.PointName,b.Tstamp,b.Goal,b.BatchNumber,b.Position,b.InstrumentNum,a.Description,b.Tester,a.CompareItemName,a.TestValue,b.TestType SampleType,a.TestType RealSampleType 
                            ,a.RelativeOffset,a.AbsoluteOffset,c.CompareLimitValue,a.Evaluate
                            from TB_RealSamples a
                            left join TB_StandardSolutionCheck b on a.TaskCode=b.TaskCode and b.PollutantName=a.CompareItemName
                            left join [AMS_BaseData].[dbo].[TB_CompareConfig] c
                            on c.[CompareItem]=a.CompareItem
                            left join [AMS_BaseData].[dbo].[V_Factor_Water_SiteMap] d
                            on d.PID=b.PollutantCode
                            and isnull(b.TestType,'1')!=3
                          ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        /// <summary>
        /// 根据因子code查看比对限值
        /// </summary>
        /// <param name="PollutantCode">因子code</param>
        /// <returns></returns>
        public string GetCompareLimitValue(string pollutantCode)
        {
            try
            {
                string sql = string.Format(@"  select [CompareLimitValue] from [AMS_BaseData].[dbo].[TB_CompareConfig]
                                                where [CompareItem]='{0}'", pollutantCode);
                return g_DatabaseHelper.ExecuteScalar(sql, connection).ToString();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public string GetRid(string Sid)
        {
            try
            {
                string sql = string.Format(@"SELECT TOP 1 point.ObjectName
                                            FROM [EQMS_Framework].[dbo].[TB_OMMP_InstrumentInstanceSite] state
                                            left join [EQMS_Framework].[dbo].[TB_OMMP_InstrumentInstance] ins
                                            on state.[InstanceGuid]=ins.[RowGuid]
                                            left join [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] point
                                            on ins.SiteGuid=point.RowGuid
                                            where [InstanceGuid]='{0}'
                                            order by state.[ID] desc", Sid);
                return g_DatabaseHelper.ExecuteScalar(sql, connection).ToString();

            }
            catch (Exception ex) { throw ex; }
        }


        #endregion  Method
    }
}
