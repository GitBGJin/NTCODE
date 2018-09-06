using SmartEP.Core.Generic;
using SmartEP.DomainModel.Framework;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Water
{
    /// <summary>
    /// 名称：OM_ReagentInBillItemDAL.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-08-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：标液配置表DAL层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class OM_ReagentInBillItemDAL
    {
        #region  Method
        public OM_ReagentInBillItemDAL()
        { }
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(OM_ReagentInBillItemEntiy model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.RowGuid != null)
            {
                strSql1.Append("RowGuid,");
                strSql2.Append("'" + model.RowGuid + "',");
            }
            if (model.BillGuid != null)
            {
                strSql1.Append("BillGuid,");
                strSql2.Append("'" + model.BillGuid + "',");
            }
            if (model.ReagentGuid != null)
            {
                strSql1.Append("ReagentGuid,");
                strSql2.Append("'" + model.ReagentGuid + "',");
            }
            if (model.ItemCount != null)
            {
                strSql1.Append("ItemCount,");
                strSql2.Append("'" + model.ItemCount + "',");
            }
            if (model.ReagentName != null)
            {
                strSql1.Append("ReagentName,");
                strSql2.Append("'" + model.ReagentName + "',");
            }
            if (model.ReagentParentName != null)
            {
                strSql1.Append("ReagentParentName,");
                strSql2.Append("'" + model.ReagentParentName + "',");
            }
            if (model.ProductSN != null)
            {
                strSql1.Append("ProductSN,");
                strSql2.Append("'" + model.ProductSN + "',");
            }
            if (model.GuaranteeNumber != null)
            {
                strSql1.Append("GuaranteeNumber,");
                strSql2.Append("'" + model.GuaranteeNumber + "',");
            }
            if (model.PeriodType != null)
            {
                strSql1.Append("PeriodType,");
                strSql2.Append("'" + model.PeriodType + "',");
            }
            if (model.ManufactureDate != null)
            {
                strSql1.Append("ManufactureDate,");
                strSql2.Append("'" + model.ManufactureDate + "',");
            }
            if (model.EndValidDate != null)
            {
                strSql1.Append("EndValidDate,");
                strSql2.Append("'" + model.EndValidDate + "',");
            }
            if (model.Source != null)
            {
                strSql1.Append("Source,");
                strSql2.Append("'" + model.Source + "',");
            }
            if (model.ParentCount != null)
            {
                strSql1.Append("ParentCount,");
                strSql2.Append("'" + model.ParentCount + "',");
            }
            if (model.ReagentParentGuid != null)
            {
                strSql1.Append("ReagentParentGuid,");
                strSql2.Append("'" + model.ReagentParentGuid + "',");
            }
            if (model.Note != null)
            {
                strSql1.Append("Note,");
                strSql2.Append("'" + model.Note + "',");
            }
            if (model.ReagentType != null)
            {
                strSql1.Append("ReagentType,");
                strSql2.Append("'" + model.ReagentType + "',");
            }
            if (model.RowStatus != null)
            {
                strSql1.Append("RowStatus,");
                strSql2.Append("'" + model.RowStatus + "',");
            }
            if (model.TypeName1 != null)
            {
                strSql1.Append("TypeName1,");
                strSql2.Append("'" + model.TypeName1 + "',");
            }
            if (model.TypeName2 != null)
            {
                strSql1.Append("TypeName2,");
                strSql2.Append("'" + model.TypeName2 + "',");
            }
            if (model.MiddleProductSN != null)
            {
                strSql1.Append("MiddleProductSN,");
                strSql2.Append("'" + model.MiddleProductSN + "',");
            }
            if (model.Capacity != null)
            {
                strSql1.Append("Capacity,");
                strSql2.Append("'" + model.Capacity + "',");
            }
            if (model.SourceType != null)
            {
                strSql1.Append("SourceType,");
                strSql2.Append("'" + model.SourceType + "',");
            }
            if (model.Number != null)
            {
                strSql1.Append("Number,");
                strSql2.Append("'" + model.Number + "',");
            }
            if (model.Concentration != null)
            {
                strSql1.Append("Concentration,");
                strSql2.Append("'" + model.Concentration + "',");
            }
            if (model.Multiple != null)
            {
                strSql1.Append("Multiple,");
                strSql2.Append("'" + model.Multiple + "',");
            }
            if (model.DiluteConcen != null)
            {
                strSql1.Append("DiluteConcen,");
                strSql2.Append("'" + model.DiluteConcen + "',");
            }
            if (model.OldProductSN != null)
            {
                strSql1.Append("OldProductSN,");
                strSql2.Append("'" + model.OldProductSN + "',");
            }
            if (model.TypeName != null)
            {
                strSql1.Append("TypeName,");
                strSql2.Append("'" + model.TypeName + "',");
            }
            if (model.Dilution != null)
            {
                strSql1.Append("Dilution,");
                strSql2.Append("'" + model.Dilution + "',");
            }
            if (model.Unit != null)
            {
                strSql1.Append("Unit,");
                strSql2.Append("'" + model.Unit + "',");
            }
            if (model.OldSystemSN != null)
            {
                strSql1.Append("OldSystemSN,");
                strSql2.Append("'" + model.OldSystemSN + "',");
            }
            if (model.Description != null)
            {
                strSql1.Append("Description,");
                strSql2.Append("'" + model.Description + "',");
            }
            if (model.ActualConcentration != null)
            {
                strSql1.Append("ActualConcentration,");
                strSql2.Append("'" + model.ActualConcentration + "',");
            }
            if (model.ActualMultiple != null)
            {
                strSql1.Append("ActualMultiple,");
                strSql2.Append("'" + model.ActualMultiple + "',");
            }
            if (model.IsBlindSolution != null)
            {
                strSql1.Append("IsBlindSolution,");
                strSql2.Append("'" + model.IsBlindSolution + "',");
            }
            if (model.ConfigPeople != null)
            {
                strSql1.Append("ConfigPeople,");
                strSql2.Append("'" + model.ConfigPeople + "',");
            }
            if (model.RecordFlag != null)
            {
                strSql1.Append("RecordFlag,");
                strSql2.Append("'" + model.RecordFlag + "',");
            }
            if (model.NumberUnit != null)
            {
                strSql1.Append("NumberUnit,");
                strSql2.Append("'" + model.NumberUnit + "',");
            }
            if (model.ConfigAmount != null)
            {
                strSql1.Append("ConfigAmount,");
                strSql2.Append("'" + model.ConfigAmount + "',");
            }
            if (model.GuaranteeUnit != null)
            {
                strSql1.Append("GuaranteeUnit,");
                strSql2.Append("'" + model.GuaranteeUnit + "',");
            }
            strSql.Append("insert into TB_OMMP_ReagentInBillItem(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            strSql.Append(";select @@IDENTITY");
            object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddBill(OM_ReagentInBillEntiy model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.RowGuid != null)
            {
                strSql1.Append("RowGuid,");
                strSql2.Append("'" + model.RowGuid + "',");
            }
            if (model.BillSN != null)
            {
                strSql1.Append("BillSN,");
                strSql2.Append("'" + model.BillSN + "',");
            }
            if (model.InHouseDate != null)
            {
                strSql1.Append("InHouseDate,");
                strSql2.Append("'" + model.InHouseDate + "',");
            }
            if (model.RecorderName != null)
            {
                strSql1.Append("RecorderName,");
                strSql2.Append("'" + model.RecorderName + "',");
            }
            if (model.RecorderGuid != null)
            {
                strSql1.Append("RecorderGuid,");
                strSql2.Append("'" + model.RecorderGuid + "',");
            }
            if (model.Note != null)
            {
                strSql1.Append("Note,");
                strSql2.Append("'" + model.Note + "',");
            }
            if (model.RowStatus != null)
            {
                strSql1.Append("RowStatus,");
                strSql2.Append("'" + model.RowStatus + "',");
            }
            if (model.ParentType != null)
            {
                strSql1.Append("ParentType,");
                strSql2.Append("'" + model.ParentType + "',");
            }

            strSql.Append("insert into TB_OMMP_ReagentInBill(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            strSql.Append(";select @@IDENTITY");
            object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddDetail(OM_ReagentInItemDetailEntiy model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.RowGuid != null)
            {
                strSql1.Append("RowGuid,");
                strSql2.Append("'" + model.RowGuid + "',");
            }
            if (model.FixCode != null)
            {
                strSql1.Append("FixCode,");
                strSql2.Append("'" + model.FixCode + "',");
            }
            if (model.BillItemGuid != null)
            {
                strSql1.Append("BillItemGuid,");
                strSql2.Append("'" + model.BillItemGuid + "',");
            }
            if (model.ManufactureDate != null)
            {
                strSql1.Append("ManufactureDate,");
                strSql2.Append("'" + model.ManufactureDate + "',");
            }
            if (model.EndValidDate != null)
            {
                strSql1.Append("EndValidDate,");
                strSql2.Append("'" + model.EndValidDate + "',");
            }
            if (model.IsPrint != null)
            {
                strSql1.Append("IsPrint,");
                strSql2.Append("'" + model.IsPrint + "',");
            }

            strSql.Append("insert into TB_OMMP_ReagentInItemDetail(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            strSql.Append(";select @@IDENTITY");
            object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddStyle(OM_ReagentTypeEntiy model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.RowGuid != null)
            {
                strSql1.Append("RowGuid,");
                strSql2.Append("'" + model.RowGuid + "',");
            }
            if (model.TypeName != null)
            {
                strSql1.Append("TypeName,");
                strSql2.Append("'" + model.TypeName + "',");
            }
            if (model.SortNum != null)
            {
                strSql1.Append("SortNum,");
                strSql2.Append("'" + model.SortNum + "',");
            }
            if (model.Type != null)
            {
                strSql1.Append("Type,");
                strSql2.Append("'" + model.Type + "',");
            }
            if (model.ReagentName != null)
            {
                strSql1.Append("ReagentName,");
                strSql2.Append("'" + model.ReagentName + "',");
            }
            if (model.Concentration != null)
            {
                strSql1.Append("Concentration,");
                strSql2.Append("'" + model.Concentration + "',");
            }
            if (model.WaringCount != null)
            {
                strSql1.Append("WaringCount,");
                strSql2.Append("'" + model.WaringCount + "',");
            }
            if (model.ConcentrationUnitGuid != null)
            {
                strSql1.Append("ConcentrationUnitGuid,");
                strSql2.Append("'" + model.ConcentrationUnitGuid + "',");
            }
            strSql.Append("insert into TB_OMMP_ReagentType(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            strSql.Append(";select @@IDENTITY");
            object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddRea(OM_ReagentEntiy model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.RowGuid != null)
            {
                strSql1.Append("RowGuid,");
                strSql2.Append("'" + model.RowGuid + "',");
            }
            if (model.TypeName != null)
            {
                strSql1.Append("TypeName,");
                strSql2.Append("'" + model.TypeName + "',");
            }
            if (model.OrgGuid != null)
            {
                strSql1.Append("OrgGuid,");
                strSql2.Append("'" + model.OrgGuid + "',");
            }
            if (model.ParentType != null)
            {
                strSql1.Append("ParentType,");
                strSql2.Append("'" + model.ParentType + "',");
            }
            if (model.ReagentName != null)
            {
                strSql1.Append("ReagentName,");
                strSql2.Append("'" + model.ReagentName + "',");
            }
            if (model.Concentration != null)
            {
                strSql1.Append("Concentration,");
                strSql2.Append("'" + model.Concentration + "',");
            }
            if (model.Code != null)
            {
                strSql1.Append("Code,");
                strSql2.Append("'" + model.Code + "',");
            }
            if (model.ConcentrationUnitGuid != null)
            {
                strSql1.Append("ConcentrationUnitGuid,");
                strSql2.Append("'" + model.ConcentrationUnitGuid + "',");
            }
            strSql.Append("insert into TB_OMMP_Reagent(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            strSql.Append(";select @@IDENTITY");
            object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        ///// <summary>
        ///// 更新一条数据
        ///// </summary>
        public bool Update(string[] SN)
        {
            string where = string.Empty;
            if (SN != null && SN.Length > 0)
            {
                where = " where RowGuid IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TB_OMMP_ReagentInBillItem set ");
            strSql.Append("RecordFlag=2,");

            int n = strSql.ToString().LastIndexOf(",");
            strSql.Remove(n, 1);
            strSql.Append(where);
            int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString));
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///// <summary>
        ///// 更新一条数据
        ///// </summary>
        public bool Update(string StringSN, string moveVolume)
        {
            string[] SN = new string[1];
            SN[0] = StringSN;
            decimal MV = Convert.ToDecimal(moveVolume);
            StringBuilder strsql2 = new StringBuilder();
            strsql2.Append("select Number from TB_OMMP_ReagentInBillItem where RowGuid IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')");
            decimal Number = Convert.ToDecimal(g_DatabaseHelper.ExecuteScalar(strsql2.ToString(), ConnectionString));
            decimal Num = Number - MV;
            if (Num > 0)
            {
                string where = string.Empty;
                if (SN != null && SN.Length > 0)
                {
                    where = " where RowGuid IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_OMMP_ReagentInBillItem set ");
                strSql.Append("Number=" + Num + ",");

                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(where);
                int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString));
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (Num <= 0)
            {
                string where = string.Empty;
                if (SN != null && SN.Length > 0)
                {
                    where = " where RowGuid IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_OMMP_ReagentInBillItem set ");
                strSql.Append("RecordFlag=2" + ",");

                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(where);
                int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString));
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取系统编号数字部分
        /// </summary>
        /// <returns></returns>
        public string Count()
        {
            string sql = "select MiddleProductSN from EQMS_Framework.dbo.TB_OMMP_ReagentInBillItem order by MiddleProductSN desc";
            DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, ConnectionString);
            if (dt.Rows.Count == 0)
            {
                return "WB0000";
            }
            return dt.Rows[0]["MiddleProductSN"].ToString();
        }
        /// <summary>
        /// 插入空数据
        /// </summary>
        /// <param name="BillItemGuid"></param>
        /// <param name="SysNum"></param>
        public void InsertNullData(Guid BillItemGuid, string SysNum)
        {
            try
            {
                string sql = string.Format(@"insert into EQMS_Framework.dbo.TB_OMMP_ReagentInBillItem (RowGuid, MiddleProductSN) Values('{0}','{1}') ", BillItemGuid, SysNum);
                g_DatabaseHelper.ExecuteNonQuery(sql, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询母溶液的稀释倍数与浓度
        /// </summary>
        /// <param name="RowGuids"></param>
        /// <returns></returns>
        public DataTable SelectMultipleAndConcentration(string SN)
        {
            string sql = @"select Multiple,DiluteConcen from  [EQMS_Framework].[dbo].[TB_OMMP_ReagentInBillItem]  
                            where MiddleProductSN='" + SN +"'";
            return g_DatabaseHelper.ExecuteDataTable(sql, ConnectionString);
        }
        /// <summary>
        /// 查询母溶液的浓度
        /// </summary>
        /// <param name="RowGuids"></param>
        /// <returns></returns>
        public DataTable SelectConcentration(string SN,string ProNum)
        {
            string sql = @"select Concentration from  [EQMS_Framework].[dbo].[TB_OMMP_ReagentInBillItem]  
                            where MiddleProductSN='" + SN + "' and ProductSN='"+ProNum+"'";
            return g_DatabaseHelper.ExecuteDataTable(sql, ConnectionString);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int IDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [TB_OMMP_ReagentInBillItem] ");
            strSql.Append(" where ID =" + IDs);
            int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString));
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            //if (strWhere.Contains("有证物质"))
            //    strSql.Append("select ID,RowGuid,BillGuid,ReagentGuid,MiddleProductSN,TypeName,ReagentName,EndValidDate,TypeName1,Number,Concentration,DiluteConcen,Unit,Note,Multiple,ActualConcentration,ActualMultiple,Dilution,OldProductSN,OldSystemSN,ProductSN,ManufactureDate,ReagentType,RowStatus,IsBlindSolution,Description");
            //else
            strSql.Append("select *");
            strSql.Append(" FROM TB_OMMP_ReagentInBillItem ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where (1=1) " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetLists(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TypeName1");
            strSql.Append(" FROM TB_OMMP_ReagentInBillItem ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where (1=1) " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetListStyle(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" FROM TB_OMMP_ReagentType ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where (1=1) and ReagentName='" + strWhere + "' Order By SortNum desc");
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns>获取符合条件的数据</returns> 
        public string rowGuid(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" FROM TB_OMMP_ReagentInItemDetail ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where (1=1) and BillItemGuid='" + strWhere + "'");
            }
            DataTable dt = g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
            return dt.Rows[0]["RowGuid"].ToString();
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        private string ConnectionString
        {
            get
            {
                return "Frame_Connection";
            }
        }
        #endregion  Method
    }
}
