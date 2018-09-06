using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Report
{
    public class jzSendMessageManage
    {
        /// <summary>
        /// POST 发送短信返回结果
        /// </summary>
        /// <param name="MobileNum">手机号，多个用,隔开</param>
        /// <param name="content">短信内容含【签名】</param>
        /// <returns></returns>
        public string PostSendMessage(string MobileNum, string content)
        {
            return jzSendMessageService.PostSendMessage(MobileNum, content);
        }

        /// <summary>
        /// POST 上行回复内容查询
        /// </summary>
        /// <returns></returns>
        public string PostReplyMessage()
        {
            return jzSendMessageService.PostReplyMessage();
        }
    }
}
