using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    public class ReagentConfigRecordDAL
    {
        /// <summary>
        /// 数据库出库类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string connection = "Frame_Connection";

        public DataTable GetData(DateTime staDate, DateTime endDate, string points, string factors)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from [EQMS_Framework].[dbo].[V_ReagentConfigRecord] where ");
            sb.Append("[ManufactureDate]>='" + staDate + "'");
            sb.Append(" and [ManufactureDate]<='" + endDate + "'");
            string[] point = points.Split(';');
            string p_item = "";
            foreach (string p in point)
            {
                p_item = p_item + "'" + p + "',";
            }
            p_item = p_item.Remove(p_item.Length - 1, 1);
            sb.Append(" and [PointName] in (" + p_item + ")");
            string[] factor = factors.Split(';');
            string f_item = "";
            foreach (string f in factor)
            {
                f_item = f_item + "'" + f + "',";
            }
            f_item = f_item.Remove(f_item.Length - 1, 1);
            sb.Append(" and [CheckFactor] in (" + f_item + ") order by ManufactureDate desc,CheckFactor,ReagentName ,PointName ");

            return g_DatabaseHelper.ExecuteDataTable(sb.ToString(), connection);
        }
    }
}
