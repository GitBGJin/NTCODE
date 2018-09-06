﻿using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace highchart_export_module_asp_net
{
    public class DAL
    {
        #region <<变量>>
        /// <summary>
        /// 获取一个日志记录器
        /// </summary>
        ILog log = LogManager.GetLogger("App.Logging");

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string AirAutoMonitorConnection = "AMS_AirAutoMonitorConnection";


        DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
        #endregion

        #region 方法
        /// <summary>
        /// 激光雷达图表数据查询
        /// </summary>
        /// <param name="Quality"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="DtStart"></param>
        /// <param name="DtEnd"></param>
        /// <returns></returns>
        public DataTable GetLadarData(string Quality, string DtStart, string DtEnd, string stch, string stchMin)
        {
            try
            {
                string sql = string.Format(@"select distinct DateTime, Height,Number from [dbo].[TB_SuperStation_jiguangleida]
                                                where DateTime<='{2}' and DateTime>='{1}' and Height<={3} and Height>={4}  and DataType='{0}'  order by Height,DateTime"
                        , Quality, DtStart, DtEnd, stch,stchMin);

                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        public DataView GetHeightData(string Quality, string DtStart, string DtEnd)
        {
            try 
            {
                string sql = string.Format(@"select DateTime,Number from [dbo].[TB_SuperStation_jiguangleida]
                                                where DateTime<='{2}' and DateTime>='{1}'   and DataType='{0}'  order by Height,DateTime"
                        , Quality, DtStart, DtEnd);

                DataView dv = g_DatabaseHelper.ExecuteDataView(sql, AirAutoMonitorConnection);
                return dv;
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        #endregion
    }
}