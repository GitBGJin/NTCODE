using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Utilities.AdoData;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using System.Data;
using System.Data.SqlClient;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.DomainModel.MonitoringBusiness;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：SamplingRecordDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 采样记录详情数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SamplingRecordDetailDAL
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
        /// 数据库表名（主表）
        /// </summary>
        private string tableNameMain = "dbo.TB_SamplingRecord";

        /// <summary>
        /// 数据库表名（详情表）
        /// </summary>
        private string tableNameDetail = "dbo.TB_SamplingRecordDetail";
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_RealSamples";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public SamplingRecordDetailDAL()
        {
            //connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Hour);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">采样记录详情实体</param>
        /// <returns></returns>
        public int Add(params SamplingRecordDetailEntity[] models)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                foreach (SamplingRecordDetailEntity model in models)
                {
                    StringBuilder strSql1 = new StringBuilder();
                    StringBuilder strSql2 = new StringBuilder();
                    if (model.id != null)
                    {
                        strSql1.Append("id,");
                        strSql2.Append("'" + Guid.NewGuid().ToString() + "',");
                    }
                    if (model.SamplingMainGuid != null)
                    {
                        strSql1.Append("SamplingMainGuid,");
                        strSql2.Append("'" + model.SamplingMainGuid + "',");
                    }
                    if (model.PointId != null)
                    {
                        strSql1.Append("PointId,");
                        strSql2.Append("'" + model.PointId + "',");
                    }
                    if (model.PointName != null)
                    {
                        strSql1.Append("PointName,");
                        strSql2.Append("'" + model.PointName + "',");
                    }
                    if (model.SamplingUser != null)
                    {
                        strSql1.Append("SamplingUser,");
                        strSql2.Append("'" + model.SamplingUser + "',");
                    }
                    if (model.SampleNumber != null)
                    {
                        strSql1.Append("SampleNumber,");
                        strSql2.Append("'" + model.SampleNumber + "',");
                    }
                    if (model.SamplingTime != null)
                    {
                        strSql1.Append("SamplingTime,");
                        strSql2.Append("'" + model.SamplingTime + "',");
                    }
                    if (model.SamplingPosition != null)
                    {
                        strSql1.Append("SamplingPosition,");
                        strSql2.Append("'" + model.SamplingPosition + "',");
                    }
                    if (model.InstrumentNumber != null)
                    {
                        strSql1.Append("InstrumentNumber,");
                        strSql2.Append("'" + model.InstrumentNumber + "',");
                    }
                    if (model.PollutantCode != null)
                    {
                        strSql1.Append("PollutantCode,");
                        strSql2.Append("'" + model.PollutantCode + "',");
                    }
                    if (model.PollutantValue != null)
                    {
                        strSql1.Append("PollutantValue,");
                        strSql2.Append("" + model.PollutantValue + ",");
                    }
                    if (model.SamplingType != null)
                    {
                        strSql1.Append("SamplingType,");
                        strSql2.Append("'" + model.SamplingType + "',");
                    }
                    if (model.ComparisonAnalysisProject != null)
                    {
                        strSql1.Append("ComparisonAnalysisProject,");
                        strSql2.Append("'" + model.ComparisonAnalysisProject + "',");
                    }
                    if (model.Remark != null)
                    {
                        strSql1.Append("Remark,");
                        strSql2.Append("'" + model.Remark + "',");
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
                    strSql.Append(" insert into TB_SamplingRecordDetail(");
                    strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                    strSql.Append(")");
                    strSql.Append(" values (");
                    strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                    strSql.Append("); ");
                }
                strSql.Append(";select @@ROWCOUNT");
                //g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">采样记录详情实体</param>
        /// <returns></returns>
        public int Update(SamplingRecordDetailEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_SamplingRecordDetail set ");
                if (model.SamplingMainGuid != null)
                {
                    strSql.Append("SamplingMainGuid='" + model.SamplingMainGuid + "',");
                }
                else
                {
                    strSql.Append("SamplingMainGuid= null ,");
                }
                if (model.SampleNumber != null)
                {
                    strSql.Append("SampleNumber='" + model.SampleNumber + "',");
                }
                else
                {
                    strSql.Append("SampleNumber= null ,");
                }
                if (model.PointId != null)
                {
                    strSql.Append("PointId='" + model.PointId + "',");
                }
                else
                {
                    strSql.Append("PointId= null ,");
                }
                if (model.PointName != null)
                {
                    strSql.Append("PointName='" + model.PointName + "',");
                }
                else
                {
                    strSql.Append("PointName= null ,");
                }
                if (model.SamplingUser != null)
                {
                    strSql.Append("SamplingUser='" + model.SamplingUser + "',");
                }
                else
                {
                    strSql.Append("SamplingUser= null ,");
                }
                if (model.SamplingTime != null)
                {
                    strSql.Append("SamplingTime='" + model.SamplingTime + "',");
                }
                else
                {
                    strSql.Append("SamplingTime= null ,");
                }
                if (model.SamplingPosition != null)
                {
                    strSql.Append("SamplingPosition='" + model.SamplingPosition + "',");
                }
                else
                {
                    strSql.Append("SamplingPosition= null ,");
                }
                if (model.InstrumentNumber != null)
                {
                    strSql.Append("InstrumentNumber='" + model.InstrumentNumber + "',");
                }
                else
                {
                    strSql.Append("InstrumentNumber= null ,");
                }
                if (model.PollutantCode != null)
                {
                    strSql.Append("PollutantCode='" + model.PollutantCode + "',");
                }
                else
                {
                    strSql.Append("PollutantCode= null ,");
                }
                if (model.PollutantValue != null)
                {
                    strSql.Append("PollutantValue=" + model.PollutantValue + ",");
                }
                else
                {
                    strSql.Append("PollutantValue= null ,");
                }
                if (model.SamplingType != null)
                {
                    strSql.Append("SamplingType='" + model.SamplingType + "',");
                }
                else
                {
                    strSql.Append("SamplingType= null ,");
                }
                if (model.ComparisonAnalysisProject != null)
                {
                    strSql.Append("ComparisonAnalysisProject='" + model.ComparisonAnalysisProject + "',");
                }
                else
                {
                    strSql.Append("ComparisonAnalysisProject= null ,");
                }
                if (model.Remark != null)
                {
                    strSql.Append("Remark='" + model.Remark + "',");
                }
                else
                {
                    strSql.Append("Remark= null ,");
                }
                if (model.CreatUser != null)
                {
                    strSql.Append("CreatUser='" + model.CreatUser + "',");
                }
                else
                {
                    strSql.Append("CreatUser= null ,");
                }
                if (model.CreatDateTime != null)
                {
                    strSql.Append("CreatDateTime='" + model.CreatDateTime + "',");
                }
                else
                {
                    strSql.Append("CreatDateTime= null ,");
                }
                if (model.UpdateUser != null)
                {
                    strSql.Append("UpdateUser='" + model.UpdateUser + "',");
                }
                else
                {
                    strSql.Append("UpdateUser= null ,");
                }
                if (model.UpdateDateTime != null)
                {
                    strSql.Append("UpdateDateTime='" + model.UpdateDateTime + "',");
                }
                else
                {
                    strSql.Append("UpdateDateTime= null ,");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where id='" + model.id + "' ");
                strSql.Append(";select @@ROWCOUNT");
                //g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">采样记录详情实体</param>
        /// <returns></returns>
        public int Update(RealSampleEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_RealSamples set ");

                if (model.SampleNumber != null)
                {
                    strSql.Append("SampleNumber='" + model.SampleNumber + "',");
                }
                else
                {
                    strSql.Append("SampleNumber= null ,");
                }
                if (model.PointId != null)
                {
                    strSql.Append("PointId='" + model.PointId + "',");
                }
                else
                {
                    strSql.Append("PointId= null ,");
                }
                if (model.PointName != null)
                {
                    strSql.Append("PointName='" + model.PointName + "',");
                }
                else
                {
                    strSql.Append("PointName= null ,");
                }
                if (model.CompareItem != null)
                {
                    strSql.Append("CompareItem='" + model.CompareItem + "',");
                }
                else
                {
                    strSql.Append("CompareItem= null ,");
                }
                if (model.CompareItemName != null)
                {
                    strSql.Append("CompareItemName='" + model.CompareItemName + "',");
                }
                else
                {
                    strSql.Append("CompareItemName= null ,");
                }
                if (model.TestValue != null)
                {
                    strSql.Append("TestValue=" + model.TestValue + ",");
                }
                else
                {
                    strSql.Append("TestValue= null ,");
                }
                if (model.LabValue != null)
                {
                    strSql.Append("LabValue=" + model.LabValue + ",");
                }
                else
                {
                    strSql.Append("LabValue= null ,");
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
                if (model.CompareLimitValue != null)
                {
                    strSql.Append("CompareLimitValue=" + model.CompareLimitValue + ",");
                }
                else
                {
                    strSql.Append("CompareLimitValue= null ,");
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
                if (model.Limit != null)
                {
                    strSql.Append("Limit='" + model.Limit + "',");
                }
                else
                {
                    strSql.Append("Limit= null ,");
                }
                if (model.Datetime != null)
                {
                    strSql.Append("Datetime='" + model.Datetime + "',");
                }
                else
                {
                    strSql.Append("Datetime= null ,");
                }
                if (model.Tester != null)
                {
                    strSql.Append("Tester='" + model.Tester + "',");
                }
                else
                {
                    strSql.Append("Tester= null ,");
                }
                if (model.OrderByNum != null)
                {
                    strSql.Append("OrderByNum=" + model.OrderByNum + ",");
                }
                else
                {
                    strSql.Append("OrderByNum= null ,");
                }
                if (model.Description != null)
                {
                    strSql.Append("Description='" + model.Description + "',");
                }
                else
                {
                    strSql.Append("Description= null ,");
                }
                if (model.CreatUser != null)
                {
                    strSql.Append("CreatUser='" + model.CreatUser + "',");
                }
                else
                {
                    strSql.Append("CreatUser= null ,");
                }
                if (model.CreatDateTime != null)
                {
                    strSql.Append("CreatDateTime='" + model.CreatDateTime + "',");
                }
                else
                {
                    strSql.Append("CreatDateTime= null ,");
                }
                if (model.UpdateUser != null)
                {
                    strSql.Append("UpdateUser='" + model.UpdateUser + "',");
                }
                else
                {
                    strSql.Append("UpdateUser= null ,");
                }
                if (model.UpdateDateTime != null)
                {
                    strSql.Append("UpdateDateTime='" + model.UpdateDateTime + "',");
                }
                else
                {
                    strSql.Append("UpdateDateTime= null ,");
                }
                if (model.TaskCode != null)
                {
                    strSql.Append("TaskCode='" + model.TaskCode + "',");
                }
                else
                {
                    strSql.Append("TaskCode= null ,");
                }
                if (model.TestType != null)
                {
                    strSql.Append("TestType='" + model.TestType + "',");
                }
                else
                {
                    strSql.Append("TestType= null ,");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where SampleGuid='" + model.SampleGuid + "' ");
                strSql.Append(";select @@ROWCOUNT");
                //g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from TB_SamplingRecordDetail ");
                strSql.Append(" where id='" + id + "' ");
                strSql.Append(";select @@ROWCOUNT");
                strSql.Append(" delete from TB_RealSamples ");
                strSql.Append(" where SampleGuid='" + id + "' ");
                strSql.Append(";select @@ROWCOUNT");
                //g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="idlist">id数组</param>
        /// <returns></returns>
        public int DeleteList(string[] idlist)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                string sqlId = "'" + idlist.Aggregate((t, m) => t + "','" + m) + "'";
                strSql.Append("delete from TB_SamplingRecordDetail ");
                strSql.Append(" where id in (" + sqlId + ")  ");
                strSql.Append(";select @@ROWCOUNT");
                //g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public SamplingRecordDetailEntity GetModel(Guid id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select  top 1  ");
            strSql.Append(" id,SamplingMainGuid,PointId,PointName,SamplingUser,SampleNumber,SamplingTime,SamplingPosition,InstrumentNumber,PollutantCode,PollutantValue,SamplingType,ComparisonAnalysisProject,Remark,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime ");
            strSql.Append(" from TB_SamplingRecordDetail ");
            strSql.Append(" where SamplingMainGuid='" + id + "' ");
            SamplingRecordDetailEntity model = new SamplingRecordDetailEntity();
            DataTable dt = g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
            if (dt.Rows.Count > 0)
            {
                return DataRowToModel(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SamplingRecordDetailEntity DataRowToModel(DataRow row)
        {
            SamplingRecordDetailEntity model = new SamplingRecordDetailEntity();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = new Guid(row["id"].ToString());
                }
                if (row["SamplingMainGuid"] != null && row["SamplingMainGuid"].ToString() != "")
                {
                    model.SamplingMainGuid = new Guid(row["SamplingMainGuid"].ToString());
                }
                if (row["SampleNumber"] != null)
                {
                    model.SampleNumber = row["SampleNumber"].ToString();
                }
                if (row["PointId"] != null)
                {
                    model.PointId = Convert.ToInt32(row["PointId"]);
                }
                if (row["PointName"] != null)
                {
                    model.PointName = row["PointName"].ToString();
                }
                if (row["SamplingUser"] != null)
                {
                    model.SamplingUser = row["SamplingUser"].ToString();
                }
                if (row["SamplingTime"] != null && row["SamplingTime"].ToString() != "")
                {
                    model.SamplingTime = DateTime.Parse(row["SamplingTime"].ToString());
                }
                if (row["SamplingPosition"] != null)
                {
                    model.SamplingPosition = row["SamplingPosition"].ToString();
                }
                if (row["InstrumentNumber"] != null)
                {
                    model.InstrumentNumber = row["InstrumentNumber"].ToString();
                }
                if (row["PollutantCode"] != null)
                {
                    model.PollutantCode = row["PollutantCode"].ToString();
                }
                if (row["PollutantValue"] != null && row["PollutantValue"].ToString() != "")
                {
                    model.PollutantValue = decimal.Parse(row["PollutantValue"].ToString());
                }
                if (row["SamplingType"] != null && row["SamplingType"].ToString() != "")
                {
                    model.SamplingType = row["SamplingType"].ToString();
                }
                if (row["ComparisonAnalysisProject"] != null)
                {
                    model.ComparisonAnalysisProject = row["ComparisonAnalysisProject"].ToString();
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row["CreatUser"] != null)
                {
                    model.CreatUser = row["CreatUser"].ToString();
                }
                if (row["CreatDateTime"] != null && row["CreatDateTime"].ToString() != "")
                {
                    model.CreatDateTime = DateTime.Parse(row["CreatDateTime"].ToString());
                }
                if (row["UpdateUser"] != null)
                {
                    model.UpdateUser = row["UpdateUser"].ToString();
                }
                if (row["UpdateDateTime"] != null && row["UpdateDateTime"].ToString() != "")
                {
                    model.UpdateDateTime = DateTime.Parse(row["UpdateDateTime"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 取得详情表的查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="sampleNumbers">样品编号</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] sampleNumbers,
            DateTime dtmStart, DateTime dtmEnd, out int recordTotal, int pageSize = int.MaxValue, int pageNo = 0,
             string orderBy = "SamplingDate")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                //string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), ",");
                //string samplingMainGuidsStr = StringExtensions.GetArrayStrNoEmpty(samplingMainGuids.ToList<string>(), ",");
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string sampleNumbersStr = StringExtensions.GetArrayStrNoEmpty(sampleNumbers.ToList<string>(), "','");
                string dateStr = string.Empty;
                string keyName = "id";
                string fieldName = @"
                                     id,SamplingMainGuid,SampleNumber,SamplingTime,SamplingPosition,InstrumentNumber,PollutantCode,PollutantValue,
                                     SamplingType,ComparisonAnalysisProject,Remark,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime";//ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,
                string where = string.Empty;//查询条件拼接

                //if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
                //{
                //    idsStr = " AND b.id =" + idsStr;
                //}
                //else if (!string.IsNullOrEmpty(idsStr))
                //{
                //    idsStr = " AND b.id IN(" + idsStr + ")";
                //}
                //if (samplingMainGuids.Length == 1 && !string.IsNullOrEmpty(samplingMainGuids[0]))
                //{
                //    samplingMainGuidsStr = " AND SamplingMainGuid =" + samplingMainGuidsStr;
                //}
                //else if (!string.IsNullOrEmpty(samplingMainGuidsStr))
                //{
                //    samplingMainGuidsStr = " AND SamplingMainGuid IN(" + samplingMainGuidsStr + ")";
                //}
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND a.PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND a.PointId IN(" + portIdsStr + ")";
                }
                if (sampleNumbers.Length == 1 && !string.IsNullOrEmpty(sampleNumbers[0]))
                {
                    sampleNumbersStr = " AND SampleNumber ='" + sampleNumbersStr + "'";
                }
                else if (!string.IsNullOrEmpty(sampleNumbersStr))
                {
                    sampleNumbersStr = " AND SampleNumber IN('" + sampleNumbersStr + "')";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  SamplingTime>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  SamplingTime<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "SamplingDate" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2} ", portIdsStr, sampleNumbersStr, dateStr);
                //ROW_NUMBER() OVER(order by ) as rowNum 
                return g_GridViewPager.GetGridViewPager(tableNameDetail, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得详情表的查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="sampleNumbers">样品编号</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetData(Guid id, string orderBy = "SamplingDate")
        {
            try
            {
                int recordTotal = 0;
                int pageSize = 99999;
                int pageNo = 0;
                StringBuilder sqlStringBuilder = new StringBuilder();
                string dateStr = string.Empty;
                string keyName = "id";
                string guidStr = id.ToString();
                string fieldName = @"
                                     id,SamplingMainGuid,PointId,PointName,SamplingUser,SampleNumber,SamplingTime,SamplingPosition,InstrumentNumber,PollutantCode,PollutantValue,
                                     SamplingType,ComparisonAnalysisProject,Remark,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime";//ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,
                string where = string.Empty;//查询条件拼接

                //if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
                //{
                //    idsStr = " AND b.id =" + idsStr;
                //}
                //else if (!string.IsNullOrEmpty(idsStr))
                //{
                //    idsStr = " AND b.id IN(" + idsStr + ")";
                //}
                //if (samplingMainGuids.Length == 1 && !string.IsNullOrEmpty(samplingMainGuids[0]))
                //{
                //    samplingMainGuidsStr = " AND SamplingMainGuid =" + samplingMainGuidsStr;
                //}
                //else if (!string.IsNullOrEmpty(samplingMainGuidsStr))
                //{
                //    samplingMainGuidsStr = " AND SamplingMainGuid IN(" + samplingMainGuidsStr + ")";
                //}
                if (guidStr.IsNotNullOrDBNull())
                {
                    guidStr = " AND SamplingMainGuid ='" + guidStr + "'";
                }
                else if (!string.IsNullOrEmpty(guidStr))
                {
                    guidStr = " AND SamplingMainGuid IN('" + guidStr + "')";
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "SamplingDate" : orderBy;
                where = string.Format(" 1=1 {0}", guidStr);
                //ROW_NUMBER() OVER(order by ) as rowNum 
                return g_GridViewPager.GetGridViewPager(tableNameDetail, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得详情表的查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="sampleNumbers">样品编号</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataNew(Guid id, string orderBy = "SamplingDate")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                string dateStr = string.Empty;
                string guidStr = id.ToString();
                string fieldName = @"a.Id,SamplingMainGuid,a.SampleNumber,SamplingTime,a.PointId,a.PointName,PollutantCode,PollutantValue,b.LabValue,b.RelativeOffset,b.AbsoluteOffset,b.Limit,b.Evaluate,SamplingUser,SamplingPosition,InstrumentNumber,
                                     SamplingType,ComparisonAnalysisProject,a.Remark,b.OffsetValue ";//ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,
                string where = string.Empty;//查询条件拼接
                if (guidStr.IsNotNullOrDBNull())
                {
                    guidStr = " AND SamplingMainGuid ='" + guidStr + "'";
                }
                else if (!string.IsNullOrEmpty(guidStr))
                {
                    guidStr = " AND SamplingMainGuid IN('" + guidStr + "')";
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "SamplingDate" : orderBy;
                where = string.Format(" 1=1 {0}", guidStr);
                sqlStringBuilder.AppendFormat(" SELECT {0} FROM {1} as a left join {2} as b on a.Id=b.SampleGuid  WHERE {3} ORDER BY {4} ", fieldName, tableNameDetail, tableName, where, orderBy);

                return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得详情表导出数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="sampleNumbers">样品编号</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, string[] sampleNumbers,
            DateTime dtmStart, DateTime dtmEnd, string orderBy = "SamplingTime")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                //string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), ",");
                //string samplingMainGuidsStr = StringExtensions.GetArrayStrNoEmpty(samplingMainGuids.ToList<string>(), ",");
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string sampleNumbersStr = StringExtensions.GetArrayStrNoEmpty(sampleNumbers.ToList<string>(), "','");
                string dateStr = string.Empty;
                string fieldName = @"
                                     id,SamplingMainGuid,PointId,PointName,SamplingUser,SampleNumber,SamplingTime,SamplingPosition,InstrumentNumber,PollutantCode,PollutantValue,
                                     SamplingType,ComparisonAnalysisProject,Remark,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime";//ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,
                string where = string.Empty;//查询条件拼接

                //if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
                //{
                //    idsStr = " AND PointId =" + idsStr;
                //}
                //else if (!string.IsNullOrEmpty(idsStr))
                //{
                //    idsStr = " AND PointId IN(" + idsStr + ")";
                //}
                //if (samplingMainGuids.Length == 1 && !string.IsNullOrEmpty(samplingMainGuids[0]))
                //{
                //    samplingMainGuidsStr = " AND SamplingMainGuid =" + samplingMainGuidsStr;
                //}
                //else if (!string.IsNullOrEmpty(samplingMainGuidsStr))
                //{
                //    samplingMainGuidsStr = " AND SamplingMainGuid IN(" + samplingMainGuidsStr + ")";
                //}
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (sampleNumbers.Length == 1 && !string.IsNullOrEmpty(sampleNumbers[0]))
                {
                    sampleNumbersStr = " AND SampleNumber ='" + sampleNumbersStr + "'";
                }
                else if (!string.IsNullOrEmpty(sampleNumbersStr))
                {
                    sampleNumbersStr = " AND SampleNumber IN('" + sampleNumbersStr + "')";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  SamplingTime>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  SamplingTime<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "SamplingDate" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2} ", portIdsStr, sampleNumbersStr, dateStr);
                sqlStringBuilder.AppendFormat(" SELECT {0} FROM {1} WHERE {2} ORDER BY {3} ", fieldName, tableNameDetail, where, orderBy);

                return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得数据总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="sampleNumbers">样品编号</param>
        /// <param name="portIds">测点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public int GetAllDataCount(string[] portIds, string[] sampleNumbers, DateTime dtmStart, DateTime dtmEnd)
        {
            //string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), ",");
            //string samplingMainGuidsStr = StringExtensions.GetArrayStrNoEmpty(samplingMainGuids.ToList<string>(), ",");
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            string sampleNumbersStr = StringExtensions.GetArrayStrNoEmpty(sampleNumbers.ToList<string>(), "','");
            string dateStr = string.Empty;
            string where = string.Empty;//查询条件拼接

            //if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
            //{
            //    idsStr = " AND PointId =" + idsStr;
            //}
            //else if (!string.IsNullOrEmpty(idsStr))
            //{
            //    idsStr = " AND PointId IN(" + idsStr + ")";
            //}
            //if (samplingMainGuids.Length == 1 && !string.IsNullOrEmpty(samplingMainGuids[0]))
            //{
            //    samplingMainGuidsStr = " AND SamplingMainGuid =" + samplingMainGuidsStr;
            //}
            //else if (!string.IsNullOrEmpty(samplingMainGuidsStr))
            //{
            //    samplingMainGuidsStr = " AND SamplingMainGuid IN(" + samplingMainGuidsStr + ")";
            //}
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                portIdsStr = " AND PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                portIdsStr = " AND PointId IN(" + portIdsStr + ")";
            }
            if (sampleNumbers.Length == 1 && !string.IsNullOrEmpty(sampleNumbers[0]))
            {
                sampleNumbersStr = " AND SampleNumber ='" + sampleNumbersStr + "'";
            }
            else if (!string.IsNullOrEmpty(sampleNumbersStr))
            {
                sampleNumbersStr = " AND SampleNumber IN('" + sampleNumbersStr + "')";
            }
            if (dtmStart != DateTime.MinValue)
            {
                dateStr += string.Format(" AND  CreatDateTime>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (dtmEnd != DateTime.MinValue)
            {
                dateStr += string.Format(" AND  CreatDateTime<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            where = string.Format(" 1=1 {0} {1} {2} ", portIdsStr, sampleNumbersStr, dateStr);

            return g_GridViewPager.GetAllDataCount(tableNameDetail, "SamplingDate", where, connection);
        }
        #endregion  Method
    }
}

