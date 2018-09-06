using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.DomainModel.Framework;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Air
{
    /// <summary>
    /// 名称：InstrumentFaultDAL.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-05-24
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 仪器表单
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InstrumentFaultDAL
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
        private string connection = "Frame_Connection";

        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_OMMP_InstrumentInstanceTimeRecord";
        private string tableName1 = "dbo.TB_OMMP_InstrumentInstanceRecord2";
        private string tableName2 = "dbo.TB_OMMP_InstrumentInstanceRecord3";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public InstrumentFaultDAL()
        {
            //connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Hour);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Add(InstrumentInstanceTimeRecordEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.RowGuid != null)
                {
                    strSql1.Append("RowGuid,");
                    strSql2.Append("'" + model.RowGuid + "',");
                }

                if (model.TypeGuid != null)
                {
                    strSql1.Append("TypeGuid,");
                    strSql2.Append("'" + model.TypeGuid + "',");
                }
                if (model.OccurTime != null)
                {
                    strSql1.Append("OccurTime,");
                    strSql2.Append("'" + model.OccurTime + "',");
                }
                if (model.OperateUserGuid != null)
                {
                    strSql1.Append("OperateUserGuid,");
                    strSql2.Append("'" + model.OperateUserGuid + "',");
                }
                if (model.OperateUserName != null)
                {
                    strSql1.Append("OperateUserName,");
                    strSql2.Append("'" + model.OperateUserName + "',");
                }
                if (model.OperateContent != null)
                {
                    strSql1.Append("OperateContent,");
                    strSql2.Append("'" + model.OperateContent + "',");
                }
                if (model.Note != null)
                {
                    strSql1.Append("Note,");
                    strSql2.Append("'" + model.Note + "',");
                }
                if (model.FormUrl != null)
                {
                    strSql1.Append("FormUrl,");
                    strSql2.Append("'" + model.FormUrl + "',");
                }
                if (model.RowStatus != null)
                {
                    strSql1.Append("RowStatus,");
                    strSql2.Append("'" + model.RowStatus + "',");
                }
                if (model.InstanceGuid != null)
                {
                    strSql1.Append("InstanceGuid,");
                    strSql2.Append("'" + model.InstanceGuid + "',");
                }
                if (model.ChangeType != null)
                {
                    strSql1.Append("ChangeType,");
                    strSql2.Append("'" + model.ChangeType + "',");
                }
                if (model.DeviceGuid != null)
                {
                    strSql1.Append("DeviceGuid,");
                    strSql2.Append("'" + model.DeviceGuid + "',");
                }
                if (model.OccurStatus != null)
                {
                    strSql1.Append("OccurStatus,");
                    strSql2.Append("'" + model.OccurStatus + "',");
                }
                if (model.OperateResault != null)
                {
                    strSql1.Append("OperateResault,");
                    strSql2.Append("'" + model.OperateResault + "',");
                }
                if (model.IsReagent != null)
                {
                    strSql1.Append("IsReagent,");
                    strSql2.Append("'" + model.IsReagent + "',");
                }
                if (model.ReagentGuid != null)
                {
                    strSql1.Append("ReagentGuid,");
                    strSql2.Append("'" + model.ReagentGuid + "',");
                }
                if (model.ID != null)
                {
                    strSql1.Append("ID,");
                    strSql2.Append("" + model.ID + ",");
                }
                if (model.ItemCount != null)
                {
                    strSql1.Append("ItemCount,");
                    strSql2.Append("" + model.ItemCount + ",");
                }
                if (model.PointId != null)
                {
                    strSql1.Append("PointId,");
                    strSql2.Append("" + model.PointId + ",");
                }
                strSql.Append("insert into TB_OMMP_InstrumentInstanceTimeRecord(");
                strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                strSql.Append(")");
                strSql.Append(" values (");
                strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                strSql.Append(")");
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
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int AddRecord2(InstrumentInstanceRecord2Entity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.RowGuid != null)
                {
                    strSql1.Append("RowGuid,");
                    strSql2.Append("'" + model.RowGuid + "',");
                }

                if (model.OperateUserGuid != null)
                {
                    strSql1.Append("OperateUserGuid,");
                    strSql2.Append("'" + model.OperateUserGuid + "',");
                }
                if (model.OperateUserName != null)
                {
                    strSql1.Append("OperateUserName,");
                    strSql2.Append("'" + model.OperateUserName + "',");
                }
                if (model.OperateDate != null)
                {
                    strSql1.Append("OperateDate,");
                    strSql2.Append("'" + model.OperateDate + "',");
                }
                if (model.OperateContent != null)
                {
                    strSql1.Append("OperateContent,");
                    strSql2.Append("'" + model.OperateContent + "',");
                }
                if (model.OperateResult != null)
                {
                    strSql1.Append("OperateResult,");
                    strSql2.Append("'" + model.OperateResult + "',");
                }
                if (model.Note != null)
                {
                    strSql1.Append("Note,");
                    strSql2.Append("'" + model.Note + "',");
                }
                if (model.FormUrl != null)
                {
                    strSql1.Append("FormUrl,");
                    strSql2.Append("'" + model.FormUrl + "',");
                }
                if (model.RowStatus != null)
                {
                    strSql1.Append("RowStatus,");
                    strSql2.Append("'" + model.RowStatus + "',");
                }
                if (model.InstrumentInstanceGuid != null)
                {
                    strSql1.Append("InstrumentInstanceGuid,");
                    strSql2.Append("'" + model.InstrumentInstanceGuid + "',");
                }
                if (model.ID != null)
                {
                    strSql1.Append("ID,");
                    strSql2.Append("" + model.ID + ",");
                }
                if (model.ObjectType != null)
                {
                    strSql1.Append("ObjectType,");
                    strSql2.Append("'" + model.ObjectType + "',");
                }
                if (model.OccurStatus != null)
                {
                    strSql1.Append("OccurStatus,");
                    strSql2.Append("'" + model.OccurStatus + "',");
                }
                strSql.Append("insert into TB_OMMP_InstrumentInstanceRecord2(");
                strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                strSql.Append(")");
                strSql.Append(" values (");
                strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                strSql.Append(")");
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
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int AddRecord3(InstrumentInstanceRecord3Entity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.RowGuid != null)
                {
                    strSql1.Append("RowGuid,");
                    strSql2.Append("'" + model.RowGuid + "',");
                }

                if (model.OperateUserGuid != null)
                {
                    strSql1.Append("OperateUserGuid,");
                    strSql2.Append("'" + model.OperateUserGuid + "',");
                }
                if (model.OperateUserName != null)
                {
                    strSql1.Append("OperateUserName,");
                    strSql2.Append("'" + model.OperateUserName + "',");
                }
                if (model.OperateDate != null)
                {
                    strSql1.Append("OperateDate,");
                    strSql2.Append("'" + model.OperateDate + "',");
                }
                if (model.OperateContent != null)
                {
                    strSql1.Append("OperateContent,");
                    strSql2.Append("'" + model.OperateContent + "',");
                }
                if (model.OperateResult != null)
                {
                    strSql1.Append("OperateResult,");
                    strSql2.Append("'" + model.OperateResult + "',");
                }
                if (model.Note != null)
                {
                    strSql1.Append("Note,");
                    strSql2.Append("'" + model.Note + "',");
                }
                if (model.FormUrl != null)
                {
                    strSql1.Append("FormUrl,");
                    strSql2.Append("'" + model.FormUrl + "',");
                }
                if (model.RowStatus != null)
                {
                    strSql1.Append("RowStatus,");
                    strSql2.Append("'" + model.RowStatus + "',");
                }
                if (model.InstrumentInstanceGuid != null)
                {
                    strSql1.Append("InstrumentInstanceGuid,");
                    strSql2.Append("'" + model.InstrumentInstanceGuid + "',");
                }
                if (model.ID != null)
                {
                    strSql1.Append("ID,");
                    strSql2.Append("" + model.ID + ",");
                }
                if (model.ObjectType != null)
                {
                    strSql1.Append("ObjectType,");
                    strSql2.Append("'" + model.ObjectType + "',");
                }
                if (model.OccurStatus != null)
                {
                    strSql1.Append("OccurStatus,");
                    strSql2.Append("'" + model.OccurStatus + "',");
                }
                strSql.Append("insert into TB_OMMP_InstrumentInstanceRecord3(");
                strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                strSql.Append(")");
                strSql.Append(" values (");
                strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                strSql.Append(")");
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
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Update(InstrumentInstanceTimeRecordEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_OMMP_InstrumentInstanceTimeRecord set ");
                if (model.RowGuid != null)
                {
                    strSql.Append("RowGuid='" + model.RowGuid + "',");
                }
                else
                {
                    strSql.Append("RowGuid= null ,");
                }
                if (model.TypeGuid != null)
                {
                    strSql.Append("TypeGuid='" + model.TypeGuid + "',");
                }
                else
                {
                    strSql.Append("TypeGuid= null ,");
                }
                if (model.OccurTime != null)
                {
                    strSql.Append("OccurTime='" + model.OccurTime + "',");
                }
                else
                {
                    strSql.Append("OccurTime= null ,");
                }
                if (model.OperateUserGuid != null)
                {
                    strSql.Append("OperateUserGuid='" + model.OperateUserGuid + "',");
                }
                else
                {
                    strSql.Append("OperateUserGuid= null ,");
                }
                if (model.OperateUserName != null)
                {
                    strSql.Append("OperateUserName='" + model.OperateUserName + "',");
                }
                else
                {
                    strSql.Append("OperateUserName= null ,");
                }
                if (model.OperateContent != null)
                {
                    strSql.Append("OperateContent='" + model.OperateContent + "',");
                }
                else
                {
                    strSql.Append("OperateContent= null ,");
                }
                if (model.Note != null)
                {
                    strSql.Append("Note='" + model.Note + "',");
                }
                else
                {
                    strSql.Append("Note= null ,");
                }
                if (model.FormUrl != null)
                {
                    strSql.Append("FormUrl='" + model.FormUrl + "',");
                }
                else
                    strSql.Append("FormUrl= null ,");
                if (model.RowStatus != null)
                {
                    strSql.Append("RowStatus='" + model.RowStatus + "',");
                }
                else
                {
                    strSql.Append("RowStatus= null ,");
                }
                if (model.InstanceGuid != null)
                {
                    strSql.Append("InstanceGuid='" + model.InstanceGuid + "',");
                }
                else
                {
                    strSql.Append("InstanceGuid= null ,");
                }
                if (model.ChangeType != null)
                {
                    strSql.Append("ChangeType='" + model.ChangeType + "',");
                }
                else
                {
                    strSql.Append("ChangeType= null ,");
                }
                if (model.DeviceGuid != null)
                {
                    strSql.Append("DeviceGuid='" + model.DeviceGuid + "',");
                }
                else
                {
                    strSql.Append("DeviceGuid= null ,");
                }
                if (model.OccurStatus != null)
                {
                    strSql.Append("OccurStatus='" + model.OccurStatus + "',");
                }
                else
                {
                    strSql.Append("OccurStatus= null ,");
                }
                if (model.OperateResault != null)
                {
                    strSql.Append("OperateResault='" + model.OperateResault + "',");
                }
                else
                {
                    strSql.Append("OperateResault= null ,");
                }
                if (model.IsReagent != null)
                {
                    strSql.Append("IsReagent='" + model.IsReagent + "',");
                }
                else
                {
                    strSql.Append("IsReagent= null ,");
                }
                if (model.ReagentGuid != null)
                {
                    strSql.Append("ReagentGuid='" + model.ReagentGuid + "',");
                }
                else
                {
                    strSql.Append("ReagentGuid= null ,");
                }
                if (model.ItemCount != null)
                {
                    strSql.Append("ItemCount=" + model.ItemCount + ",");
                }
                else
                {
                    strSql.Append("ItemCount= null ,");
                }
                if (model.PointId != null)
                {
                    strSql.Append("PointId=" + model.PointId + ",");
                }
                else
                {
                    strSql.Append("PointId= null ,");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where ID=" + model.ID);
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
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int UpdateRecord2(InstrumentInstanceRecord2Entity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_OMMP_InstrumentInstanceRecord2 set ");
                if (model.RowGuid != null)
                {
                    strSql.Append("RowGuid='" + model.RowGuid + "',");
                }
                else
                {
                    strSql.Append("RowGuid= null ,");
                }
                if (model.OperateUserGuid != null)
                {
                    strSql.Append("OperateUserGuid='" + model.OperateUserGuid + "',");
                }
                else
                {
                    strSql.Append("OperateUserGuid= null ,");
                }
                if (model.OperateUserName != null)
                {
                    strSql.Append("OperateUserName='" + model.OperateUserName + "',");
                }
                else
                {
                    strSql.Append("OperateUserName= null ,");
                }
                if (model.OperateDate != null)
                {
                    strSql.Append("OperateDate='" + model.OperateDate + "',");
                }
                else
                {
                    strSql.Append("OperateDate= null ,");
                }
                if (model.OperateContent != null)
                {
                    strSql.Append("OperateContent='" + model.OperateContent + "',");
                }
                else
                {
                    strSql.Append("OperateContent= null ,");
                }
                if (model.OperateResult != null)
                {
                    strSql.Append("OperateResult='" + model.OperateResult + "',");
                }
                else
                {
                    strSql.Append("OperateResult= null ,");
                }
                if (model.Note != null)
                {
                    strSql.Append("Note='" + model.Note + "',");
                }
                else
                {
                    strSql.Append("Note= null ,");
                }
                if (model.FormUrl != null)
                {
                    strSql.Append("FormUrl='" + model.FormUrl + "',");
                }
                else
                    strSql.Append("FormUrl= null ,");
                if (model.RowStatus != null)
                {
                    strSql.Append("RowStatus='" + model.RowStatus + "',");
                }
                else
                {
                    strSql.Append("RowStatus= null ,");
                }
                if (model.InstrumentInstanceGuid != null)
                {
                    strSql.Append("InstrumentInstanceGuid='" + model.InstrumentInstanceGuid + "',");
                }
                else
                {
                    strSql.Append("InstrumentInstanceGuid= null");
                }
                if (model.ObjectType != null)
                {
                    strSql.Append("ObjectType='" + model.ObjectType + "',");
                }
                else
                {
                    strSql.Append("ObjectType= null");
                }
                if (model.OccurStatus != null)
                {
                    strSql.Append("OccurStatus='" + model.OccurStatus + "',");
                }
                else
                {
                    strSql.Append("OccurStatus= null");
                }
                //if (model.ID != null)
                //{
                //    strSql.Append("ID=" + model.ID + ",");
                //}
                //else
                //{
                //    strSql.Append("ID= null ,");
                //}
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where RowGuid='" + model.RowGuid + "' ");
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
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int UpdateRecord3(InstrumentInstanceRecord3Entity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_OMMP_InstrumentInstanceRecord3 set ");
                if (model.RowGuid != null)
                {
                    strSql.Append("RowGuid='" + model.RowGuid + "',");
                }
                else
                {
                    strSql.Append("RowGuid= null ,");
                }
                if (model.OperateUserGuid != null)
                {
                    strSql.Append("OperateUserGuid='" + model.OperateUserGuid + "',");
                }
                else
                {
                    strSql.Append("OperateUserGuid= null ,");
                }
                if (model.OperateUserName != null)
                {
                    strSql.Append("OperateUserName='" + model.OperateUserName + "',");
                }
                else
                {
                    strSql.Append("OperateUserName= null ,");
                }
                if (model.OperateDate != null)
                {
                    strSql.Append("OperateDate='" + model.OperateDate + "',");
                }
                else
                {
                    strSql.Append("OperateDate= null ,");
                }
                if (model.OperateContent != null)
                {
                    strSql.Append("OperateContent='" + model.OperateContent + "',");
                }
                else
                {
                    strSql.Append("OperateContent= null ,");
                }
                if (model.OperateResult != null)
                {
                    strSql.Append("OperateResult='" + model.OperateResult + "',");
                }
                else
                {
                    strSql.Append("OperateResult= null ,");
                }
                if (model.Note != null)
                {
                    strSql.Append("Note='" + model.Note + "',");
                }
                else
                {
                    strSql.Append("Note= null ,");
                }
                if (model.FormUrl != null)
                {
                    strSql.Append("FormUrl='" + model.FormUrl + "',");
                }
                else
                    strSql.Append("FormUrl= null ,");
                if (model.RowStatus != null)
                {
                    strSql.Append("RowStatus='" + model.RowStatus + "',");
                }
                else
                {
                    strSql.Append("RowStatus= null ,");
                }
                if (model.InstrumentInstanceGuid != null)
                {
                    strSql.Append("InstrumentInstanceGuid='" + model.InstrumentInstanceGuid + "',");
                }
                else
                {
                    strSql.Append("InstrumentInstanceGuid= null");
                }
                if (model.ObjectType != null)
                {
                    strSql.Append("ObjectType='" + model.ObjectType + "',");
                }
                else
                {
                    strSql.Append("ObjectType= null");
                }
                if (model.OccurStatus != null)
                {
                    strSql.Append("OccurStatus='" + model.OccurStatus + "',");
                }
                else
                {
                    strSql.Append("OccurStatus= null");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where RowGuid='" + model.RowGuid + "' ");
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
        public int DeleteRecord2(int id)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from TB_OMMP_InstrumentInstanceRecord2 ");
                strSql.Append(" where ID=" + id);
                strSql.Append(";select @@ROWCOUNT");
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
        public int DeleteRecord3(int id)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from TB_OMMP_InstrumentInstanceRecord3 ");
                strSql.Append(" where ID=" + id);
                //strSql.Append("delete from TB_OMMP_InstrumentInstanceRecord3 ");
                //strSql.Append(" where RowGuid='" + rowguid + "'");
                strSql.Append(";select @@ROWCOUNT");
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
                strSql.Append("delete from TB_OMMP_InstrumentInstanceTimeRecord ");
                strSql.Append(" where ID in (" + sqlId + ")  ");
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
        public InstrumentInstanceTimeRecordEntity GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append("*");
            strSql.Append(" from TB_OMMP_InstrumentInstanceTimeRecord ");
            strSql.Append(" where ID=" + id);
            InstrumentInstanceTimeRecordEntity model = new InstrumentInstanceTimeRecordEntity();
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
        /// <param name="id">id</param>
        /// <returns></returns>
        public InstrumentInstanceRecord2Entity GetModelRecord2(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append("*");
            strSql.Append(" from TB_OMMP_InstrumentInstanceRecord2 ");
            strSql.Append(" where Id=" + Id);
            InstrumentInstanceRecord2Entity model = new InstrumentInstanceRecord2Entity();
            DataTable dt = g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
            if (dt.Rows.Count > 0)
            {
                return DataRowToModelRecord2(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public InstrumentInstanceRecord3Entity GetModelRecord3(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append("*");
            strSql.Append(" from TB_OMMP_InstrumentInstanceRecord3 ");
            strSql.Append(" where Id=" + Id);
            InstrumentInstanceRecord3Entity model = new InstrumentInstanceRecord3Entity();
            DataTable dt = g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
            if (dt.Rows.Count > 0)
            {
                return DataRowToModelRecord3(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public InstrumentInstanceTimeRecordEntity DataRowToModel(DataRow row)
        {
            InstrumentInstanceTimeRecordEntity model = new InstrumentInstanceTimeRecordEntity();
            if (row != null)
            {
                if (row["RowGuid"] != null)
                {
                    model.RowGuid = row["RowGuid"].ToString();
                }
                if (row["TypeGuid"] != null)
                {
                    model.TypeGuid = row["TypeGuid"].ToString();
                }
                if (row["OccurTime"] != null && row["OccurTime"].ToString() != "")
                {
                    model.OccurTime = DateTime.Parse(row["OccurTime"].ToString());
                }
                if (row["OperateUserGuid"] != null)
                {
                    model.OperateUserGuid = row["OperateUserGuid"].ToString();
                }
                if (row["OperateUserName"] != null)
                {
                    model.OperateUserName = row["OperateUserName"].ToString();
                }
                if (row["OperateContent"] != null)
                {
                    model.OperateContent = row["OperateContent"].ToString();
                }
                if (row["Note"] != null)
                {
                    model.Note = row["Note"].ToString();
                }
                if (row["FormUrl"] != null)
                {
                    model.FormUrl = row["FormUrl"].ToString();
                }
                if (row["RowStatus"] != null)
                {
                    model.RowStatus = row["RowStatus"].ToString();
                }
                if (row["InstanceGuid"] != null)
                {
                    model.InstanceGuid = row["InstanceGuid"].ToString();
                }
                if (row["ChangeType"] != null)
                {
                    model.ChangeType = row["ChangeType"].ToString();
                }
                if (row["DeviceGuid"] != null)
                {
                    model.DeviceGuid = row["DeviceGuid"].ToString();
                }
                if (row["OccurStatus"] != null)
                {
                    model.OccurStatus = row["OccurStatus"].ToString();
                }
                if (row["OperateResault"] != null)
                {
                    model.OperateResault = row["OperateResault"].ToString();
                }
                if (row["IsReagent"] != null)
                {
                    model.IsReagent = row["IsReagent"].ToString();
                }
                if (row["ReagentGuid"] != null)
                {
                    model.ReagentGuid = row["ReagentGuid"].ToString();
                }
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["ItemCount"] != null && row["ItemCount"].ToString() != "")
                {
                    model.ItemCount = int.Parse(row["ItemCount"].ToString());
                }
            }
            return model;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public InstrumentInstanceRecord2Entity DataRowToModelRecord2(DataRow row)
        {
            InstrumentInstanceRecord2Entity model = new InstrumentInstanceRecord2Entity();
            if (row != null)
            {
                if (row["RowGuid"] != null)
                {
                    model.RowGuid = row["RowGuid"].ToString();
                }
                if (row["OperateUserGuid"] != null)
                {
                    model.OperateUserGuid = row["OperateUserGuid"].ToString();
                }
                if (row["OperateUserName"] != null)
                {
                    model.OperateUserName = row["OperateUserName"].ToString();
                }
                if (row["OperateDate"] != null && row["OperateDate"].ToString() != "")
                {
                    model.OperateDate = DateTime.Parse(row["OperateDate"].ToString());
                }
                if (row["OperateContent"] != null)
                {
                    model.OperateContent = row["OperateContent"].ToString();
                }
                if (row["OperateResult"] != null)
                {
                    model.OperateResult = row["OperateResult"].ToString();
                }
                if (row["Note"] != null)
                {
                    model.Note = row["Note"].ToString();
                }
                if (row["FormUrl"] != null)
                {
                    model.FormUrl = row["FormUrl"].ToString();
                }
                if (row["RowStatus"] != null)
                {
                    model.RowStatus = row["RowStatus"].ToString();
                }
                if (row["InstrumentInstanceGuid"] != null)
                {
                    model.InstrumentInstanceGuid = row["InstrumentInstanceGuid"].ToString();
                }
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
            }
            return model;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public InstrumentInstanceRecord3Entity DataRowToModelRecord3(DataRow row)
        {
            InstrumentInstanceRecord3Entity model = new InstrumentInstanceRecord3Entity();
            if (row != null)
            {
                if (row["RowGuid"] != null)
                {
                    model.RowGuid = row["RowGuid"].ToString();
                }
                if (row["OperateUserGuid"] != null)
                {
                    model.OperateUserGuid = row["OperateUserGuid"].ToString();
                }
                if (row["OperateUserName"] != null)
                {
                    model.OperateUserName = row["OperateUserName"].ToString();
                }
                if (row["OperateDate"] != null && row["OperateDate"].ToString() != "")
                {
                    model.OperateDate = DateTime.Parse(row["OperateDate"].ToString());
                }
                if (row["OperateContent"] != null)
                {
                    model.OperateContent = row["OperateContent"].ToString();
                }
                if (row["OperateResult"] != null)
                {
                    model.OperateResult = row["OperateResult"].ToString();
                }
                if (row["Note"] != null)
                {
                    model.Note = row["Note"].ToString();
                }
                if (row["FormUrl"] != null)
                {
                    model.FormUrl = row["FormUrl"].ToString();
                }
                if (row["RowStatus"] != null)
                {
                    model.RowStatus = row["RowStatus"].ToString();
                }
                if (row["InstrumentInstanceGuid"] != null)
                {
                    model.InstrumentInstanceGuid = row["InstrumentInstanceGuid"].ToString();
                }
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
            }
            return model;
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
        public DataView GetDataPager(string[] Users, DateTime dtmStart, DateTime dtmEnd, string operateContent, string objectType,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "OperateDate")
        {
            recordTotal = 0;
            try
            {
                string InstrumentStr = "";
                string UsersStr = "";
                string operateStr = "";
                //if (Instruments != null && Instruments.Length > 0 && Instruments[0].ToString() != "")
                //    InstrumentStr = " AND InstanceGuid IN ('" + StringExtensions.GetArrayStrNoEmpty(Instruments.ToList(), "','") + "')";

                if (Users != null && Users.Length > 0 && Users[0].ToString() != "")
                    UsersStr = " AND OccurStatus IN ('" + StringExtensions.GetArrayStrNoEmpty(Users.ToList(), "','") + "')";
                if (operateContent != "")
                    operateStr = " AND OperateContent ='" + operateContent + "'";
                string dateStr = string.Empty;
                string where = string.Empty;//查询条件拼接

                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  CONVERT(datetime ,OperateDate)>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  CONVERT(datetime ,OperateDate)<='{0}'", dtmEnd.ToString("yyyy-MM-dd 23:59:59"));
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "CONVERT(datetime ,OperateDate)" : orderBy;

                where = string.Format(" 1=1 {0} {1} {2} {3}  and objectType='{4}'", InstrumentStr, UsersStr, dateStr, operateStr, objectType);

                string sql = string.Format(@"select *
                                             from {0} 
                                            where {1} order by CONVERT(datetime ,OperateDate)",
                                           tableName1, where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
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
        public DataView GetDataNewPager(string[] Users, DateTime dtmStart, DateTime dtmEnd, string operateContent, string objectType,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "OperateDate")
        {
            recordTotal = 0;
            try
            {
                string InstrumentStr = "";
                string UsersStr = "";
                string operateStr = "";
                //if (Instruments != null && Instruments.Length > 0 && Instruments[0].ToString() != "")
                //    InstrumentStr = " AND InstanceGuid IN ('" + StringExtensions.GetArrayStrNoEmpty(Instruments.ToList(), "','") + "')";

                if (Users != null && Users.Length > 0 && Users[0].ToString() != "")
                    UsersStr = " AND OccurStatus IN ('" + StringExtensions.GetArrayStrNoEmpty(Users.ToList(), "','") + "')";
                if (operateContent != "")
                    operateStr = " AND OperateContent ='" + operateContent + "'";
                string dateStr = string.Empty;
                string where = string.Empty;//查询条件拼接

                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  CONVERT(datetime ,OperateDate)>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  CONVERT(datetime ,OperateDate)<='{0}'", dtmEnd.ToString("yyyy-MM-dd 23:59:59"));
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "CONVERT(datetime ,OperateDate)" : orderBy;

                where = string.Format(" 1=1 {0} {1} {2} {3}  and objectType='{4}'", InstrumentStr, UsersStr, dateStr, operateStr, objectType);

                string sql = string.Format(@"select *
                                             from {0} 
                                            where {1} order by CONVERT(datetime ,OperateDate)",
                                           tableName2, where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得主表导出数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, string orderBy = "UsedDate")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string dateStr = string.Empty;
                string keyName = "id";
                string fieldName = "id,PointId,InstrumentNumber,UsedUser,UsedDate,ShouldReturnDate,RealReturnDate,UseContent,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ";
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
                    dateStr += string.Format(" AND  UsedDate>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  UsedDate<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "UsedDate" : orderBy;
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
        /// 分页获取数据列表
        /// </summary>
        public DataTable GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.id desc");
            }
            strSql.Append(")AS Row, T.*  from TB_InstrumentUsedRecord T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion  Method
    }
}
