﻿using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.QualityControlOperation.Air;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Air
{
    /// <summary>
    /// 名称：OM_DeviceRepairDetailRepository.cs
    /// 创建人：王秉晟
    /// 创建日期：2016-03-09
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 仪器检修记录明细仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class OM_DeviceRepairDetailRepository : BaseGenericRepository<MonitoringBusinessModel, OM_DeviceRepairDetailEntity>
    {
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return RetrieveCount(x => x.RowGuid.Equals(strKey)) == 0 ? false : true;
        }

        #region << ADO.NET >>
        /// <summary>
        /// 任务配置DAL
        /// </summary>
        OM_DeviceRepairDetailDAL d_OM_TaskItemConfigDAL = Singleton<OM_DeviceRepairDetailDAL>.GetInstance();

        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere">查询条件（例如：1=1）</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return d_OM_TaskItemConfigDAL.GetList(strWhere);
        }
        #endregion
    }
}
