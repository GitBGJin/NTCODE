using SmartEP.AMSRepository.Air;
using SmartEP.Core.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Air
{
    /// <summary>
    /// 名称：SuperStation_lijingpu.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-05-13
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-05-31
    /// 功能摘要：
    /// 粒径谱数据服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SuperStation_lijingpuService
    {
        /// <summary>
        /// 粒径谱数据仓储层
        /// </summary>
        SuperStation_lijingpuRepository r_SuperStation_lijingpuRepository = Singleton<SuperStation_lijingpuRepository>.GetInstance();
        /// <summary>
        /// 获取粒径谱数据
        /// </summary>
        /// <param name="pointId">站点Id</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="datatype">数据类型</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetDataList(string pointId, DateTime dtStart, DateTime dtEnd, string datatype, string orderBy = "DateTime desc")
        {
            return r_SuperStation_lijingpuRepository.GetDataList(pointId, dtStart, dtEnd, datatype, orderBy);
        }
        /// <summary>
        /// 根据选中站点数组获取粒径谱数据
        /// </summary>
        /// <param name="datatype">数据类型</param>
        /// <param name="pointId">站点Id数组</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetDataList(string datatype, string[] pointId, DateTime dtStart, DateTime dtEnd, string orderBy = "DateTime desc")
        {
            return r_SuperStation_lijingpuRepository.GetDataList(datatype, pointId, dtStart, dtEnd, orderBy);
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
            return r_SuperStation_lijingpuRepository.GetDataList(datatype, pointId, tmBegin, tmEnd, tmFrom, tmTo);
        }
        /// <summary>
        /// 获取粒径谱数据
        /// </summary>
        /// <param name="pointId">站点Id</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="datatype">数据类型</param>
        /// <param name="num">排序</param>
        /// <returns></returns>
        public DataView GetOneData(string pointId, DateTime dtStart, DateTime dtEnd, string datatype, string num)
        {
            return r_SuperStation_lijingpuRepository.GetOneData(pointId, dtStart, dtEnd, datatype, num);
        }

        public DataView GetAllData(string pointId, DateTime dtStart, DateTime dtEnd, string datatype, string num)
        {
            return r_SuperStation_lijingpuRepository.GetAllData(pointId, dtStart, dtEnd, datatype, num);
        }

        /// <summary>
        /// 获取所配置的粒径
        /// </summary>
        /// <returns></returns>
        public DataView getLiJingConfig()
        {
            var dv = r_SuperStation_lijingpuRepository.getLiJingConfig();
            return dv;
        }

        /// <summary>
        /// 根据选中站点获取粒径谱数据
        /// </summary>
        /// <param name="pointId">站点Id</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="num">排序</param>
        /// <returns></returns>
        public DataView GetOneData(string type,string pointId, DateTime dtStart, DateTime dtEnd, string num)
        {
            return r_SuperStation_lijingpuRepository.GetOneData(type,pointId, dtStart, dtEnd, num);
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
            return r_SuperStation_lijingpuRepository.GetOneData(type, pointId, dtB, dtE, dtF, dtT, num);
        }
    }

}
