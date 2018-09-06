using SmartEP.Data.SqlServer.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water
{
    /// <summary>
    /// 名称：WaterRemoteControlService.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-11-4
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：水远程控制
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterRemoteControlService
    {
        RemoteControlDAL m_RemoteControlDAL = new RemoteControlDAL();

        /// <summary>
        /// 获取远程命令类型
        /// </summary>
        /// <returns></returns>
        public DataView GetCommandType()
        {
            return m_RemoteControlDAL.GetCommandType();
        }

        /// <summary>
        /// 获取远程控制命令
        /// </summary>
        /// <returns></returns>
        public DataView GetCommandList()
        {
            return m_RemoteControlDAL.GetCommandList();
        }
    }
}
