using SmartEP.Data.SqlServer.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Air
{
    /// <summary>
    /// 名称：WaterRemoteControlService.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-11-4
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：空气远程控制
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AirRemoteControlService
    {
        RemoteControlDAL m_RemoteControlDAL = new RemoteControlDAL();

        /// <summary>
        /// 获取远程命令类型
        /// </summary>
        /// <returns></returns>
        public DataView GetCommandType()
        {
            string ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
            return m_RemoteControlDAL.GetCommandType(ApplicationUid);
        }

        /// <summary>
        /// 获取远程控制命令
        /// </summary>
        /// <returns></returns>
        public DataView GetCommandList()
        {
            string ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
            return m_RemoteControlDAL.GetCommandList(ApplicationUid);
        }
    }
}
