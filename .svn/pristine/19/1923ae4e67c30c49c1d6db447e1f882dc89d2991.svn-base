using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：ReagentFormulaDAL.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-08-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 试剂配置数据访问类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class ReagentFormulaDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string Frame_Connection = "Frame_Connection";
        private string Base_Connection = "AMS_BaseDataConnection";
        /// <summary>
        /// 试剂类型数据库表名
        /// </summary>
        private string Reagent_TableName = "[dbo].[TB_OMMP_ReagentType]";
        private string ReagentFormula_TableName = "dbo.TB_ReagentFormula";
        private string Instance_TableName = "[dbo].[TB_OMMP_InstrumentInfo]";
        /// <summary>
        /// 获取试剂类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetReagentKind()
        {
            try
            {
                string sql = string.Format(@"SELECT [RowGuid],[TypeName]
                                              FROM {0}
                                              where rowstatus=1 and [Type]='标液'", Reagent_TableName);
                return g_DatabaseHelper.ExecuteDataTable(sql, Frame_Connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据仪器类型获取试剂类型
        /// </summary>
        /// <param name="InstanceGuid"></param>
        /// <returns></returns>
        public DataTable GetReagentKind(string InstanceGuid)
        {
            try
            {
                //select a.[RowGuid],a.[TypeName] from [EQMS_Framework].[dbo].[TB_OMMP_ReagentType] a
                // left join [AMS_BaseData].[dbo].[TB_ReagentFormula] b 
                // on a.[RowGuid] = b.[ReagentGuid] where a.[RowStatus]=1 and a.[Type]='标液' and b.[InstanceGuid] = '0006D53A-D489-4A4D-9CFB-BBB791BB85B0';
                string sql = string.Format(@"SELECT distinct a.[RowGuid],a.[TypeName]
                                              FROM {0} as a
                                              left join [AMS_BaseData].[dbo].[TB_ReagentFormula] as b
                                              on a.[RowGuid] = b.[ReagentGuid] 
                                              where a.[RowStatus]=1 and a.[Type]='标液' and b.[InstanceGuid] = '{1}';", Reagent_TableName, InstanceGuid);
                return g_DatabaseHelper.ExecuteDataTable(sql, Frame_Connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取仪器类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetInstanceKind()
        {
            try
            {
                string sql = string.Format(@"SELECT RowGuid,InstrumentName+'('+SpecificationModel+')' as InstrumentName
                                            FROM  {0} 
                                            where rowstatus=1 and ObjectType=1", Instance_TableName);
                return g_DatabaseHelper.ExecuteDataTable(sql, Frame_Connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据试剂类型获取化学品
        /// </summary>
        /// <param name="ReagentGuid"></param>
        /// <returns></returns>
        public DataTable GetReagentFormula(string ReagentGuid)
        {
            try
            {
                string sql = string.Format(@"SELECT [ReagentGuid]
                                                        ,c.ChemistryType
                                                      ,a.[ChemistryGuid]
                                                      ,b.[ChemistryName]
                                                      ,b.Specifications
                                                      ,a.[SingleDose]
                                                      ,a.[ChemistryUnit]
                                                      ,a.[SolutionVolume] 
                                                      ,a.[SolutionVolumeUnit]
                                                  FROM {0} as a  left join dbo.TB_Chemistry as b  
                                                    on a.ChemistryGuid=b.RowGuid
                                                    left join [dbo].[TB_ChemistryType] as c
                                                    on b.[ChemistryTypeGuid]=c.[RowGuid]
                                                where ReagentGuid='{1}'", ReagentFormula_TableName, ReagentGuid);
                return g_DatabaseHelper.ExecuteDataTable(sql, Base_Connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据仪器类型和试剂类型获取化学品
        /// </summary>
        /// <param name="ReagentGuid"></param>
        /// <returns></returns>
        public DataTable GetReagentFormula(string ReagentGuid, string InstanceGuid)
        {
            try
            {
                string sql = string.Format(@"SELECT [ReagentGuid]
                                                        ,c.ChemistryType
                                                      ,a.[ChemistryGuid]
                                                      ,b.[ChemistryName]
                                                      ,b.Specifications
                                                      ,a.[SingleDose]
                                                      ,a.[ChemistryUnit]
                                                      ,a.[SolutionVolume] 
                                                      ,a.[SolutionVolumeUnit]
                                                  FROM {0} as a  left join dbo.TB_Chemistry as b  
                                                    on a.ChemistryGuid=b.RowGuid
                                                    left join [dbo].[TB_ChemistryType] as c
                                                    on b.[ChemistryTypeGuid]=c.[RowGuid]
                                                where ReagentGuid='{1}' and InstanceGuid='{2}'", ReagentFormula_TableName, ReagentGuid, InstanceGuid);
                return g_DatabaseHelper.ExecuteDataTable(sql, Base_Connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新入库表和配方记录表
        /// </summary>
        /// <param name="sqlBill"></param>
        /// <param name="sqlFormula"></param>
        public bool InsertTable(string sqlBill, string sqlFormula, out string msg)
        {
            try
            {
                msg = "";
                g_DatabaseHelper.ExecuteNonQuery(sqlBill, Frame_Connection);
                g_DatabaseHelper.ExecuteNonQuery(sqlFormula, Base_Connection);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 删除试剂
        /// </summary>
        /// <param name="billGuid"></param>
        /// <param name="billItemGuid"></param>
        /// <param name="FormulaCode"></param>
        /// <param name="ReagentGuid"></param>
        /// <param name="DetailGuid"></param>
        public void deleteFormula(string billGuid, string billItemGuid, string FormulaCode, string ReagentGuid, string DetailGuid)
        {
            try
            {
                string sqlBill = string.Format(@"delete [dbo].[TB_OMMP_ReagentInBillItem] where RowGuid='{0}';
                                             delete [dbo].[TB_OMMP_ReagentInItemDetail] where RowGuid='{1}';
                                             delete [dbo].[TB_OMMP_ReagentInBill] where RowGuid='{2}';
                                             delete [dbo].[TB_OMMP_Reagent] where Code='{3}'", billItemGuid, DetailGuid, billGuid, FormulaCode);
                string sqlFormula = string.Format(@"delete  [dbo].[TB_ReagentFormulaInstance]  where FixCode='{0}'", FormulaCode);
                g_DatabaseHelper.ExecuteNonQuery(sqlBill, Frame_Connection);
                g_DatabaseHelper.ExecuteNonQuery(sqlFormula, Base_Connection);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 获取试剂配方
        /// </summary>
        /// <returns></returns>
        public DataTable getFormulaInstance()
        {
            try
            {
                string sql = string.Format(@"
                                    select a.ReagentGuid,b.ChemistryName,c.ChemistryType,e.TypeName1,a.ParentCount,a.FixCode,e.ReagentName,e.Number,e.ConfigPeople,e.Concentration,a.ChemistryUnit,
                                            a.FixCode as FormulaCode,e.RowGuid as BillItemGuid,e.BillGuid,e.ReagentGuid, d.RowGuid,b.ApplicationIndex,e.ManufactureDate,e.NumberNew,e.NumberUnit from [dbo].[TB_ReagentFormulaInstance] as a  left join dbo.TB_Chemistry as b  
                                                    on a.ChemistryGuid=b.RowGuid
                                                    left join [dbo].[TB_ChemistryType] as c
                                                    on b.[ChemistryTypeGuid]=c.[RowGuid]
                                                    left join  [EQMS_Framework].[dbo].[TB_OMMP_ReagentInItemDetail] as d
                                                    on a.FixCode=d.FixCode
                                                    left join [EQMS_Framework].[dbo].[TB_OMMP_ReagentInBillItem] as e
                                                    on d.BillItemGuid=e.RowGuid
                                                    where e.ReagentType='标液'
order by e.ManufactureDate  desc
                                                    ");
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, Base_Connection);
                string[] dtTmp = dt.AsEnumerable().Select(d => d.Field<string>("FixCode")).Distinct().ToArray();
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("FormulaCode", typeof(string));
                dtNew.Columns.Add("BillItemGuid", typeof(string));
                dtNew.Columns.Add("BillGuid", typeof(string));
                dtNew.Columns.Add("ReagentGuid", typeof(string));
                dtNew.Columns.Add("RowGuid", typeof(string));
                dtNew.Columns.Add("TypeName1", typeof(string));
                dtNew.Columns.Add("ReagentName", typeof(string));
                dtNew.Columns.Add("Chemistry", typeof(string));
                dtNew.Columns.Add("Concentration", typeof(string));
                dtNew.Columns.Add("ConfigPeople", typeof(string));
                dtNew.Columns.Add("ConfigAmount", typeof(string));
                dtNew.Columns.Add("ApplicationIndex", typeof(string));
                dtNew.Columns.Add("ManufactureDate", typeof(string));
                dtNew.Columns.Add("NumberNew", typeof(string));
                foreach (string item in dtTmp)
                {
                    DataRow[] dr = dt.Select("FixCode='" + item + "'");
                    if (dr.Length > 0)
                    {
                        DataRow drNew = dtNew.NewRow();
                        drNew["FormulaCode"] = dr[0]["FormulaCode"];
                        drNew["BillItemGuid"] = dr[0]["BillItemGuid"];
                        drNew["BillGuid"] = dr[0]["BillGuid"];
                        drNew["ReagentGuid"] = dr[0]["ReagentGuid"];
                        drNew["RowGuid"] = dr[0]["RowGuid"];
                        drNew["TypeName1"] = dr[0]["TypeName1"];
                        drNew["ReagentName"] = dr[0]["ReagentName"];
                        drNew["Concentration"] = dr[0]["Concentration"];
                        drNew["ConfigPeople"] = dr[0]["ConfigPeople"];
                        drNew["ConfigAmount"] = (dr[0]["Number"] == DBNull.Value ? "" : dr[0]["Number"].ToString()) + (dr[0]["NumberUnit"] == DBNull.Value ? "" : dr[0]["NumberUnit"].ToString());
                        drNew["ApplicationIndex"] = dr[0]["ApplicationIndex"];
                        drNew["ManufactureDate"] = dr[0]["ManufactureDate"];
                        drNew["NumberNew"] = dr[0]["NumberNew"];
                        string Chemistry = "";
                        for (int i = 0; i < dr.Length; i++)
                        {
                            if (i < dr.Length - 1)
                            {
                                Chemistry += "化学品类型：" + dr[i]["ChemistryType"].ToString() + ";化学品名称：" + dr[i]["ChemistryName"].ToString() + ";使用量：" + dr[i]["ParentCount"].ToString() + dr[i]["ChemistryUnit"].ToString() + "<br />";
                            }
                            else
                            {
                                Chemistry += "化学品类型：" + dr[i]["ChemistryType"].ToString() + ";化学品名称：" + dr[i]["ChemistryName"].ToString() + ";使用量：" + dr[i]["ParentCount"].ToString() + dr[i]["ChemistryUnit"].ToString();
                            }
                        }
                        drNew["Chemistry"] = Chemistry;
                        dtNew.Rows.Add(drNew);
                    }
                }
                return dtNew;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取试剂类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetReagentType()
        {
            try
            {
                string sql = string.Format(@"SELECT [RowGuid],[TypeName1]
                                              FROM {0}
                                              where RowStatus=1 ", "TB_OMMP_ReagentInBillItem");
                return g_DatabaseHelper.ExecuteDataTable(sql, Frame_Connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //删除数据
        public bool DeleteData(string sql)
        {
            try
            {
                g_DatabaseHelper.ExecuteNonQuery(sql, Frame_Connection);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 添加有证物质
        /// </summary>
        /// <param name="sqlBillItem"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddItem(string sqlBillItem, out string msg)
        {
            try
            {
                msg = "";
                g_DatabaseHelper.ExecuteNonQuery(sqlBillItem, Frame_Connection);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }

        public string GetDefaultPerson()
        {
            try
            {
                string sql = "select ItemText from [EQMS_Framework].[dbo].[V_CodeMainItem] where CodeName = '试剂配置默认人'";
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, Frame_Connection);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetProductSN()
        {
            try
            {
                string sql = "select ProductSN from EQMS_Framework.dbo.TB_OMMP_ReagentInBillItem order by ProductSN desc";
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, Frame_Connection);
                return dt.Rows[0]["ProductSN"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertNullData(Guid BillItemGuid, string SysNum)
        {
            try
            {
                string sql = string.Format(@"insert into EQMS_Framework.dbo.TB_OMMP_ReagentInBillItem (RowGuid, ProductSN) Values('{0}','{1}') ", BillItemGuid, SysNum);
                g_DatabaseHelper.ExecuteNonQuery(sql, Frame_Connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Guid GetGuid(string SysNum)
        {
            try
            {
                string sql = string.Format(@"select RowGuid from EQMS_Framework.dbo.TB_OMMP_ReagentInBillItem where ProductSN='{0}'", SysNum);
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, Frame_Connection);
                return Guid.Parse(dt.Rows[0]["RowGuid"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
