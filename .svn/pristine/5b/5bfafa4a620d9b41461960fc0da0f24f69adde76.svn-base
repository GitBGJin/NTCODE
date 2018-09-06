using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository
{
    /// <summary>
    /// 名称：MessageTextRepository.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-3-13
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：短信内容管理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MessageTextRepository
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        MessageTextDAL g_MessageTextDAL = Singleton<MessageTextDAL>.GetInstance();
        #region 获得发送人员手机号
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public DataView getTelNum(string messageType)
        {
            return g_MessageTextDAL.getTelNum(messageType);
        }
        #endregion
        #region 插入数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagetype"></param>
        /// <param name="datetime"></param>
        /// <param name="dtbegin"></param>
        /// <param name="dtend"></param>
        /// <param name="messagetext"></param>
        /// <param name="daterange"></param>
        /// <param name="username"></param>
        public void insertTable(string messagetype, DateTime? datetime, DateTime dtbegin, DateTime dtend, string messagetext, string daterange, string username)
        {
            g_MessageTextDAL.insertTable(messagetype, datetime, dtbegin, dtend, messagetext, daterange, username);
        }
        #endregion
        #region 获取数据
        public DataView GetMessageText(string strWhere)
        {
            return g_MessageTextDAL.GetMessageText(strWhere);
        }
        #endregion
        #region 更新数据
        public void updatedata(string strWhere, string messagetext, string username)
        {
            g_MessageTextDAL.updatedata(strWhere, messagetext, username);
        }
        #endregion
        #region 更新发送状态
        public void upSendStatus(string strWhere, string messagetext, string username)
        {
            g_MessageTextDAL.updatedata(strWhere, messagetext, username);
        }
        #endregion
    }
}
