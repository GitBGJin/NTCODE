using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Dictionary
{
    /// <summary>
    /// 名称：StandardSolutionConfigService.cs
    /// 创建人：吕云
    /// 创建日期：2016-09-05
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：水质自动巡检标液配置记录表仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class StandardSolutionConfigRepository
    {
        StandardSolutionConfigDAL m_StandardSolutionConfigDAL = new StandardSolutionConfigDAL();
        public DataTable GetData(DateTime staDate, DateTime endDate)
        {
            return m_StandardSolutionConfigDAL.GetData(staDate, endDate);
        }

        public DataTable GetData(DateTime staDate, DateTime endDate, string typeName1, string productSN, double dc_begin, double dc_end)
        {
            return m_StandardSolutionConfigDAL.GetData(staDate, endDate, typeName1, productSN, dc_begin, dc_end);
        }

        public DataTable GetProductSN()
        {
            return m_StandardSolutionConfigDAL.GetProductSN();
        }

        public DataTable GetTypeName1()
        {
            return m_StandardSolutionConfigDAL.GetTypeName1();
        }

        public DataTable GetProductSN(string TypeName1)
        {
            return m_StandardSolutionConfigDAL.GetProductSN(TypeName1);
        }
    }
}
