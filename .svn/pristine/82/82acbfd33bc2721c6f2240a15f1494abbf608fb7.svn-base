using log4net;
using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.AutoMonitoring;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.AMSRepository.Air
{
    /// <summary>
    /// 名称：SuperStation_lijingpuRepository.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-05-13
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-05-19
    /// 功能摘要：
    /// 粒径谱数据仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SuperStation_lijingpuRepository
    {
        #region << ADO.NET >>
        /// <summary>
        /// 粒径谱数据DAL
        /// </summary>
        SuperStation_lijingpuDAL d_SuperStation_lijingpuDAL = Singleton<SuperStation_lijingpuDAL>.GetInstance();
        //获取一个日志记录器
        ILog log = LogManager.GetLogger("FileLogging");

        /// <summary>
        /// 获取粒径谱数据
        /// </summary>
        /// <param name="pointId">站点id</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="datatype">数据类型（大粒径，小粒径）</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetDataList(string pointId, DateTime dtStart, DateTime dtEnd, string datatype, string orderBy)
        {
            return d_SuperStation_lijingpuDAL.GetDataList(pointId, dtStart, dtEnd, datatype, orderBy);
        }
        /// <summary>
        /// 根据选择站点数组获取粒径谱数据
        /// </summary>
        /// <param name="datatype">数据类型</param>
        /// <param name="pointId">站点id数组</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetDataList(string datatype,string[] pointId, DateTime dtStart, DateTime dtEnd, string orderBy)
        {
            return d_SuperStation_lijingpuDAL.GetDataList(datatype,pointId, dtStart, dtEnd, orderBy);
        }
        /// <summary>
        /// 根据选中站点数组获取粒径谱数据(需转换日期)
        /// </summary>
        /// <param name="datatype">数据类型</param>
        /// <param name="pointId">站点Id数组</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetDataList(string datatype, string[] pointId, int tmBegin, int tmEnd, int tmFrom, int tmTo)
        {
            try
            {
                return d_SuperStation_lijingpuDAL.GetDataList(datatype, pointId, tmBegin, tmEnd, tmFrom, tmTo);
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
        /// <param name="pointId">站点id</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="datatype">数据类型（大粒径，小粒径）</param>
        /// <param name="num">第几条记录</param>
        /// <returns></returns>
        public DataView GetOneData(string pointId, DateTime dtStart, DateTime dtEnd, string datatype, string num)
        {
            return d_SuperStation_lijingpuDAL.GetOneData(pointId, dtStart, dtEnd, datatype, num);
        }
        /// <summary>
        /// 根据选中站点获取粒径谱一条数据
        /// </summary>
        /// <param name="pointId">站点id</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="num">第几条记录</param>
        /// <returns></returns>
        public DataView GetOneData(string type,string pointId, DateTime dtStart, DateTime dtEnd, string num)
        {
            return d_SuperStation_lijingpuDAL.GetOneData(type,pointId, dtStart, dtEnd, num);
        }

        public DataView GetAllData(string pointId, DateTime dtStart, DateTime dtEnd, string datatype, string num)
        {
            return d_SuperStation_lijingpuDAL.GetAllData(pointId, dtStart, dtEnd, datatype, num);
        }

        /// <summary>
        /// 获取所配置的粒径
        /// </summary>
        /// <returns></returns>
        public DataView getLiJingConfig()
        {
            try
            {
                var dv = d_SuperStation_lijingpuDAL.getLiJingConfig();
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
                return d_SuperStation_lijingpuDAL.GetOneData(type, pointId, dtB, dtE, dtF, dtT, num);
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
