using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Air
{
    /// <summary>
    /// 名称：MessageTextService.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-3-13
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：短信内容管理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MessageTextService
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        MessageTextRepository g_MessageTextRepository = Singleton<MessageTextRepository>.GetInstance();
        #region 获取手机号码
        public string getTelNum(string messageType)
        {
            try
            {
                DataView dv = g_MessageTextRepository.getTelNum(messageType);
                string tel = "";
                if (dv != null)
                {
                    for (int i = 0; i < dv.Count; i++)
                    {
                        tel += dv[i]["TelNumber"].ToString() + ",";
                    }
                }
                return tel.TrimEnd(',');
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion
        #region 插入数据
        public void insertTable(string messagetype, DateTime? datetime, DateTime dtbegin, DateTime dtend, string messagetext, string daterange, string username)
        {
            g_MessageTextRepository.insertTable(messagetype, datetime, dtbegin, dtend, messagetext, daterange, username);
        }
        #endregion
        #region 获取数据
        public DataView GetMessageText(string messagetype, DateTime? datetime, DateTime dtbegin, DateTime dtend)
        {
            string strWhere = " 1=1";
            if (!string.IsNullOrWhiteSpace(messagetype))
            {
                strWhere += string.Format(" and messagetype ='{0}' ", messagetype);
            }
            if (datetime != null)
            {
                strWhere += string.Format(" and datetime ='{0}' ", datetime);
            }
            if (dtbegin != null)
            {
                strWhere += string.Format(" and begintime ='{0}' ", dtbegin);
            }
            if (dtend != null)
            {
                strWhere += string.Format(" and endtime ='{0}' ", dtend);
            }
            return g_MessageTextRepository.GetMessageText(strWhere);
        }
        public DataView GetMessageTextList(string messagetype, DateTime dtbegin, DateTime dtend)
        {
            string strWhere = " 1=1";
            if (!string.IsNullOrWhiteSpace(messagetype))
            {
                strWhere += string.Format(" and messagetype ='{0}' ", messagetype);
            }
            if (dtbegin != null)
            {
                strWhere += string.Format(" and begintime >='{0}' ", dtbegin);
            }
            if (dtend != null)
            {
                strWhere += string.Format(" and endtime <='{0}' ", dtend);
            }
            return g_MessageTextRepository.GetMessageText(strWhere);
        }
        #endregion
        #region 更新数据
        public void updatedata(string messagetype, DateTime? datetime, DateTime dtbegin, DateTime dtend, string messagetext, string username)
        {
            string strWhere = "1=1";
            if (!string.IsNullOrWhiteSpace(messagetype))
            {
                strWhere += string.Format(" and messagetype ='{0}' ", messagetype);
            }
            if (datetime != null)
            {
                strWhere += string.Format(" and datetime ='{0}' ", datetime);
            }
            if (dtbegin != null)
            {
                strWhere += string.Format(" and begintime ='{0}' ", dtbegin);
            }
            if (dtend != null)
            {
                strWhere += string.Format(" and endtime ='{0}' ", dtend);
            }
            if (strWhere != "1=1")
            {
                g_MessageTextRepository.updatedata(strWhere, messagetext, username);
            }
        }
        #endregion
        #region 更新发送状态
        public void upSendStatus(string messagetype, DateTime? datetime, DateTime dtbegin, DateTime dtend, string messagetext, string username)
        {
            string strWhere = "1=1";
            if (!string.IsNullOrWhiteSpace(messagetype))
            {
                strWhere += string.Format(" and messagetype ='{0}' ", messagetype);
            }
            if (datetime != null)
            {
                strWhere += string.Format(" and datetime ='{0}' ", datetime);
            }
            if (dtbegin != null)
            {
                strWhere += string.Format(" and begintime ='{0}' ", dtbegin);
            }
            if (dtend != null)
            {
                strWhere += string.Format(" and endtime ='{0}' ", dtend);
            }
            if (strWhere != "1=1")
            {
                g_MessageTextRepository.updatedata(strWhere, messagetext, username);
            }
        }
        #endregion
    }
}
