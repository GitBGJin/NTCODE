using SmartEP.BaseInfoRepository.Dictionary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：StandardSolutionConfigService.cs
    /// 创建人：吕云
    /// 创建日期：2016-09-05
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：水质自动巡检标液配置记录表服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class StandardSolutionConfigService
    {
        StandardSolutionConfigRepository m_StandardSolutionConfigRepository = new StandardSolutionConfigRepository();
        /// <summary>
        /// 显示标准溶液配置记录
        /// </summary>
        /// <param name="staDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="factors">因子</param>
        /// <returns></returns>
        public DataTable GetData(DateTime staDate, DateTime endDate)
        {
            DataTable dt = m_StandardSolutionConfigRepository.GetData(staDate, endDate);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["NumberUnit"] != DBNull.Value)
                {
                    if (dt.Rows[i]["NumberUnit"].ToString() == "L")
                    {
                        dt.Rows[i]["Number"] = decimal.Parse(dt.Rows[i]["Number"].ToString())*1000;
                        dt.Rows[i]["NumberLast"] = decimal.Parse(dt.Rows[i]["NumberLast"].ToString()) * 1000;
                    }
                }
                if (dt.Rows[i]["Unit"] != DBNull.Value)
                {
                    if (dt.Rows[i]["Unit"].ToString() == "ug/L")
                    {
                        dt.Rows[i]["Concentration"] = decimal.Parse(dt.Rows[i]["Concentration"].ToString()) / 1000;
                        dt.Rows[i]["DiluteConcen"] = decimal.Parse(dt.Rows[i]["DiluteConcen"].ToString()) / 1000;
                    }
                }
            }
            return dt;

        }

        public DataTable GetData(DateTime staDate, DateTime endDate, string typeName1, string productSN, double dc_begin, double dc_end)
        {
            DataTable dt = m_StandardSolutionConfigRepository.GetData(staDate, endDate, typeName1, productSN, dc_begin, dc_end);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["ProductSN"].ToString().Contains("&nbsp"))
            //    {
            //        dt.Rows[i]["ProductSN"] = "";
            //    }
            //}
            return dt;
        }

        public DataTable GetProductSN()
        {
            return m_StandardSolutionConfigRepository.GetProductSN();
        }

        public DataTable GetTypeName1()
        {
            return m_StandardSolutionConfigRepository.GetTypeName1();
        }

        public DataTable GetProductSN(string TypeName1)
        {
            DataTable dt = m_StandardSolutionConfigRepository.GetProductSN(TypeName1);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["ProductSN"].ToString().Contains("&nbsp"))
            //    {
            //        dt.Rows[i]["ProductSN"] = "";
            //    }
            //}
            return dt;
        }
    }
}
