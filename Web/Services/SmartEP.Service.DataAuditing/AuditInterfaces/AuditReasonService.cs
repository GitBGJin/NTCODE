﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.DomainModel.MonitoringBusiness;
using System.Data;
using SmartEP.Core.Enums;

namespace SmartEP.Service.DataAuditing.AuditInterfaces
{
    public class AuditReasonService
    {
        AuditReasonRepository reasonRep = new AuditReasonRepository();

        #region 增删改
        /// <summary>
        /// 增加理由
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Add(AuditReasonEntity reason)
        {
            reasonRep.Add(reason);
        }

        /// <summary>
        /// 更新理由
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Update(AuditReasonEntity reason)
        {
            reasonRep.Update(reason);
        }

        /// <summary>
        /// 批量删除理由
        /// </summary>
        /// <param name="ReasonGuids">主键数组</param>
        public void Delete(List<string> ReasonGuids)
        {
            IQueryable<AuditReasonEntity> query = reasonRep.Retrieve(x => ReasonGuids.Contains(x.ReasonGuid));
            reasonRep.BatchDelete(query.ToList<AuditReasonEntity>());
        }
        #endregion

        #region 获取审核理由列表
        /// <summary>
        /// 获取设备Uid下的所有审核理由
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <returns></returns>
        public IQueryable<AuditReasonEntity> GetAuditReasonList(string ApplicationUid)
        {
            return reasonRep.Retrieve(x => x.ApplicationUid == ApplicationUid);
        }
        /// <summary>
        /// 获取设备Uid下的所有审核理由
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <returns></returns>
        public IQueryable<AuditReasonEntity> GetAuditReasonList(string ApplicationUid, string AuditOperatorUid)
        {
            return reasonRep.Retrieve(x => x.ApplicationUid == ApplicationUid && x.Description.Contains(AuditOperatorUid));
        }
        /// <summary>
        /// 获取指定主键的审核理由数据
        /// </summary>
        /// <param name="ReasonGuid"></param>
        /// <returns></returns>
        public IQueryable<AuditReasonEntity> GetAuditReason(string ReasonGuid)
        {
            return reasonRep.Retrieve(x => x.ReasonGuid == ReasonGuid);
        }

        /// <summary>
        /// 生成审核原因数据
        /// </summary>
        /// <param name="applicationType">设备类型Uid</param>
        /// <param name="KeyWords">关键字</param>
        /// <returns></returns>
        public DataTable GetAuditReasonData(ApplicationType applicationType, string KeyWords = null)
        {
            return reasonRep.GetAuditReasonData(applicationType, KeyWords);
        }
        #endregion


    }
}
