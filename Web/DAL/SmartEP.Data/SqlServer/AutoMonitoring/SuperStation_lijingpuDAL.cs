using log4net;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.AutoMonitoring
{
    /// <summary>
    /// 名称：SuperStation_lijingpuDAL.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-05-14
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2016-05-19
    /// 功能摘要：
    /// 粒径谱数据DAL层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SuperStation_lijingpuDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = string.Empty;

        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName_L = string.Empty;
        private string tableName_M = string.Empty;

        //获取一个日志记录器
        ILog log = LogManager.GetLogger("FileLogging");
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationType">应用类型（空气、地表水、噪声）</param>
        public SuperStation_lijingpuDAL()
        {
            tableName_L = "TB_SuperStation_lijingpu_L";
            tableName_M = "TB_SuperStation_lijingpu_M";
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
        }
        #endregion
        #region << 方法 >>
        /// <summary>
        /// 获取粒径谱数据
        /// </summary>
        /// <param name="pointId">站点 例：9 </param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="datatype">数据类型（大粒径，小粒径）</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetDataList(string pointId, DateTime dtStart, DateTime dtEnd, string datatype, string orderBy)
        {
            try
            {
                string tablename = string.Empty;
                if (datatype.Contains("3772L"))
                {
                    tablename = tableName_L;
                }
                else
                {
                    tablename = tableName_M;
                }
                //取得查询行转列字段拼接
                string sql = string.Empty;
                sql = string.Format("select * from {0} where PointId={1} and DateTime>='{2}' and DateTime<='{3}' order by {4}", tablename, pointId, dtStart, dtEnd, orderBy);
                return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据选择站点数组获取粒径谱数据
        /// </summary>
        /// <param name="datatype">数据类型</param>
        /// <param name="pointId">站点 例：9 </param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetDataList(string datatype,string[] pointId, DateTime dtStart, DateTime dtEnd, string orderBy)
        {
            try
            {
                string tablename = string.Empty;
                switch (datatype)
                {
                    case "Day":
                        tablename = "TB_SuperStation_lijingpu_NT_Day";
                        break;
                    case "Hour":
                        tablename = "TB_SuperStation_lijingpu_NT_Hour";
                        break;
                    case "Min1":
                        tablename = "TB_SuperStation_lijingpu_NT_Min1";
                        break;
                    case "Min5":
                        tablename = "TB_SuperStation_lijingpu_NT_Min5";
                        break;
                    case "HourOri":
                        tablename = "TB_SuperStation_lijingpu_NT_HourOri";
                        break;
                    case "DayOri":
                        tablename = "TB_SuperStation_lijingpu_NT_DayOri";
                        break;
                }

                StringBuilder sb = new StringBuilder();
                if (pointId != null && pointId.Length > 0)
                {
                    for (int i = 0; i < pointId.Length; i++)
                    {
                        sb.Append(pointId[i].ToString() + ",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }

                if (datatype == "Day" || datatype == "Hour")
                {
                    orderBy = "ReportDateTime desc";
                    string sql = string.Empty;
                    sql = string.Format("SELECT PointId,ReportDateTime,data1,data2,data3,data4,data5,data6,data7,data8,data9,data10,data11,data12,data13,data14,data15,data16,data17,data18,data19,data20,data21,data22,data23,data24,data25,data26,data27,data28,data29,data30,data31,data32,data33,data34,data35 from {0} where PointId in ({1}) and ReportDateTime>='{2}' and ReportDateTime<='{3}' order by {4}", tablename, sb, dtStart, dtEnd, orderBy);
                    return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
                }
                if (datatype == "DayOri" || datatype == "HourOri" || datatype == "Min5" || datatype == "Min1")
                {
                    orderBy = "ReciveDateTime desc";
                    string sql = string.Empty;
                    sql = string.Format("SELECT PointId,ReciveDateTime,data1,data2,data3,data4,data5,data6,data7,data8,data9,data10,data11,data12,data13,data14,data15,data16,data17,data18,data19,data20,data21,data22,data23,data24,data25,data26,data27,data28,data29,data30,data31,data32,data33,data34,data35 from {0} where PointId in ({1}) and ReciveDateTime>='{2}' and ReciveDateTime<='{3}' order by {4}", tablename, sb, dtStart, dtEnd, orderBy);
                    return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 转换日期后获取数据
        /// </summary>
        /// <param name="Type">数据类型</param>
        /// <param name="pointId">测点</param>
        /// <param name="tmBegin">日期</param>
        /// <param name="tmEnd">日期</param>
        /// <param name="tmFrom">日期</param>
        /// <param name="tmTo">日期</param>
        /// <returns></returns>
        public DataView GetDataList(string Type, string[] pointId, int tmBegin, int tmEnd, int tmFrom, int tmTo)
        {
            //转换测点数组
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < pointId.Length; i++)
                {
                    sb.Append("'" + pointId[i] + "'" + ",");
                }
                sb.Remove(sb.Length - 1, 1);

                if (Type == "Month")
                {
                    string sql = string.Format("SELECT PointId,Year,MonthOfYear,data1,data2,data3,data4,data5,data6,data7,data8,data9,data10,data11,data12,data13,data14,data15,data16,data17,data18,data19,data20,data21,data22,data23,data24,data25,data26,data27,data28,data29,data30,data31,data32,data33,data34,data35 FROM TB_SuperStation_lijingpu_NT_Month WHERE ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01' AND PointId IN ({4})", tmBegin, tmFrom, tmEnd, tmTo, sb);
                    DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                    return dv;
                }
                if (Type == "MonthOri")
                {
                    string sql = string.Format("SELECT PointId,Year,MonthOfYear,data1,data2,data3,data4,data5,data6,data7,data8,data9,data10,data11,data12,data13,data14,data15,data16,data17,data18,data19,data20,data21,data22,data23,data24,data25,data26,data27,data28,data29,data30,data31,data32,data33,data34,data35 FROM TB_SuperStation_lijingpu_NT_MonthOri WHERE ReciveDateTime>='{0}-{1}-01' and ReciveDateTime<='{2}-{3}-01' AND PointId IN ({4})", tmBegin, tmFrom, tmEnd, tmTo, sb);
                    DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                    return dv;
                }
                if (Type == "Season")
                {
                    int seasonFrom = tmBegin * 1000 + tmFrom;
                    int seasonTo = tmEnd * 1000 + tmTo;
                    string sql = string.Format("SELECT PointId,Year,SeasonOfYear,data1,data2,data3,data4,data5,data6,data7,data8,data9,data10,data11,data12,data13,data14,data15,data16,data17,data18,data19,data20,data21,data22,data23,data24,data25,data26,data27,data28,data29,data30,data31,data32,data33,data34,data35 FROM TB_SuperStation_lijingpu_NT_Season WHERE (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}  AND PointId IN ({2})", seasonFrom, seasonTo, sb);
                    DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                    return dv;
                }
                if (Type == "Week")
                {
                    int weekFrom = tmBegin * 1000 + tmFrom;
                    int weekTo = tmEnd * 1000 + tmTo;
                    string sql = string.Format("SELECT PointId,Year,WeekOfYear,data1,data2,data3,data4,data5,data6,data7,data8,data9,data10,data11,data12,data13,data14,data15,data16,data17,data18,data19,data20,data21,data22,data23,data24,data25,data26,data27,data28,data29,data30,data31,data32,data33,data34,data35 FROM TB_SuperStation_lijingpu_NT_Week WHERE (Year*1000 + WeekOfYear)>= {0} AND (Year*1000 + WeekOfYear)<={1}  AND PointId IN ({2})", weekFrom, weekTo, sb);
                    DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                    return dv;
                }
                if (Type == "Year")
                {
                    string sql = string.Format("SELECT PointId,Year,data1,data2,data3,data4,data5,data6,data7,data8,data9,data10,data11,data12,data13,data14,data15,data16,data17,data18,data19,data20,data21,data22,data23,data24,data25,data26,data27,data28,data29,data30,data31,data32,data33,data34,data35 FROM TB_SuperStation_lijingpu_NT_Year WHERE Year>='{0}' AND Year<='{1}'  AND PointId IN ({2})", tmBegin, tmEnd, sb);
                    DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                    return dv;
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取粒径谱一条数据
        /// </summary>
        /// <param name="pointId">站点 例：9 </param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="datatype">数据类型（大粒径，小粒径）</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetOneData(string pointId, DateTime dtStart, DateTime dtEnd, string datatype, string num)
        {
            try
            {
                string tablename = string.Empty;
                if (datatype.Contains("3772L"))
                {
                    tablename = tableName_L;
                }
                else
                {
                    tablename = tableName_M;
                }
                //取得查询行转列字段拼接
                string sql = string.Empty;
                sql = string.Format("select top 1 * from {0} where PointId={1} and DateTime>='{2}' and DateTime<='{3}' and id not in (select top {4} id from  {5} where PointId={6} and DateTime>='{7}' and DateTime<='{8}' order by DateTime ) order by DateTime "
                    , tablename, pointId, dtStart, dtEnd, num, tablename, pointId, dtStart, dtEnd);
                return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataView GetAllData(string pointId, DateTime dtStart, DateTime dtEnd, string datatype, string num)
        {
            try
            {
                string tablename = string.Empty;
                if (datatype.Contains("3772L"))
                {
                    tablename = tableName_L;
                }
                else
                {
                    tablename = tableName_M;
                }
                //取得查询行转列字段拼接
                string sql = string.Empty;
                sql = string.Format("select * from {0} where PointId={1} and DateTime>='{2}' and DateTime<='{3}'  order by DateTime "
                    , tablename, pointId, dtStart, dtEnd, num, tablename, pointId, dtStart, dtEnd);
                return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所配置的粒径
        /// </summary>
        /// <returns></returns>
        public DataView getLiJingConfig()
        {
            try
            {
                string conn = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
                string sql = "SELECT DataCount,DataContent FROM DT_LiJingPuConfig";
                DataView dv = g_DatabaseHelper.ExecuteDataView(sql.ToString(), conn);
                return dv;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 根据选中站点获取粒径谱数据(需转换日期)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pointId"></param>
        /// <param name="dtB"></param>
        /// <param name="dtE"></param>
        /// <param name="dtF"></param>
        /// <param name="dtT"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public DataView GetOneData(string type, string pointId, int dtB, int dtE, int dtF, int dtT, string num)
        {
            try
            {
                if (type == "Month")
                {
                    string sql = string.Empty;
                    sql = string.Format("select top 1 * from TB_SuperStation_lijingpu_NT_Month where PointId = {0} and ReportDateTime>='{1}-{3}-01' and ReportDateTime<='{2}-{4}-01' and id not in (select top {5} id from TB_SuperStation_lijingpu_NT_Month where PointId={0} and ReportDateTime>='{1}-{2}-01' and ReportDateTime<='{3}-{4}-01' order by ReportDateTime ) order by ReportDateTime "
                        , pointId, dtB, dtE, dtF, dtT, num);
                    return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
                }
                if (type == "MonthOri")
                {
                    string sql = string.Empty;
                    sql = string.Format("select top 1 * from TB_SuperStation_lijingpu_NT_MonthOri  where PointId = {0} and ReciveDateTime>='{1}-{3}-01' and ReciveDateTime<='{2}-{4}-01' and id not in (select top {5} id from TB_SuperStation_lijingpu_NT_MonthOri  where PointId={0} and ReciveDateTime>='{1}-{2}-01' and ReciveDateTime<='{3}-{4}-01' order by ReciveDateTime ) order by ReciveDateTime "
                        , pointId, dtB, dtE, dtF, dtT, num);
                    return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
                }
                if (type == "Week")
                {
                    int weekFrom = dtB * 1000 + dtF;
                    int weekTo = dtE * 1000 + dtT;
                    string sql = string.Empty;
                    sql = string.Format("select top 1 * from TB_SuperStation_lijingpu_NT_Week   where PointId = {0} and (Year*1000 + WeekOfYear)>='{1}' and (Year*1000 + WeekOfYear)<='{2}' and id not in (select top {5} id from TB_SuperStation_lijingpu_NT_Week  where PointId={0} and (Year*1000 + WeekOfYear)>='{1}' and (Year*1000 + WeekOfYear)<='{2}' order by Year,WeekOfYear ) order by Year,WeekOfYear "
                        , pointId, weekFrom, weekTo, "", "", num);
                    return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
                }
                if (type == "Season")
                {
                    int seasonFrom = dtB * 1000 + dtF;
                    int seasonTo = dtE * 1000 + dtT;
                    string sql = string.Empty;
                    sql = string.Format("select top 1 * from TB_SuperStation_lijingpu_NT_Season  where PointId = {0} and (Year*1000 + SeasonOfYear)>='{1}' and (Year*1000 + SeasonOfYear)<='{2}' and id not in (select top {5} id from TB_SuperStation_lijingpu_NT_Season  where PointId={0} and (Year*1000 + SeasonOfYear)>='{1}' and (Year*1000 + SeasonOfYear)<='{2}' order by Year,SeasonOfYear ) order by Year,SeasonOfYear "
                        , pointId, seasonFrom, seasonTo, "", "", num);
                    return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
                }
                if (type == "Year")
                {
                    string sql = string.Empty;
                    sql = string.Format("select top 1 * from TB_SuperStation_lijingpu_NT_Year  where PointId = {0} and Year>='{1}-{3}-01' and Year<='{2}-{4}-01' and id not in (select top {5} id from TB_SuperStation_lijingpu_NT_Year  where PointId={0} and Year>='{1}-{2}-01' and Year<='{3}-{4}-01' order by Year ) order by Year "
                        , pointId, dtB, dtE, "", "", num);
                    return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 根据选中站点获取粒径谱一条数据
        /// </summary>
        /// <param name="pointId">站点 例：9 </param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetOneData(string type,string pointId, DateTime dtStart, DateTime dtEnd, string num)
        {
            try
            {
                string tablename = string.Empty;
                switch (type)
                {
                    case "Day":
                        tablename = "TB_SuperStation_lijingpu_NT_Day";
                        break;
                    case "Hour":
                        tablename = "TB_SuperStation_lijingpu_NT_Hour";
                        break;
                    case "Min1":
                        tablename = "TB_SuperStation_lijingpu_NT_Min1";
                        break;
                    case "Min5":
                        tablename = "TB_SuperStation_lijingpu_NT_Min5";
                        break;
                    case "HourOri":
                        tablename = "TB_SuperStation_lijingpu_NT_HourOri";
                        break;
                    case "DayOri":
                        tablename = "TB_SuperStation_lijingpu_NT_DayOri";
                        break;
                }

                if (type == "Day" || type == "Hour")
                {
                    string sql = string.Empty;
                    sql = string.Format("select top 1 * from {0} where PointId = {1} and ReportDateTime>='{2}' and ReportDateTime<='{3}' and id not in (select top {4} id from  {5} where PointId={6} and ReportDateTime>='{7}' and ReportDateTime<='{8}' order by ReportDateTime ) order by ReportDateTime "
                        , tablename, pointId, dtStart, dtEnd, num, tablename, pointId, dtStart, dtEnd);
                    return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
                }
                else if (type == "DayOri" || type == "HourOri" || type == "Min5" || type == "Min1")
                {
                    string sql = string.Empty;
                    sql = string.Format("select top 1 * from {0} where PointId = {1} and ReciveDateTime>='{2}' and ReciveDateTime<='{3}' and id not in (select top {4} id from  {5} where PointId={6} and ReciveDateTime>='{7}' and ReciveDateTime<='{8}' order by ReciveDateTime ) order by ReciveDateTime "
                        , tablename, pointId, dtStart, dtEnd, num, tablename, pointId, dtStart, dtEnd);
                    return g_DatabaseHelper.ExecuteDataView(sql.ToString(), connection);
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        #endregion
    }
}
