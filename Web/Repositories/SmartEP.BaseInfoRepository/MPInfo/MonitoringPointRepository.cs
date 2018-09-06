using log4net;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.Data.SqlServer.Common.WebControl;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.MPInfo
{
    /// <summary>
    /// 名称：MonitoringPointRepository.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-05-24
    /// 功能摘要：
    /// 空气站点信息基础服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MonitoringPointRepository : BaseGenericRepository<BaseDataModel, MonitoringPointEntity>
    {
        //获取一个日志记录器
        ILog log = LogManager.GetLogger("FileLogging");

        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.MonitoringPointUid.Equals(strKey)).Count() == 0 ? false : true;
        }


        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        MonitoringPointDAL m_MonitoringPointDAC = new MonitoringPointDAL();
        CbxRsmControlDAL g_CbxRsmControlDAL = new CbxRsmControlDAL();
        /// <summary>
        /// 根据点位ID获取点位名称信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetPointNameByID(string pointid)
        {
            return m_MonitoringPointDAC.GetPointNameByID(pointid);
        }
        /// <summary>
        /// 取得自定义控件返回值
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="rsmType"></param>
        /// <param name="pointType"></param>
        /// <param name="defaultStrList"></param>
        /// <param name="notIn"></param>
        /// <param name="userGuid"></param>
        /// <param name="IsCheckAll"></param>
        /// <param name="isAllSel"></param>
        /// <returns></returns>
        public DataView GetRsmData(ApplicationType applicationType, CbxRsmType rsmType, RsmPointMode pointType, String defaultStrList, String notIn, String userGuid, Boolean IsCheckAll = false, bool isAllSel = false)
        {
            return g_CbxRsmControlDAL.GetRsmData(applicationType, rsmType, pointType, "", "", userGuid, IsCheckAll = false, isAllSel = false);
        }

        /// <summary>
        /// 根据所选站点ID获取相应区域信息
        /// </summary>
        /// <param name="regionUid"></param>
        /// <returns></returns>
        public DataView GetRegionByPointId(string[] pointIds)
        {
            try
            {
                return m_MonitoringPointDAC.GetRegionByPointId(pointIds);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 根据区域名获取相应站点ID
        /// </summary>
        /// <param name="regionUid"></param>
        /// <returns></returns>
        public DataTable GetPointIdByCityName(string CityName)
        {
            try
            {
                return m_MonitoringPointDAC.GetPointIdByCityName(CityName);
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
