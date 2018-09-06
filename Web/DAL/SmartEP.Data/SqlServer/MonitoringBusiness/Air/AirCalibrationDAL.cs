using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：AirCalibrationDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-11-26
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：校准数据查询
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AirCalibrationDAL
    {
        /// <summary>
        /// 数据库出库类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = new DatabaseHelper();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_AirAutoMonitorConnection";

        /// <summary>
        /// 校准数据
        /// </summary>
        /// <param name="PointIds">测点Id</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">截止日期</param>
        /// <param name="CalTypeCodes">校准类型</param>
        /// <returns></returns>
        public DataTable GetData(List<int> PointIds, DateTime StartDate, DateTime EndDate, List<string> CalTypeCodes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT DISTINCT a.PointId,b.MonitoringPointName,convert(varchar(16),a.calTime,120) as calTime, a.calNameCode, a.calConc, a.calFlow ");
            //校准结束时间
            strSql.Append(",(SELECT TOP 1 convert(varchar(16),receiveTime,120) from QC.TB_AirCalibration where dateDiff(MINUTE,calTime,a.calTime)=0 order by receiveTime desc) as overTime ");
            //校准类型
            strSql.Append(",(case a.calTypeCode ");
            strSql.Append("when 'Z' then '自动零校准' when 'z' then '手动零校准' ");
            strSql.Append("when 'A' then '自动精密度校准' when 'a' then '手动精密度校准' ");
            strSql.Append("when 'S' then '自动跨度校准' when 's' then '手动跨度校准' ");
            strSql.Append(" end) as calTypeCode ");
            strSql.Append("FROM QC.TB_AirCalibration a ");
            strSql.Append("INNER JOIN dbo.SY_MonitoringPoint b ON a.PointId=b.PointId and b.EnableOrNot=1 ");
            strSql.Append("WHERE a.calTime >=CONVERT(datetime,'" + StartDate + "') and calTime <=CONVERT(datetime,'" + EndDate + "') ");
            if (PointIds.Count == 1)
            {
                strSql.Append("and a.PointId=CONVERT(int,'" + PointIds[0] + "') ");
            }
            else
            {
                for (int i = 0; i < PointIds.Count; i++)
                {
                    if (i == 0)
                    {
                        strSql.Append("and ( a.PointId=CONVERT(int,'" + PointIds[i] + "') ");
                    }
                    else if (i > 0 && i < PointIds.Count - 1)
                    {
                        strSql.Append("or a.PointId=CONVERT(int,'" + PointIds[i] + "') ");
                    }
                    else
                    {
                        strSql.Append("or a.PointId=CONVERT(int,'" + PointIds[i] + "') ) ");
                    }
                }
            }
            if (CalTypeCodes.Count == 1)
            {
                strSql.Append("and a.calTypeCode='" + CalTypeCodes[0] + "' ");
            }
            else
            {
                for (int i = 0; i < CalTypeCodes.Count; i++)
                {
                    if (i == 0)
                    {
                        strSql.Append("and ( a.calTypeCode='" + CalTypeCodes[i] + "' ");
                    }
                    else if (i > 0 && i < CalTypeCodes.Count - 1)
                    {
                        strSql.Append("or a.calTypeCode='" + CalTypeCodes[i] + "' ");
                    }
                    else
                    {
                        strSql.Append("or a.calTypeCode='" + CalTypeCodes[i] + "' ) ");
                    }
                }
            }
            strSql.Append("ORDER BY a.PointId,convert(varchar(16),a.calTime,120) ");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
    }
}
