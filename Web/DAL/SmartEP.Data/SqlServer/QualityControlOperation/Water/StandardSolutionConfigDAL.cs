using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：StandardSolutionConfigDAL.cs
    /// 创建人：吕云
    /// 创建日期：2016-09-05
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 标准溶液配置记录表数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>

    public class StandardSolutionConfigDAL
    {
        /// <summary>
        /// 数据库出库类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = new DatabaseHelper();

        /// <summary>
        /// 连接字符串
        /// </summary>
        private string connection = "Frame_Connection";

        /// <summary>
        /// 显示标准溶液配置记录
        /// </summary>
        /// <param name="staDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="factors">因子</param>
        /// <returns></returns>
        public DataTable GetData(DateTime staDate, DateTime endDate)
        {
            try
            {
                    string sql = string.Format(@"  select [ManufactureDate] 
                                            ,[TypeName1] 
                                            ,[MiddleProductSN]
                                            ,[DiluteConcen] 
                                            ,[ProductSN]
                                            ,[Unit]
                                            ,[Concentration]
                                            ,[Number]
                                            ,convert(FLOAT,[Number])*convert(int,[Multiple])/1000 NumberLast
                                            ,[NumberUnit]
                                            ,[ConfigPeople]
                                            ,[Description]
                                            FROM [EQMS_Framework].[dbo].[TB_OMMP_ReagentInBillItem] 
                                            where TypeName='标准溶液'
                                            and [RecordFlag]=1
                                            and ManufactureDate>='{0}' and ManufactureDate<='{1}'"
                        , staDate, endDate);
                    return g_DatabaseHelper.ExecuteDataTable(sql, connection);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetData(DateTime staDate, DateTime endDate, string typeName1, string productSN, double dc_begin, double dc_end)
        {
            try
            {
                string[] TypeName1s = typeName1.Trim(',').Split(',');
                string TypeName1 = "";
                for (int i = 0; i < TypeName1s.Length; i++)
                {
                    TypeName1 += "'" + TypeName1s[i] + "'";
                    if (i != TypeName1s.Length - 1)
                    {
                        TypeName1 += ",";
                    }
                }

                string[] ProductSNs = productSN.Trim(',').Split(',');
                string ProductSN = "";
                for (int i = 0; i < ProductSNs.Length; i++)
                {
                    ProductSN += "'" + ProductSNs[i] + "'";
                    if (i != ProductSNs.Length - 1)
                    {
                        ProductSN += ",";
                    }
                }

                //if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                //{
                //    portIdsStr = " AND PointId ='" + portIdsStr + "'";
                //}
                //else if (!string.IsNullOrEmpty(portIdsStr))
                //{
                //    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                //}

                string ProductSNstr = "" ;
                if (string.IsNullOrEmpty(ProductSN))
                {
                    ProductSNstr = "and ProductSN='" + ProductSN + "'";
                }
                else
                {
                    ProductSNstr = "and ProductSN IN(" + ProductSN + ")";
                }
                string sql = string.Format(@"  select [ManufactureDate] 
                                            ,[ReagentName]
                                            ,[TypeName1] 
                                            ,[MiddleProductSN]
                                            ,[DiluteConcen] 
                                            ,[ProductSN]
                                            ,[Unit]
                                            ,[Concentration]
                                            ,[Number]
                                            ,convert(FLOAT,[Number])*convert(FLOAT,[Multiple])/1000 NumberLast
                                            ,[NumberUnit]
                                            ,[ConfigPeople]
                                            ,[Description]
                                            FROM [EQMS_Framework].[dbo].[TB_OMMP_ReagentInBillItem] 
                                            where TypeName='标准溶液'
                                            and [RecordFlag]=1
                                            and ManufactureDate>='{0}' and ManufactureDate<='{1}'
                                            and TypeName1 IN ({2})
                                            and ProductSN IN ({3})
                                            and CONVERT (decimal(19,2),DiluteConcen )>={4}
                                            and cast(Concentration as decimal(19,2))<={5}"
                    , staDate, endDate, TypeName1, ProductSN, dc_begin, dc_end);
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProductSN()
        {
            try
            {
                string sql = "select Distinct([ProductSN]) FROM [EQMS_Framework].[dbo].[TB_OMMP_ReagentInBillItem] where TypeName='标准溶液' and [RecordFlag]=1";
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProductSN(string typeName1)
        {
            try
            {
                string[] TypeName1s = typeName1.Trim(',').Split(',');
                string TypeName1 = "";
                for (int i = 0; i < TypeName1s.Length; i++)
                {
                    TypeName1 += "'" + TypeName1s[i] + "'";
                    if (i != TypeName1s.Length - 1)
                    {
                        TypeName1 += ",";
                    }
                }
                string sql = "select Distinct([ProductSN]) FROM [EQMS_Framework].[dbo].[TB_OMMP_ReagentInBillItem] where TypeName='标准溶液' and [RecordFlag]=1 and TypeName1 IN (" + TypeName1 + ")";
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTypeName1()
        {
            try
            {
                string sql = "select Distinct([TypeName1]) FROM [EQMS_Framework].[dbo].[TB_OMMP_ReagentInBillItem] where TypeName='标准溶液' and [RecordFlag]=1";
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
