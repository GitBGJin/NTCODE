using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Report
{
    public class jzSendMessageService
    {
        /// <summary>
        /// POST 发送短信返回结果
        /// </summary>
        /// <param name="MobileNum">手机号，多个用,隔开</param>
        /// <param name="content">短信内容含【签名】</param>
        /// <returns></returns>
        public static string PostSendMessage(string MobileNum, string content)
        {
            string url = "http://js.ums86.com:8899/sms/Api/Send.do";
            string Spcode = System.Configuration.ConfigurationManager.AppSettings["JCSpcode"]; ;  //企业账号
            string LoginName = System.Configuration.ConfigurationManager.AppSettings["JCLoginName"]; ;  //用户名
            string Password = System.Configuration.ConfigurationManager.AppSettings["JCPassword"]; ;  //密码
            string MessageContent = content;  //短信内容
            string UserNumber = MobileNum; //手机号
            string SerialNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "001";  //流水号
            string ScheduleTime = "";  //预约发送时间
            string ExtendAccessNum = System.Configuration.ConfigurationManager.AppSettings["JCExtendAccessNum"]; ;  //接入号扩展号
            StringBuilder sb = new StringBuilder();
            sb.Append("SpCode=" + Spcode + "&LoginName=" + LoginName + "&Password=" + Password + "&MessageContent=" + MessageContent + "&UserNumber=" + UserNumber + "&SerialNumber=" + SerialNumber + "&ScheduleTime=" + ScheduleTime + "&ExtendAccessNum=" + ExtendAccessNum + "&f=1");
            byte[] bData = Encoding.GetEncoding("GBK").GetBytes(sb.ToString());
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;
            string strResult = string.Empty;
            try
            {
                hwRequest = (HttpWebRequest)WebRequest.Create(url);
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;
                Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.Default);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch
            { ;}
            return strResult;
        }

        /// <summary>
        /// POST 上行回复内容查询
        /// </summary>
        /// <returns></returns>
        public static string PostReplyMessage()
        {
            string url = "http://js.ums86.com:8899/sms/Api/reply.do";
            string Spcode = "238698";  //企业账号
            string LoginName = "sz_hjjczx";  //用户名
            string Password = "sz2015!";  //密码
            StringBuilder sb = new StringBuilder();
            sb.Append("SpCode=" + Spcode + "&LoginName=" + LoginName + "&Password=" + Password + "");
            byte[] bData = Encoding.GetEncoding("GBK").GetBytes(sb.ToString());
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;
            string strResult = string.Empty;
            try
            {
                hwRequest = (HttpWebRequest)WebRequest.Create(url);
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;
                Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.Default);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch
            { ;}
            return strResult;
        }
    }
}
