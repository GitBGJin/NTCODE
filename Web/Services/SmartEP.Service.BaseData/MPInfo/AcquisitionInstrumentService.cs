using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.BaseInfoRepository.Channel;
using SmartEP.Core.Enums;
using SmartEP.BaseInfoRepository.MPInfo;

namespace SmartEP.Service.BaseData.MPInfo
{
    /// <summary>

    /// 名称：InstrumentService.cs

    /// 创建人：邱奇

    /// 创建日期：2015-08-18

    /// 维护人员：

    /// 最新维护人员：

    /// 最新维护日期：

    /// 功能摘要：提供数采仪信息服务

    /// 版权所有(C)：江苏远大信息股份有限公司

    /// </summary>
    public class AcquisitionInstrumentService
    {
        private AcquisitionInstrumentRepository g_Repository = new AcquisitionInstrumentRepository();
        #region 增删改
        /// <summary>
        /// 增加数采仪
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Add(AcquisitionInstrumentEntity acquisition)
        {
            g_Repository.Add(acquisition);
        }

        /// <summary>
        /// 更新数采仪
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Update(AcquisitionInstrumentEntity acquisition)
        {
            g_Repository.Update(acquisition);
        }

        /// <summary>
        /// 删除数采仪
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Delete(AcquisitionInstrumentEntity acquisition)
        {
            g_Repository.Delete(acquisition);
        }
        #endregion

        /// <summary>
        /// 是否存在数采仪名称
        /// </summary>
        /// <param name="acqusitionName"></param>
        /// <returns></returns>
        public bool IsExistByName(string acqusitionName)
        {
             return g_Repository.Retrieve(x => x.AcquisitionName.Equals(acqusitionName)).Count() == 0 ? false : true;
        }

        /// <summary>
        /// 是否存在数采仪MN
        /// </summary>
        /// <param name="acqusitionName"></param>
        /// <returns></returns>
        public bool IsExistByMN(string mn)
        {
            return g_Repository.Retrieve(x => x.MN.Equals(mn)).Count() == 0 ? false : true;
        }

        /// <summary>
        /// 获取所有数采仪列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<AcquisitionInstrumentEntity> RetrieveList()
        {
            IQueryable<AcquisitionInstrumentEntity> acquisition = g_Repository.RetrieveAll();
            return acquisition;
        }

        /// <summary>
        /// 根据数采仪Uid获取数采仪
        /// </summary>
        /// <param name="AcquisitionUid"></param>
        /// <returns></returns>
        public IQueryable<AcquisitionInstrumentEntity> RetrieveEntityByAcquisitionUid(string acquisitionUid)
        {
            return g_Repository.Retrieve(p => p.AcquisitionUid == acquisitionUid);
        }

        /// <summary>
        /// 根据点位Uid获取数采仪
        /// </summary>
        /// <param name="MonitoringPointUid"></param>
        /// <returns></returns>
        public AcquisitionInstrumentEntity RetrieveEntityByMonitoringPointUid(string monitoringPointUid)
        {
            return g_Repository.RetrieveFirstOrDefault(p => p.MonitoringPointUid == monitoringPointUid);
        }

        /// <summary>
        /// 根据MN号获取数采仪
        /// </summary>
        /// <param name="MonitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<AcquisitionInstrumentEntity> RetrieveEntityByMN(string mn)
        {
            return g_Repository.Retrieve(p => p.MN == mn);
        }

    }
}
