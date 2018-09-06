using MSXML2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BaseDataModel;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Model;

namespace Service
{
    public class Send
    {
        //发送类
        BaseDataModel.BaseDataModel mSend = new BaseDataModel.BaseDataModel();
        MonitorBusinessModel mSends = new MonitorBusinessModel();
        private string m_MessageSendUrl = System.Configuration.ConfigurationManager.AppSettings["MessageSendUrl"].ToString();
        private CommonF myComm = new CommonF();
        private string messageType = "AirMessage";//页面类型
        string startTime = ConfigurationManager.AppSettings["startTime"];
        string endTime = ConfigurationManager.AppSettings["endTime"];
        /// <summary>
        /// 发送断线、数据超标、数据缺失短信方法
        /// </summary>
        public void SendMessage()
        {

            string sendMessage = string.Empty;
            //string mobile = GetTelphoneNumber(messageType);
            string mobile = System.Configuration.ConfigurationManager.AppSettings["PhoneMessage"].ToString();
            List<string> listMessage = new List<string>() { };
            string xmltext = string.Empty;
            string mail = ConfigurationManager.AppSettings["mail"];
            //发邮件测试
            //SendEmail(mail, "G20短信预警", "邮件测试：存在AQI预警数据，请注意核查！");
            //发短信测试
            //xmltext = SendSMSBySionyd("浊度仪离线 575小时0分 最近时间 2017/9/1 13:00:00", mobile);

            WriteTextLog("断线报警发送短信返回结果", xmltext, DateTime.Now);
            //foreach (string mobile in mobiles)
            //{
                try
                {

                    WriteTextLog("断线报警开始", "", DateTime.Now);
                    //sendMessage = "[环境空气报警信息]10月27日13时，渔洋山点位断线.";
                    sendMessage = getOfflineContent();
                    //sendMessage += "(测试,数据为模拟数据)";
                    //string send = "'" + sendMessage + "'";
                    WriteTextLog("断线报警内容", sendMessage, DateTime.Now);
                    if (!string.IsNullOrWhiteSpace(sendMessage))
                    {
                        //在记录表中寻找是否有同样数据
                        TB_NotifySend[] notifySends = mSend.TB_NotifySends.Where(p => (p.SendContent.ToString().Trim().Equals(sendMessage.Trim()))).ToArray();



                        if (notifySends.Count() > 0)
                        {
                            foreach (TB_NotifySend notifySendExist in notifySends)
                            {
                                if (notifySendExist.SendFinishOrNot == false)
                                {
                                    notifySendExist.SendFinishOrNot = true;
                                    mSend.SaveChanges();
                                }

                            }
                        }
                        else
                        {
                            if (DateTime.Now >= Convert.ToDateTime(startTime) && DateTime.Now <= Convert.ToDateTime(endTime))
                            {
                                //发邮件
                                //StringBuilder str = new StringBuilder();
                                //str.Append("存在断线数据，请注意核查！<br/>");
                                //str.Append("SendMessage:" + sendMessage + "<br/>");
                                //str.Append("Mobile:" + mobile + "<br/>");

                                StringBuilder str = new StringBuilder();
                                str.Append("存在断线数据，请注意核查！");
                                str.Append("SendMessage:" + sendMessage);
                                str.Append("Mobile:" + mobile);
                                //SendEmail(mail, "G20短信预警", str.ToString());

                                xmltext = SendSMSBySionyd(sendMessage, mobile);

                                WriteTextLog("断线报警发送短信返回结果", xmltext, DateTime.Now);

                                TB_NotifySend notifySend = new TB_NotifySend();
                                notifySend.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
                                notifySend.NotifySendUid = Guid.NewGuid().ToString();
                                notifySend.NotifyTypeUid = "9504ec80-e5f8-4436-9fa1-1768805ea1ca";
                                notifySend.SendUserName = "system";
                                notifySend.ReceiveUserAddresses = mobile;
                                notifySend.SendTitle = "autoOut";
                                notifySend.SendContent = sendMessage;
                                notifySend.SendDateTime = DateTime.Now;
                                notifySend.CreatDateTime = DateTime.Now;
                                if (!string.IsNullOrWhiteSpace(xmltext))
                                {
                                    notifySend.SendFinishOrNot = true;
                                }
                                else
                                {
                                    notifySend.SendFinishOrNot = false;

                                }
                                mSend.Add(notifySend);
                                mSend.SaveChanges();
                            }
                            else 
                            {
                                TB_NotifySend notifySend = new TB_NotifySend();
                                notifySend.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
                                notifySend.NotifySendUid = Guid.NewGuid().ToString();
                                notifySend.NotifyTypeUid = "9504ec80-e5f8-4436-9fa1-1768805ea1ca";
                                notifySend.SendUserName = "system";
                                notifySend.ReceiveUserAddresses = mobile;
                                notifySend.SendTitle = "autoOut";
                                notifySend.SendContent = sendMessage;
                                notifySend.SendDateTime = DateTime.Now;
                                notifySend.CreatDateTime = DateTime.Now;
                                notifySend.SendFinishOrNot = false;
                                mSend.Add(notifySend);
                                mSend.SaveChanges();
                            }
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteTextLog("断线异常", ex.ToString(), DateTime.Now);
                }
                finally { }
                //try
                //{
                //    WriteTextLog("数据异常报警开始", "", DateTime.Now);
                //    string sendTitle = "autoDif";
                //    if (DateTime.Now >= Convert.ToDateTime(startTime) && DateTime.Now <= Convert.ToDateTime(endTime))
                //    {
                //        Reissue(DateTime.Now, sendTitle);
                //    }

                //    listMessage = ExcessiveAndLost();
                //    if (listMessage != null && listMessage.Count() > 0)
                //    {
                //        foreach (string me in listMessage)
                //        {
                //            WriteTextLog("数据异常内容", me, DateTime.Now);
                //            sendMessage = me;
                //            //在记录表中寻找是否有同样数据
                //            TB_NotifySend[] notifySends = mSend.TB_NotifySends.Where(p => (p.SendContent.ToString().Trim().Equals(sendMessage.Trim())) & (p.SendTitle == "autoDif")).ToArray();
                //            if (notifySends.Count() > 0)
                //            {
                //                if (DateTime.Now >= Convert.ToDateTime(startTime) && DateTime.Now <= Convert.ToDateTime(endTime))
                //                {
                //                    foreach (TB_NotifySend notifySendExist in notifySends)
                //                    {
                //                        if (notifySendExist.SendFinishOrNot == false)
                //                        {
                //                            notifySendExist.SendFinishOrNot = true;
                //                            mSend.SaveChanges();
                //                        }

                //                    }
                //                }
                //                else
                //                {
                //                    foreach (TB_NotifySend notifySendExist in notifySends)
                //                    {
                //                        if (notifySendExist.SendFinishOrNot == true)
                //                        {
                //                            notifySendExist.SendFinishOrNot = false;
                //                            mSend.SaveChanges();
                //                        }

                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (DateTime.Now >= Convert.ToDateTime(startTime) && DateTime.Now <= Convert.ToDateTime(endTime))
                //                {
                //                    //xmltext = SendSMSBySionyd(sendMessage, mobile);

                //                    //WriteTextLog("数据异常报警发送短信返回结果", xmltext, DateTime.Now);
                //                    //发邮件
                //                    StringBuilder str = new StringBuilder();
                //                    str.Append("存在异常数据，请注意核查！<br/>");
                //                    str.Append("SendMessage:" + sendMessage + "<br/>");
                //                    str.Append("Mobile:" + mobile + "<br/>");
                //                    //SendEmail(mail, "G20短信预警", sendMessage);
                //                    //发短信
                //                    xmltext = SendSMSBySionyd(sendMessage, mobile);

                //                    WriteTextLog("数据异常报警发送短信返回结果", xmltext, DateTime.Now);

                //                    TB_NotifySend notifySend = new TB_NotifySend();
                //                    notifySend.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
                //                    notifySend.NotifySendUid = Guid.NewGuid().ToString();
                //                    notifySend.NotifyTypeUid = "9414e489-034c-46b8-893a-a7c4a5b5132f";
                //                    notifySend.SendUserName = "system";
                //                    notifySend.ReceiveUserAddresses = mobile;
                //                    notifySend.SendTitle = "autoDif";
                //                    notifySend.SendContent = sendMessage;
                //                    notifySend.SendDateTime = DateTime.Now;
                //                    notifySend.CreatDateTime = DateTime.Now;
                //                    if (!string.IsNullOrWhiteSpace(xmltext))
                //                    {
                //                        notifySend.SendFinishOrNot = true;
                //                    }
                //                    else
                //                    {
                //                        notifySend.SendFinishOrNot = false;

                //                    }

                //                    mSend.Add(notifySend);
                //                    mSend.SaveChanges();
                //                }
                //                else
                //                {
                //                    TB_NotifySend notifySend = new TB_NotifySend();
                //                    notifySend.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
                //                    notifySend.NotifySendUid = Guid.NewGuid().ToString();
                //                    notifySend.NotifyTypeUid = "9414e489-034c-46b8-893a-a7c4a5b5132f";
                //                    notifySend.SendUserName = "system";
                //                    notifySend.ReceiveUserAddresses = mobile;
                //                    notifySend.SendTitle = "autoDif";
                //                    notifySend.SendContent = sendMessage;
                //                    notifySend.SendDateTime = DateTime.Now;
                //                    notifySend.CreatDateTime = DateTime.Now;
                //                    notifySend.SendFinishOrNot = false;
                //                    mSend.Add(notifySend);
                //                    mSend.SaveChanges();
                //                }
                //            }
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    WriteTextLog("数据异常", ex.ToString(), DateTime.Now);
                //}
                //finally { }
                try
                {
                    WriteTextLog("超站仪器异常报警开始", "", DateTime.Now);
                    listMessage = InstrumentOffline();
                    if (listMessage != null && listMessage.Count() > 0)
                    {
                        foreach (string me in listMessage)
                        {
                            WriteTextLog("数据异常内容", me, DateTime.Now);
                            sendMessage = "[南通环境空气质量]" + me;
                            //在记录表中寻找是否有同样数据
                            TB_NotifySend[] notifySends = mSend.TB_NotifySends.Where(p => (p.SendContent.ToString().Trim().Equals(sendMessage.Trim())) & (p.SendTitle == "autoDio") && p.ReceiveUserAddresses.Equals(mobile)).ToArray();
                            if (notifySends.Count() > 0)
                            {
                                if (DateTime.Now >= Convert.ToDateTime(startTime) && DateTime.Now <= Convert.ToDateTime(endTime))
                                {
                                    foreach (TB_NotifySend notifySendExist in notifySends)
                                    {
                                        if (notifySendExist.SendFinishOrNot == false)
                                        {
                                            notifySendExist.SendFinishOrNot = true;
                                            mSend.SaveChanges();
                                        }

                                    }
                                }
                                else
                                {
                                    foreach (TB_NotifySend notifySendExist in notifySends)
                                    {
                                        if (notifySendExist.SendFinishOrNot == true)
                                        {
                                            notifySendExist.SendFinishOrNot = false;
                                            mSend.SaveChanges();
                                        }

                                    }
                                }
                            }
                            else
                            {
                                if (DateTime.Now >= Convert.ToDateTime(startTime) && DateTime.Now <= Convert.ToDateTime(endTime))
                                {
                                    //xmltext = SendSMSBySionyd(sendMessage, mobile);

                                    //WriteTextLog("数据异常报警发送短信返回结果", xmltext, DateTime.Now);
                                    //发邮件
                                    //StringBuilder str = new StringBuilder();
                                    //str.Append("存在异常数据，请注意核查！<br/>");
                                    //str.Append("SendMessage:" + sendMessage + "<br/>");
                                    //str.Append("Mobile:" + mobile + "<br/>");
                                    //SendEmail(mail, "G20短信预警", sendMessage);
                                    //发短信
                                    xmltext = SendSMSBySionyd(sendMessage, mobile);

                                    WriteTextLog("数据异常报警发送短信返回结果", xmltext, DateTime.Now);

                                    TB_NotifySend notifySend = new TB_NotifySend();
                                    notifySend.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
                                    notifySend.NotifySendUid = Guid.NewGuid().ToString();
                                    notifySend.NotifyTypeUid = "D0DC9177-BABE-46B8-983D-32674560C676";
                                    notifySend.SendUserName = "system";
                                    notifySend.ReceiveUserAddresses = mobile;
                                    notifySend.SendTitle = "autoDio";
                                    notifySend.SendContent = sendMessage;
                                    notifySend.SendDateTime = DateTime.Now;
                                    notifySend.CreatDateTime = DateTime.Now;
                                    if (!string.IsNullOrWhiteSpace(xmltext))
                                    {
                                        notifySend.SendFinishOrNot = true;
                                    }
                                    else
                                    {
                                        notifySend.SendFinishOrNot = false;

                                    }

                                    mSend.Add(notifySend);
                                    mSend.SaveChanges();
                                }
                                else
                                {
                                    TB_NotifySend notifySend = new TB_NotifySend();
                                    notifySend.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
                                    notifySend.NotifySendUid = Guid.NewGuid().ToString();
                                    notifySend.NotifyTypeUid = "D0DC9177-BABE-46B8-983D-32674560C676";
                                    notifySend.SendUserName = "system";
                                    notifySend.ReceiveUserAddresses = mobile;
                                    notifySend.SendTitle = "autoDio";
                                    notifySend.SendContent = sendMessage;
                                    notifySend.SendDateTime = DateTime.Now;
                                    notifySend.CreatDateTime = DateTime.Now;
                                    notifySend.SendFinishOrNot = false;
                                    mSend.Add(notifySend);
                                    mSend.SaveChanges();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteTextLog("数据异常", ex.ToString(), DateTime.Now);
                }
                finally { }
            //}
            
            //try
            //{
            //  WriteTextLog("AQI重污染报警", "", DateTime.Now);
            //  sendMessage = getHeavyPollute();
            //  //sendMessage+="(测试，数据为模拟数据)";
            //  WriteTextLog("AQI重污染报警", sendMessage, DateTime.Now);
            //  if (!string.IsNullOrWhiteSpace(sendMessage))
            //  {
            //    //在记录表中寻找是否有同样数据
            //    TB_NotifySend[] notifySends = mSend.TB_NotifySends.Where(p => (p.SendContent.Trim().Equals(sendMessage.Trim())) & (p.SendTitle == "重度污染")).ToArray();
            //    if (notifySends.Count() > 0)
            //    {
            //      foreach (TB_NotifySend notifySendExist in notifySends)
            //      {
            //        if (notifySendExist.SendFinishOrNot == false)
            //        {
            //          notifySendExist.SendFinishOrNot = true;
            //          mSend.SaveChanges();
            //        }

            //      }
            //    }
            //    else
            //    {

            //     //xmltext = SendSMSBySionyd(sendMessage, mobile);

            //      WriteTextLog("AQI重污染发送短信返回结果", xmltext, DateTime.Now);
            //      //发邮件
            //      StringBuilder str = new StringBuilder();
            //      str.Append("存在AQI重污染数据，请注意核查！<br/>");
            //      str.Append("SendMessage:" + sendMessage + "<br/>");
            //      str.Append("Mobile:" + mobile + "<br/>");

            //      SendEmail(mail, "G20短信预警", str.ToString());
            //      TB_NotifySend notifySend = new TB_NotifySend();
            //      notifySend.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
            //      notifySend.NotifySendUid = Guid.NewGuid().ToString();
            //      notifySend.NotifyTypeUid = "d6a15a87-041f-4e22-aac9-6ac53633e78d";
            //      notifySend.SendUserName = "system";
            //      notifySend.ReceiveUserAddresses = mobile;
            //      notifySend.SendTitle = "重度污染";
            //      notifySend.SendContent = sendMessage;
            //      notifySend.SendDateTime = DateTime.Now;
            //      notifySend.CreatDateTime = DateTime.Now;
            //      if (!string.IsNullOrWhiteSpace(xmltext))
            //      {
            //        notifySend.SendFinishOrNot = true;
            //      }
            //      else
            //      {
            //        notifySend.SendFinishOrNot = false;

            //      }
            //      mSend.Add(notifySend);
            //      mSend.SaveChanges();
            //    }
            //  }

            //}

            //catch (Exception ex)
            //{
            //  WriteTextLog("AQI重污染报警异常", ex.ToString(), DateTime.Now);
            //}
            //finally { }
            //string AQIIsSend = System.Configuration.ConfigurationManager.AppSettings["AQIIsSend"];
            //if (AQIIsSend == "1")
            //{
            //  SendAQIMessage();
            //}

        }
        /// <summary>
        /// 补发夜里的报警信息
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="sendTitle"></param>
        private void Reissue(DateTime dateTime, string sendTitle)
        {
            DateTime dtStart = Convert.ToDateTime(startTime);
            DateTime beginTime = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"));
            string xmltext = string.Empty;
            if ((dateTime - dtStart).Hours >= 0 && (dateTime - dtStart).Hours < 1)
            {
                TB_NotifySend[] notifySends = mSend.TB_NotifySends.Where(p => (p.SendTitle == sendTitle) & (p.CreatDateTime >= beginTime & p.CreatDateTime < dtStart)).ToArray();
                if (notifySends != null && notifySends.Count() > 0)
                {
                    foreach (TB_NotifySend notifySendExist in notifySends)
                    {
                        //xmltext = SendSMSBySionyd(notifySendExist.SendContent, notifySendExist.ReceiveUserAddresses);
                        if (!string.IsNullOrWhiteSpace(xmltext))
                        {
                            notifySendExist.SendFinishOrNot = true;
                        }
                        else
                        {
                            notifySendExist.SendFinishOrNot = false;

                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// 根据页面类型来获取发送短信的电话号码
        /// </summary>
        /// <param name="messagetype">页面的类型</param>
        /// <returns>联系人的电话号码</returns>
        private string GetTelphoneNumber(string messageType)
        {
            string sql = string.Format(@"SELECT TelNumber,EnableOrNot FROM [AMS_BaseDataSZ].[dbo].[TB_MessageTel] where PageType='{0}'", messageType);
            DataView TelDV = myComm.CreatDataView(sql, "AMS_BaseDataSZConnection");
            string phoneNumber = "";
            StringBuilder telephoneNumber = new StringBuilder("");
            if (TelDV != null)
            {
                foreach (DataRowView dr in TelDV)
                {
                    if (dr["EnableOrNot"].ToString().Equals("True"))
                    {
                        if (dr["TelNumber"].ToString() != null)
                        {
                            telephoneNumber.Append(dr["TelNumber"].ToString() + ",");
                        }
                    }
                }
            }
            if (telephoneNumber != null)
            {
                phoneNumber = telephoneNumber.ToString();
                phoneNumber = phoneNumber.TrimEnd(',');
            }
            return phoneNumber;
        }


        /// <summary>
        /// 发送AQI预警短信方法
        /// </summary>
        public void SendAQIMessage()
        {

            string sendMessage = string.Empty;
            string mobile = GetTelphoneNumber(messageType);
            string xmltext = string.Empty;
            string mail = ConfigurationManager.AppSettings["mail"];
            //发邮件测试
            SendEmail(mail, "G20短信预警", "邮件测试：存在AQI预警数据，请注意核查！");

            try
            {
                WriteTextLog("AQI预警开始", "", DateTime.Now);
                sendMessage = getAQIContent();
                //sendMessage += "(测试，数据为模拟数据)";
                WriteTextLog("AQI预警内容", sendMessage, DateTime.Now);
                if (!string.IsNullOrWhiteSpace(sendMessage))
                {
                    //在记录表中寻找是否有同样数据
                    TB_NotifySend[] notifySends = mSend.TB_NotifySends.Where(p => (p.SendContent.Trim().Equals(sendMessage.Trim())) & (p.SendTitle == "AQI预警")).ToArray();
                    if (notifySends.Count() > 0)
                    {
                        foreach (TB_NotifySend notifySendExist in notifySends)
                        {
                            if (notifySendExist.SendFinishOrNot == false)
                            {
                                notifySendExist.SendFinishOrNot = true;
                                mSend.SaveChanges();
                            }

                        }
                    }
                    else
                    {

                        xmltext = SendSMSBySionyd(sendMessage, mobile);

                        WriteTextLog("AQI预警发送短信返回结果", xmltext, DateTime.Now);
                        //发邮件
                        StringBuilder str = new StringBuilder();
                        str.Append("存在AQI预警数据，请注意核查！<br/>");
                        str.Append("SendMessage:" + sendMessage + "<br/>");
                        str.Append("Mobile:" + mobile + "<br/>");

                        SendEmail(mail, "G20短信预警", str.ToString());
                        TB_NotifySend notifySend = new TB_NotifySend();
                        notifySend.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
                        notifySend.NotifySendUid = Guid.NewGuid().ToString();
                        notifySend.NotifyTypeUid = "f1a72767-1630-450b-9ec8-d927444f669b";
                        notifySend.SendUserName = "system";
                        notifySend.ReceiveUserAddresses = mobile;
                        notifySend.SendTitle = "AQI预警";
                        notifySend.SendContent = sendMessage;
                        notifySend.SendDateTime = DateTime.Now;
                        notifySend.CreatDateTime = DateTime.Now;
                        if (!string.IsNullOrWhiteSpace(xmltext))
                        {
                            notifySend.SendFinishOrNot = true;
                        }
                        else
                        {
                            notifySend.SendFinishOrNot = false;

                        }
                        mSend.Add(notifySend);
                        mSend.SaveChanges();
                    }
                }

            }


            catch (Exception ex)
            {
                WriteTextLog("AQI预警异常", ex.ToString(), DateTime.Now);
            }
            finally { }
        }

        /// <summary>
        /// 获取AQI预警
        /// </summary>
        /// <returns></returns>
        public string getAQIContent()
        {
            try
            {
                //DateTime beginTime = Convert.ToDateTime("2016-08-15 18:00:00");
                //DateTime endTime = Convert.ToDateTime("2016-08-15 19:00:00");
                DateTime beginTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                DateTime endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //获取当前一小时内所有数据
                TB_CreatAlarm[] AQIAlarms = mSend.TB_CreatAlarms.
                    Where(p => (p.ItemCode == "RealAQI") && (p.CreatDateTime >= beginTime && p.CreatDateTime <= endTime)).
                    OrderByDescending(p => p.CreatDateTime).ToArray();

                if (AQIAlarms.Count() > 0)
                {
                    TB_CreatAlarm latestAQIAlarm = AQIAlarms[0];//最新数据
                    //前一小时数据
                    TB_CreatAlarm beforeAQIAlarm = mSend.TB_CreatAlarms.
                        Where(p => (p.ItemCode == "RealAQI") && (p.RecordDateTime.Equals(Convert.ToDateTime(latestAQIAlarm.RecordDateTime).AddHours(-1)))).FirstOrDefault();
                    if (beforeAQIAlarm != null)
                    {
                        return null;
                        //else if ((Convert.ToInt32(latestAQIAlarm.ItemValue) > 90 && Convert.ToInt32(latestAQIAlarm.ItemValue) <= 100) && (Convert.ToInt32(beforeAQIAlarm.ItemValue) > 90 && Convert.ToInt32(beforeAQIAlarm.ItemValue) <= 100))
                        //{
                        //    return null;
                        //}
                    }
                    return AQIAlarms[0].Content;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 苏州市区重污染报警
        /// </summary>
        /// <returns></returns>
        public string getHeavyPollute()
        {
            try
            {
                //DateTime beginTime = Convert.ToDateTime("2016-08-15 18:00:00");
                //DateTime endTime = Convert.ToDateTime("2016-08-15 19:00:00");
                DateTime beginTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                DateTime endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //获取当前一小时内所有数据
                TB_CreatAlarm[] AQIAlarms = mSend.TB_CreatAlarms.
                    Where(p => (p.ItemCode == "HeavyPollute") && (p.CreatDateTime >= beginTime && p.CreatDateTime <= endTime)).
                    OrderByDescending(p => p.CreatDateTime).ToArray();

                if (AQIAlarms.Count() > 0)
                {
                    TB_CreatAlarm latestAQIAlarm = AQIAlarms[0];//最新数据
                    //前一小时数据
                    TB_CreatAlarm beforeAQIAlarm = mSend.TB_CreatAlarms.
                        Where(p => (p.ItemCode == "HeavyPollute") && (p.RecordDateTime.Equals(Convert.ToDateTime(latestAQIAlarm.RecordDateTime).AddHours(-1)))).FirstOrDefault();
                    if (beforeAQIAlarm != null)
                    {
                        return null;
                        //else if ((Convert.ToInt32(latestAQIAlarm.ItemValue) > 90 && Convert.ToInt32(latestAQIAlarm.ItemValue) <= 100) && (Convert.ToInt32(beforeAQIAlarm.ItemValue) > 90 && Convert.ToInt32(beforeAQIAlarm.ItemValue) <= 100))
                        //{
                        //    return null;
                        //}
                    }
                    return AQIAlarms[0].Content;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 取得查询字段
        /// </summary>
        /// <param name="aQIDataType"></param>
        /// <returns></returns>
        private string GetFieldName()
        {
            string fieldName = @"MonitoringRegionUid
                        ,DateTime
                        ,SO2
                        ,SO2_IAQI
                        ,NO2
                        ,NO2_IAQI
                        ,PM10
                        ,PM10_IAQI
                        ,Recent24HoursPM10
                        ,Recent24HoursPM10_IAQI
                        ,CO
                        ,CO_IAQI
                        ,O3
                        ,O3_IAQI
                        ,Recent8HoursO3
                        ,Recent8HoursO3_IAQI
                        ,PM25
                        ,PM25_IAQI
                        ,Recent24HoursPM25
                        ,Recent24HoursPM25_IAQI
                        ,AQIValue
                        ,PrimaryPollutant
                        ,Range
                        ,RGBValue
                        ,PicturePath
                        ,Class
                        ,Grade
                        ,HealthEffect
                        ,TakeStep";

            return fieldName;
        }
        ///// <summary>
        ///// 获取断线预警
        ///// </summary>
        ///// <returns></returns>
        //public string getOfflineContent()
        //{
        //    try
        //    {
        //        string Content = string.Empty;
        //        string[] sContent = new string[] { };
        //        string pointContent = string.Empty;
        //        string sendContent = string.Empty;
        //        //获取当前1小时范围内的所有数据
        //        TB_CreatAlarm[] OfflineAlarms = mSend.TB_CreatAlarms.
        //            Where(p => (p.ItemCode == "Offline")
        //                && (p.CreatDateTime >= Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
        //                && p.CreatDateTime <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00")))).
        //                OrderByDescending(p => p.CreatDateTime).ToArray();

        //        //寻找所有站点
        //        string[] points = mSend.TB_CreatAlarms.
        //            Where(p => (p.ItemCode == "Offline")
        //                && (p.CreatDateTime >= Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
        //                && p.CreatDateTime <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00")))).
        //                Select(p => p.MonitoringPointUid).ToArray().Distinct().ToArray();

        //        //获取国控点
        //        //var v = from item in points
        //        //        where item == "d979dcb6-7707-4def-9a2d-be5cb5270376" || item == "261a801c-516b-4ab4-bad3-a289b80582ee"
        //        //        || item == "d820a83a-7a97-44da-82de-0ae0d96b1ba3" || item == "dd027da6-43a2-4650-9117-eca7221bab75"
        //        //        || item == "f4145a15-5bd5-4f9c-9783-99fca706e4d7"
        //        //        select item;
        //        //string yysf = ConfigurationManager.AppSettings["yysf"];
        //        //string yys = ConfigurationManager.AppSettings["yys"];
        //        //string zzyf = ConfigurationManager.AppSettings["zzyf"];
        //        //string zzy = ConfigurationManager.AppSettings["zzy"];
        //        //string fzgyf = ConfigurationManager.AppSettings["fzgyf"];
        //        //string fzgy = ConfigurationManager.AppSettings["fzgy"];
        //        //string lygdzf = ConfigurationManager.AppSettings["lygdzf"];
        //        //string lygdz = ConfigurationManager.AppSettings["lygdz"];
        //        //List<string> list = new List<string>();
        //        //if (yysf == "1")
        //        //{
        //        //    list.Add(yys);
        //        //}
        //        //if (zzyf == "1")
        //        //{
        //        //    list.Add(zzy);
        //        //}
        //        //if (fzgyf == "1")
        //        //{
        //        //    list.Add(fzgy);
        //        //}
        //        //if (lygdzf == "1")
        //        //{
        //        //    list.Add(lygdz);
        //        //}
        //        //list.Sort();
        //        //string[] newPoints = list.ToArray();

        //        //points = v.ToArray();
        //        //string[] cbPoints = new string[points.Length + newPoints.Length];
        //        //points.CopyTo(cbPoints, 0);
        //        //newPoints.CopyTo(cbPoints, points.Length);
        //        //points = cbPoints;
        //        Array.Sort(points);
        //        WriteTextLog("当前1h范围内所有数据的个数", OfflineAlarms.Count().ToString(), DateTime.Now);
        //        string dtstr = "";
        //        if (OfflineAlarms.Count() > 0)
        //        {
        //            WriteTextLog("站点", string.Join(",", points), DateTime.Now);
        //            foreach (string point in points)
        //            {
        //                //当前测点最新数据
        //                TB_CreatAlarm latestOfflineAlarm = mSend.TB_CreatAlarms.
        //                    Where(p => (p.ItemCode == "Offline") && (p.MonitoringPointUid == point)
        //                    && (p.CreatDateTime >= Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
        //                    && p.CreatDateTime <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))).
        //                    OrderByDescending(p => p.CreatDateTime).FirstOrDefault();
        //                //当前时间前一小时的最新数据
        //                TB_CreatAlarm latestBeforeOfflineAlarm = mSend.TB_CreatAlarms.Where(p => (p.ItemCode == "Offline") && (p.MonitoringPointUid == point)
        //                       && (p.CreatDateTime >= Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
        //                       && p.CreatDateTime <= Convert.ToDateTime(DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss")))).
        //                       OrderByDescending(p => p.CreatDateTime).FirstOrDefault();

        //                //当前测点前一小时数据  TB_CreatAlarm beforeAQIAlarm = mSend.TB_CreatAlarms.

        //                /*TB_CreatAlarm beforeOfflineAlarm = mSend.TB_CreatAlarms.
        //                    Where(p => (p.ItemCode == "Offline") && (p.MonitoringPointUid == point)
        //                    && (p.CreatDateTime >= Convert.ToDateTime(Convert.ToDateTime(latestOfflineAlarm.CreatDateTime).AddDays(-1).ToString("yyyy-MM-dd HH:00:00"))
        //                    && (p.CreatDateTime <= Convert.ToDateTime(Convert.ToDateTime(latestOfflineAlarm.CreatDateTime).AddSeconds(-1).ToString("yyyy-MM-dd HH:00:00"))))).
        //                    OrderByDescending(p => p.CreatDateTime).FirstOrDefault();
        //                WriteTextLog("当前测点前一小时数据", point +","+ beforeOfflineAlarm.AlarmUid, DateTime.Now);*/
        //                if (latestOfflineAlarm != null && latestBeforeOfflineAlarm != null)
        //                {
        //                    if (latestOfflineAlarm.RecordDateTime != latestBeforeOfflineAlarm.RecordDateTime)
        //                    {
        //                        //当前测点前一小时数据
        //                        TB_CreatAlarm beforeOfflineAlarm = mSend.TB_CreatAlarms.Where(p => (p.ItemCode == "Offline") && (p.MonitoringPointUid == point) && (p.RecordDateTime.Equals(Convert.ToDateTime(latestOfflineAlarm.RecordDateTime).AddHours(-1)))).OrderByDescending(p => p.CreatDateTime).FirstOrDefault();

        //                        if (beforeOfflineAlarm == null)
        //                        {
        //                            dtstr = Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).ToString("yyyy年MM月dd日HH时");
        //                            if (DateTime.Now.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day < 1)
        //                            {
        //                                string nowStr = Convert.ToDateTime(DateTime.Now.ToString()).ToString("HH时");
        //                                WriteTextLog("当前测点最新数据", point + "," + latestOfflineAlarm.AlarmUid, DateTime.Now);
        //                                Content = latestOfflineAlarm.SendContent;
        //                                WriteTextLog("当前报警信息的内容", Content, DateTime.Now);
        //                                sContent = Content.Split(new char[] { ';' });
        //                                pointContent += string.Format(@"{0}~{1}{2}离线,", dtstr, nowStr, sContent[0]);
        //                            }
        //                            else if (DateTime.Now.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day >= 1)
        //                            {
        //                                string nowStr = Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd日HH时");
        //                                WriteTextLog("当前测点最新数据", point + "," + latestOfflineAlarm.AlarmUid, DateTime.Now);
        //                                Content = latestOfflineAlarm.SendContent;
        //                                WriteTextLog("当前报警信息的内容", Content, DateTime.Now);
        //                                sContent = Content.Split(new char[] { ';' });
        //                                pointContent += string.Format(@"{0}~{1}{2}离线,", dtstr, nowStr, sContent[0]);
        //                            }
        //                        }
        //                    }
        //                }
        //                else if (latestOfflineAlarm != null && latestBeforeOfflineAlarm == null)
        //                {
        //                    //当前测点前一小时数据
        //                    TB_CreatAlarm beforeOfflineAlarm = mSend.TB_CreatAlarms.Where(p => (p.ItemCode == "Offline") && (p.MonitoringPointUid == point) && (p.RecordDateTime.Equals(Convert.ToDateTime(latestOfflineAlarm.RecordDateTime).AddHours(-1)))).OrderByDescending(p => p.CreatDateTime).FirstOrDefault();

        //                    if (beforeOfflineAlarm == null)
        //                    {
        //                        dtstr = Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).ToString("yyyy年MM月dd日HH时");
        //                        if (DateTime.Now.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day < 1)
        //                        {
        //                            string nowStr = Convert.ToDateTime(DateTime.Now.ToString()).ToString("HH时");
        //                            WriteTextLog("当前测点最新数据", point + "," + latestOfflineAlarm.AlarmUid, DateTime.Now);
        //                            Content = latestOfflineAlarm.SendContent;
        //                            WriteTextLog("当前报警信息的内容", Content, DateTime.Now);
        //                            sContent = Content.Split(new char[] { ';' });
        //                            pointContent += string.Format(@"{0}~{1}{2}离线,", dtstr, nowStr, sContent[0]);
        //                        }
        //                        else if (DateTime.Now.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day >= 1)
        //                        {
        //                            string nowStr = Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd日HH时");
        //                            WriteTextLog("当前测点最新数据", point + "," + latestOfflineAlarm.AlarmUid, DateTime.Now);
        //                            Content = latestOfflineAlarm.SendContent;
        //                            WriteTextLog("当前报警信息的内容", Content, DateTime.Now);
        //                            sContent = Content.Split(new char[] { ';' });
        //                            pointContent += string.Format(@"{0}~{1}{2}离线,", dtstr, nowStr, sContent[0]);
        //                        }
        //                    }
        //                }
        //            }
        //            WriteTextLog("站点内容", pointContent, DateTime.Now);
        //            if (!string.IsNullOrWhiteSpace(pointContent))
        //            {
        //                pointContent = pointContent.Remove(pointContent.Length - 1);
        //                pointContent += "。";
        //                sendContent += "[南通环境空气质量]";

        //                WriteTextLog("当前报警信息的内容", OfflineAlarms[0].RecordDateTime.ToString(), DateTime.Now);
        //                sendContent += string.Format(pointContent);

        //            }


        //            return sendContent;
        //        }
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        WriteTextLog("异常情况", e.ToString(), DateTime.Now);
        //        return null;
        //    }
        //}

        /// <summary>
        /// 获取断线预警
        /// </summary>
        /// <returns></returns>
        public string getOfflineContent()
        {
            try
            {
                string Content = string.Empty;
                string[] sContent = new string[] { };
                string pointContent = string.Empty;
                string sendContent = string.Empty;
                //获取当前1小时范围内的所有数据
                TB_CreatAlarm[] OfflineAlarms = mSend.TB_CreatAlarms.
                    Where(p => (p.ItemCode == "Offline")
                        && (p.CreatDateTime >= Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
                        && p.CreatDateTime <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00")))).
                        OrderByDescending(p => p.CreatDateTime).ToArray();

                //寻找所有站点
                string[] points = mSend.TB_CreatAlarms.
                    Where(p => (p.ItemCode == "Offline")
                        && (p.CreatDateTime >= Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
                        && p.CreatDateTime <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00")))).
                        Select(p => p.MonitoringPointUid).ToArray().Distinct().ToArray();

                //获取国控点
                //var v = from item in points
                //        where item == "d979dcb6-7707-4def-9a2d-be5cb5270376" || item == "261a801c-516b-4ab4-bad3-a289b80582ee"
                //        || item == "d820a83a-7a97-44da-82de-0ae0d96b1ba3" || item == "dd027da6-43a2-4650-9117-eca7221bab75"
                //        || item == "f4145a15-5bd5-4f9c-9783-99fca706e4d7"
                //        select item;
                //string yysf = ConfigurationManager.AppSettings["yysf"];
                //string yys = ConfigurationManager.AppSettings["yys"];
                //string zzyf = ConfigurationManager.AppSettings["zzyf"];
                //string zzy = ConfigurationManager.AppSettings["zzy"];
                //string fzgyf = ConfigurationManager.AppSettings["fzgyf"];
                //string fzgy = ConfigurationManager.AppSettings["fzgy"];
                //string lygdzf = ConfigurationManager.AppSettings["lygdzf"];
                //string lygdz = ConfigurationManager.AppSettings["lygdz"];
                //List<string> list = new List<string>();
                //if (yysf == "1")
                //{
                //    list.Add(yys);
                //}
                //if (zzyf == "1")
                //{
                //    list.Add(zzy);
                //}
                //if (fzgyf == "1")
                //{
                //    list.Add(fzgy);
                //}
                //if (lygdzf == "1")
                //{
                //    list.Add(lygdz);
                //}
                //list.Sort();
                //string[] newPoints = list.ToArray();

                //points = v.ToArray();
                //string[] cbPoints = new string[points.Length + newPoints.Length];
                //points.CopyTo(cbPoints, 0);
                //newPoints.CopyTo(cbPoints, points.Length);
                //points = cbPoints;
                Array.Sort(points);
                WriteTextLog("当前1h范围内所有数据的个数", OfflineAlarms.Count().ToString(), DateTime.Now);
                string dtstr = "";
                if (OfflineAlarms.Count() > 0)
                {
                    WriteTextLog("站点", string.Join(",", points), DateTime.Now);
                    foreach (string point in points)
                    {
                        WriteTextLog("进入循环", point, DateTime.Now);
                        //当前测点最新数据
                        TB_CreatAlarm latestOfflineAlarm = mSend.TB_CreatAlarms.
                            Where(p => (p.ItemCode == "Offline") && (p.MonitoringPointUid == point)
                            && (p.CreatDateTime >= Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
                            && p.CreatDateTime <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))).
                            OrderByDescending(p => p.CreatDateTime).FirstOrDefault();
                        //当前时间前一小时的最新数据
                        TB_CreatAlarm latestBeforeOfflineAlarm = mSend.TB_CreatAlarms.Where(p => (p.ItemCode == "Offline") && (p.MonitoringPointUid == point)
                               && (p.CreatDateTime >= Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
                               && p.CreatDateTime <= Convert.ToDateTime(DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss")))).
                               OrderByDescending(p => p.CreatDateTime).FirstOrDefault();

                        //当前测点前一小时数据  TB_CreatAlarm beforeAQIAlarm = mSend.TB_CreatAlarms.

                        /*TB_CreatAlarm beforeOfflineAlarm = mSend.TB_CreatAlarms.
                            Where(p => (p.ItemCode == "Offline") && (p.MonitoringPointUid == point)
                            && (p.CreatDateTime >= Convert.ToDateTime(Convert.ToDateTime(latestOfflineAlarm.CreatDateTime).AddDays(-1).ToString("yyyy-MM-dd HH:00:00"))
                            && (p.CreatDateTime <= Convert.ToDateTime(Convert.ToDateTime(latestOfflineAlarm.CreatDateTime).AddSeconds(-1).ToString("yyyy-MM-dd HH:00:00"))))).
                            OrderByDescending(p => p.CreatDateTime).FirstOrDefault();
                        WriteTextLog("当前测点前一小时数据", point +","+ beforeOfflineAlarm.AlarmUid, DateTime.Now);*/
                        if (latestOfflineAlarm != null && latestBeforeOfflineAlarm != null)
                        {
                            WriteTextLog("进入循环1", point, DateTime.Now);
                            if (latestOfflineAlarm.RecordDateTime != latestBeforeOfflineAlarm.RecordDateTime)
                            {
                                //当前测点前一小时数据
                                TB_CreatAlarm beforeOfflineAlarm = mSend.TB_CreatAlarms.Where(p => (p.ItemCode == "Offline") && (p.MonitoringPointUid == point) && (p.RecordDateTime.Equals(Convert.ToDateTime(latestOfflineAlarm.RecordDateTime).AddHours(-1)))).OrderByDescending(p => p.CreatDateTime).FirstOrDefault();
                                WriteTextLog("进入循环5", point, DateTime.Now);
                                if (beforeOfflineAlarm == null)
                                {
                                    WriteTextLog("进入循环4", point, DateTime.Now);
                                    dtstr = Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).ToString("yyyy年MM月dd日HH时");
                                    DateTime dtCreate = Convert.ToDateTime(latestOfflineAlarm.CreatDateTime.ToString());
                                    //DateTime.Now.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day < 1
                                    if (dtCreate.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day < 1)
                                    {
                                        //string nowStr = Convert.ToDateTime(DateTime.Now.ToString()).ToString("HH时");
                                        string nowStr = Convert.ToDateTime(dtCreate.ToString()).ToString("HH时");
                                        WriteTextLog("当前测点最新数据", point + "," + latestOfflineAlarm.AlarmUid, DateTime.Now);
                                        Content = latestOfflineAlarm.SendContent;
                                        WriteTextLog("当前报警信息的内容", Content, DateTime.Now);
                                        sContent = Content.Split(new char[] { ';' });
                                        pointContent += string.Format(@"{0}~{1}{2}离线,", dtstr, nowStr, sContent[0]);
                                    }
                                    //DateTime.Now.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day >= 1
                                    else if (dtCreate.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day >= 1)
                                    {
                                        //string nowStr = Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd日HH时");
                                        string nowStr = Convert.ToDateTime(dtCreate.ToString()).ToString("dd日HH时");
                                        WriteTextLog("当前测点最新数据", point + "," + latestOfflineAlarm.AlarmUid, DateTime.Now);
                                        Content = latestOfflineAlarm.SendContent;
                                        WriteTextLog("当前报警信息的内容", Content, DateTime.Now);
                                        sContent = Content.Split(new char[] { ';' });
                                        pointContent += string.Format(@"{0}~{1}{2}离线,", dtstr, nowStr, sContent[0]);
                                    }
                                }
                            }
                        }
                        else if (latestOfflineAlarm != null && latestBeforeOfflineAlarm == null)
                        {
                            WriteTextLog("进入循环2", point, DateTime.Now);
                            //当前测点前一小时数据
                            TB_CreatAlarm beforeOfflineAlarm = mSend.TB_CreatAlarms.Where(p => (p.ItemCode == "Offline") && (p.MonitoringPointUid == point) && (p.RecordDateTime.Equals(Convert.ToDateTime(latestOfflineAlarm.RecordDateTime).AddHours(-1)))).OrderByDescending(p => p.CreatDateTime).FirstOrDefault();

                            if (beforeOfflineAlarm == null)
                            {
                                WriteTextLog("进入循环3", point, DateTime.Now);
                                dtstr = Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).ToString("yyyy年MM月dd日HH时");
                                DateTime dtCreate = Convert.ToDateTime(latestOfflineAlarm.CreatDateTime.ToString());
                                //DateTime.Now.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day < 1
                                if (dtCreate.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day < 1)
                                {
                                    //string nowStr = Convert.ToDateTime(DateTime.Now.ToString()).ToString("HH时");
                                    string nowStr = Convert.ToDateTime(dtCreate.ToString()).ToString("HH时");
                                    WriteTextLog("当前测点最新数据", point + "," + latestOfflineAlarm.AlarmUid, DateTime.Now);
                                    Content = latestOfflineAlarm.SendContent;
                                    WriteTextLog("当前报警信息的内容", Content, DateTime.Now);
                                    sContent = Content.Split(new char[] { ';' });
                                    pointContent += string.Format(@"{0}~{1}{2}离线,", dtstr, nowStr, sContent[0]);
                                }
                                //DateTime.Now.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day >= 1
                                else if (dtCreate.Day - Convert.ToDateTime(latestOfflineAlarm.RecordDateTime.ToString()).Day >= 1)
                                {
                                    //string nowStr = Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd日HH时");
                                    string nowStr = Convert.ToDateTime(dtCreate.ToString()).ToString("dd日HH时");
                                    WriteTextLog("当前测点最新数据", point + "," + latestOfflineAlarm.AlarmUid, DateTime.Now);
                                    Content = latestOfflineAlarm.SendContent;
                                    WriteTextLog("当前报警信息的内容", Content, DateTime.Now);
                                    sContent = Content.Split(new char[] { ';' });
                                    pointContent += string.Format(@"{0}~{1}{2}离线,", dtstr, nowStr, sContent[0]);
                                }
                            }
                        }
                    }
                    WriteTextLog("站点内容", pointContent, DateTime.Now);
                    if (!string.IsNullOrWhiteSpace(pointContent))
                    {
                        pointContent = pointContent.Remove(pointContent.Length - 1);
                        pointContent += "。";
                        sendContent += "[南通环境空气质量]";

                        WriteTextLog("当前报警信息的内容", OfflineAlarms[0].RecordDateTime.ToString(), DateTime.Now);
                        sendContent += string.Format(pointContent);

                    }


                    return sendContent;
                }
                return null;
            }
            catch (Exception e)
            {
                WriteTextLog("异常情况", e.ToString(), DateTime.Now);
                return null;
            }
        }

        /// <summary>
        /// 数据超标
        /// </summary>
        /// <returns></returns>
        public List<string> Excessive()
        {
            try
            {
                List<TB_CreatAlarm> list_CreateAlarm;
                List<string> list_SendContent = new List<string>();
                string app = "airaaira-aira-aira-aira-airaairaaira";
                string alarmEventUid_Hsp = "9414e489-034c-46b8-893a-a7c4a5b5132f";
                string alarmEventUid_Lsp = "25145a3d-35c3-4462-82f6-19fb1baec351";
                DateTime beginTime = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"));
                DateTime endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //DateTime beginTime = Convert.ToDateTime("2016-08-12 7:00:00");
                //DateTime endTime = Convert.ToDateTime("2016-08-12 9:00:00");

                string[] ca_mps = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && (ca.AlarmEventUid.Equals(alarmEventUid_Hsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lsp)) && ca.CreatDateTime <= endTime && ca.CreatDateTime >= beginTime).Select(p => p.MonitoringPointUid).ToArray().Distinct().ToArray();
                string[] ca_ics = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && (ca.AlarmEventUid.Equals(alarmEventUid_Hsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lsp)) && ca.CreatDateTime <= endTime && ca.CreatDateTime >= beginTime).Select(p => p.ItemCode).ToArray().Distinct().ToArray();
                
                var v = from item in ca_mps
                        where item == "DE9ABE0A-4726-47F6-9461-F7AAA8191AA9" || item == "DCD39064-D95F-4C9E-8D6A-1E11B730E6CC"
                        || item == "0858C578-E30F-401A-BFD7-59C382301C38" || item == "81009267-5EAB-44B2-A4D4-B6AC516C8F22"
                        || item == "99F5681F-8766-4552-91F8-6DBEE390A0F9" || item == "8FBD7A2E-18B2-4C65-A450-095E896E4CBE"
                        || item == "1D00CE7B-FA6D-42D9-98C1-E3044D485E16" || item == "0904D51C-9DEA-4651-84BF-5380B3289EE4"
                        select item;
                ca_mps = v.ToArray();
                Array.Sort(ca_mps);
                Array.Sort(ca_ics);
                WriteTextLog("站点个数和因子个数", ca_mps.Length + "," + ca_ics.Length, DateTime.Now);
                if (ca_mps.Length > 0 && ca_ics.Length > 0)
                {
                    foreach (string ca_mp in ca_mps)
                    {
                        list_CreateAlarm = new List<TB_CreatAlarm>();
                        foreach (string ca_ic in ca_ics)
                        {
                            TB_CreatAlarm creatAlarm = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && (ca.AlarmEventUid.Equals(alarmEventUid_Hsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lsp)) && ca.CreatDateTime <= endTime && ca.CreatDateTime >= beginTime && ca.MonitoringPointUid.Equals(ca_mp) && ca.ItemCode.Equals(ca_ic)).OrderByDescending(d => d.CreatDateTime).FirstOrDefault();
                            WriteTextLog("当前测点以及因子的数据", ca_mp + "," + ca_ic + "," + creatAlarm.AlarmUid, DateTime.Now);
                            if (creatAlarm != null)
                            {
                                TB_CreatAlarm creatAlarm1 = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && (ca.AlarmEventUid.Equals(alarmEventUid_Hsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lsp)) && ca.RecordDateTime.Equals(Convert.ToDateTime(creatAlarm.RecordDateTime).AddHours(-1)) && ca.MonitoringPointUid.Equals(ca_mp) && ca.ItemCode.Equals(ca_ic)).FirstOrDefault();
                                WriteTextLog("当前测点一小时前以及因子的数据", ca_mp + "," + ca_ic + "," + creatAlarm1.AlarmUid, DateTime.Now);
                                if (creatAlarm1 == null)
                                {
                                    list_CreateAlarm.Add(creatAlarm);
                                }
                            }
                        }

                        if (list_CreateAlarm.Count > 0)
                        {
                            list_CreateAlarm.Sort();
                            StringBuilder sb = new StringBuilder();
                            sb.Append("[环境空气报警信息]");
                            DateTime dt = Convert.ToDateTime(list_CreateAlarm[0].RecordDateTime);
                            string point = list_CreateAlarm[0].MonitoringPoint;
                            sb.Append(dt.Month + "月" + dt.Day + "日" + dt.Hour + "时，" + point + "点位");
                            foreach (TB_CreatAlarm ca in list_CreateAlarm)
                            {
                                sb.Append(ca.ItemName + "、");
                            }
                            sb.Remove(sb.Length - 1, 1);
                            sb.Append("数值异常。");

                            list_SendContent.Add(sb.ToString());
                        }
                    }
                }

                return list_SendContent;
            }
            catch (Exception e)
            {
                WriteTextLog("异常情况", e.ToString(), DateTime.Now);
                return null;

            }
        }
        /// <summary>
        /// 超站仪器离线报警
        /// </summary>
        /// <returns></returns>
        public List<string> InstrumentOffline()
        {
            try 
            {
                //|| item == "9ef57f3c-8cce-4fe3-980f-303bbcfde260" 粒径谱仪 
                //|| item == "1589850e-0df1-4d9d-b508-4a77def158ba" 离子色谱仪
                //|| item == "3745f768-a789-4d58-9578-9e41fde5e5f0" VOC
                //|| item == "56dd6e9b-4c8f-4e67-a70f-b6a277cb44d7" 黑炭分析仪
                List<string> list_SendContent = new List<string>();
                List<InstrumentDataOnline> list_CreateAlarm;
                string[] ca_mps = mSends.InstrumentDataOnlines.Where(ca => ca.IsOnline == 0 && ca.DataTypeUid.Equals("1b6367f1-5287-4c14-b120-7a35bd176db1")).Select(p => p.InstrumentUid).ToArray().Distinct().ToArray();
                var v = from item in ca_mps
                        where item == "a6b3d80c-8281-4bc6-af47-f0febf568a5c" || item == "56dd6e9b-4c8f-4e67-a70f-b6a277cb44d7" 
                        || item == "6e4aa38a-f68b-490b-9cd7-3b92c7805c2d" || item == "3745f768-a789-4d58-9578-9e41fde5e5f0" 
                        || item == "1589850e-0df1-4d9d-b508-4a77def158ba" || item == "da4f968f-cc6e-4fec-8219-6167d100499d"
                        || item == "9ef57f3c-8cce-4fe3-980f-303bbcfde260" || item == "6cd5c158-8a79-4540-a8b1-2a062759c9a0"
                        || item == "4dbe4c32-270b-4696-ae56-d66178a3ca78" || item == "93c90000-e723-4a47-8a97-bffd8fcf36ca"
                        select item;
                ca_mps = v.ToArray();
                if (ca_mps.Length>0)
                {
                    
                    foreach (string ca_mp in ca_mps)
                    {
                        list_CreateAlarm = new List<InstrumentDataOnline>();
                        InstrumentDataOnline creatAlarm = mSends.InstrumentDataOnlines.Where(ca => ca.IsOnline == 0 && ca.DataTypeUid.Equals("1b6367f1-5287-4c14-b120-7a35bd176db1") && ca.InstrumentUid.Equals(ca_mp)).OrderByDescending(d => d.NewDataTime).FirstOrDefault();
                        //WriteTextLog("当前", ca_mp + "," + ca_ic + "," + creatAlarm.AlarmUid, DateTime.Now);
                        list_CreateAlarm.Add(creatAlarm);
                        if (list_CreateAlarm.Count > 0)
                        {
                            list_CreateAlarm.Sort();
                            StringBuilder sb = new StringBuilder();
                            
                            DateTime dt = Convert.ToDateTime(list_CreateAlarm[0].NewDataTime);
                            string InstrumentName = list_CreateAlarm[0].InstrumentName;
                            int OffLineTime = Convert.ToInt32(list_CreateAlarm[0].OffLineTime);
                            sb.Append(InstrumentName + "离线" + OffLineTime / 60 + "小时" + OffLineTime % 60 + "分" + " 最近时间" + dt);
                            list_SendContent.Add(sb.ToString());
                        }
                    }
                }
                return list_SendContent;
            }
            catch(Exception ex)
            {
                WriteTextLog("异常情况", ex.ToString(), DateTime.Now);
                return null;
            }
            
        }
        /// <summary>
        /// 因子数据缺失
        /// </summary>
        /// <returns></returns>
        public List<string> Lost()
        {
            try
            {
                List<TB_CreatAlarm> list_CreateAlarm;
                List<string> list_SendContent = new List<string>();
                string app = "airaaira-aira-aira-aira-airaairaaira";
                string alarmEventUid = "e72be1fc-4521-41ad-91ee-3e4dc379b5d6";
                DateTime beginTime = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"));
                DateTime endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //DateTime beginTime = Convert.ToDateTime("2016-07-14 15:00:00");
                //DateTime endTime = Convert.ToDateTime("2016-07-14 18:00:00");

                string[] ca_mps = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && ca.AlarmEventUid.Equals(alarmEventUid) && ca.CreatDateTime <= endTime && ca.CreatDateTime >= beginTime).OrderBy(m => m.MonitoringPointUid).Select(p => p.MonitoringPointUid).ToArray().Distinct().ToArray();
                string[] ca_ics = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && ca.AlarmEventUid.Equals(alarmEventUid) && ca.CreatDateTime <= endTime && ca.CreatDateTime >= beginTime).OrderBy(i => i.ItemCode).Select(p => p.ItemCode).ToArray().Distinct().ToArray();

                var v = from item in ca_mps
                        where item == "d979dcb6-7707-4def-9a2d-be5cb5270376" || item == "261a801c-516b-4ab4-bad3-a289b80582ee"
                        || item == "d820a83a-7a97-44da-82de-0ae0d96b1ba3" || item == "dd027da6-43a2-4650-9117-eca7221bab75"
                        || item == "f4145a15-5bd5-4f9c-9783-99fca706e4d7"
                        select item;
                ca_mps = v.ToArray();
                Array.Sort(ca_ics);
                Array.Sort(ca_mps);
                WriteTextLog("站点个数和因子个数", ca_mps.Length + "," + ca_ics.Length, DateTime.Now);
                if (ca_mps.Length > 0 && ca_ics.Length > 0)
                {
                    foreach (string ca_mp in ca_mps)
                    {
                        list_CreateAlarm = new List<TB_CreatAlarm>();
                        foreach (string ca_ic in ca_ics)
                        {
                            TB_CreatAlarm creatAlarm = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && ca.AlarmEventUid.Equals(alarmEventUid) && ca.CreatDateTime <= endTime && ca.CreatDateTime >= beginTime && ca.MonitoringPointUid.Equals(ca_mp) && ca.ItemCode.Equals(ca_ic)).OrderByDescending(d => d.CreatDateTime).FirstOrDefault();
                            WriteTextLog("当前测点以及因子的数据", ca_mp + "," + ca_ic + "," + creatAlarm.AlarmUid, DateTime.Now);
                            if (creatAlarm != null)
                            {
                                TB_CreatAlarm creatAlarm1 = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && ca.AlarmUid != creatAlarm.AlarmUid && ca.AlarmEventUid.Equals(alarmEventUid) && ca.RecordDateTime.Equals(Convert.ToDateTime(creatAlarm.RecordDateTime)) && ca.MonitoringPointUid.Equals(ca_mp) && ca.ItemCode.Equals(ca_ic)).FirstOrDefault();
                                WriteTextLog("当前测点以及因子的数据", ca_mp + "," + ca_ic + "," + creatAlarm.AlarmUid, DateTime.Now);
                                if (creatAlarm1 == null)
                                {
                                    list_CreateAlarm.Add(creatAlarm);
                                }
                            }
                        }

                        if (list_CreateAlarm.Count > 0)
                        {
                            list_CreateAlarm.Sort();
                            StringBuilder sb = new StringBuilder();
                            sb.Append("[环境空气报警信息]");
                            DateTime dt = Convert.ToDateTime(list_CreateAlarm[0].RecordDateTime).AddHours(1);
                            string point = list_CreateAlarm[0].MonitoringPoint;
                            sb.Append(dt.Month + "月" + dt.Day + "日" + dt.Hour + "时，" + point + "点位");
                            foreach (TB_CreatAlarm ca in list_CreateAlarm)
                            {
                                sb.Append(ca.ItemName + "、");
                            }
                            sb.Remove(sb.Length - 1, 1);
                            sb.Append("数值异常。");

                            list_SendContent.Add(sb.ToString());
                        }
                    }
                }

                return list_SendContent;
            }
            catch (Exception e)
            {
                WriteTextLog("异常情况", e.ToString(), DateTime.Now);
                return null;

            }
        }
        /// <summary>
        /// 数据超标和数据缺失组合报警
        /// </summary>
        /// <returns></returns>
        public List<string> ExcessiveAndLost()
        {
            try
            {
                List<TB_CreatAlarm> list_CreateAlarm;
                List<string> list_SendContent = new List<string>();
                string app = "airaaira-aira-aira-aira-airaairaaira";
                string alarmEventUid_Hsp = "9414e489-034c-46b8-893a-a7c4a5b5132f";
                string alarmEventUid_Lsp = "25145a3d-35c3-4462-82f6-19fb1baec351";
                string alarmEventUid_Lost = "e72be1fc-4521-41ad-91ee-3e4dc379b5d6";
                string alarmEventUid_Copy = "ba7bfb54-4319-4cbe-9211-2f55f87868cf";
                DateTime beginTime = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 00:00:00"));
                DateTime endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //DateTime beginTime = Convert.ToDateTime("2016-08-12 7:00:00");
                //DateTime endTime = Convert.ToDateTime("2016-08-12 9:00:00");

                string[] ca_mps = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && (ca.AlarmEventUid.Equals(alarmEventUid_Hsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lost) || ca.AlarmEventUid.Equals(alarmEventUid_Copy)) && ca.CreatDateTime <= endTime && ca.CreatDateTime >= beginTime).Select(p => p.MonitoringPointUid).ToArray().Distinct().ToArray();
                string[] ca_ics = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && (ca.AlarmEventUid.Equals(alarmEventUid_Hsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lost) || ca.AlarmEventUid.Equals(alarmEventUid_Copy)) && ca.CreatDateTime <= endTime && ca.CreatDateTime >= beginTime).Select(p => p.ItemCode).ToArray().Distinct().ToArray();

                //var v = from item in ca_mps
                //        where item == "d979dcb6-7707-4def-9a2d-be5cb5270376" || item == "261a801c-516b-4ab4-bad3-a289b80582ee"
                //        || item == "d820a83a-7a97-44da-82de-0ae0d96b1ba3" || item == "dd027da6-43a2-4650-9117-eca7221bab75"
                //        || item == "f4145a15-5bd5-4f9c-9783-99fca706e4d7"
                //        select item;
                //ca_mps = v.ToArray();
                Array.Sort(ca_mps);
                Array.Sort(ca_ics);
                WriteTextLog("站点个数和因子个数", ca_mps.Length + "," + ca_ics.Length, DateTime.Now);
                if (ca_mps.Length > 0 && ca_ics.Length > 0)
                {
                    foreach (string ca_mp in ca_mps)
                    {
                        list_CreateAlarm = new List<TB_CreatAlarm>();
                        foreach (string ca_ic in ca_ics)
                        {
                            TB_CreatAlarm creatAlarm = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && (ca.AlarmEventUid.Equals(alarmEventUid_Hsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lost) || ca.AlarmEventUid.Equals(alarmEventUid_Copy)) && ca.CreatDateTime <= endTime && ca.CreatDateTime >= beginTime && ca.MonitoringPointUid.Equals(ca_mp) && ca.ItemCode.Equals(ca_ic)).OrderByDescending(d => d.CreatDateTime).FirstOrDefault();
                            TB_CreatAlarm creatAlarmBeforeHour = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && (ca.AlarmEventUid.Equals(alarmEventUid_Hsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lost) || ca.AlarmEventUid.Equals(alarmEventUid_Copy)) && ca.CreatDateTime <= endTime.AddHours(-1) && ca.CreatDateTime >= beginTime && ca.MonitoringPointUid.Equals(ca_mp) && ca.ItemCode.Equals(ca_ic)).OrderByDescending(d => d.CreatDateTime).FirstOrDefault();
                            //WriteTextLog("当前测点以及因子的数据", ca_mp + "," + ca_ic + "," + creatAlarm.AlarmUid, DateTime.Now);
                            DateTime todayBeginTime = Convert.ToDateTime("00:00:00");
                            if (creatAlarm != null && creatAlarmBeforeHour != null)
                            {
                                if (creatAlarm.RecordDateTime != creatAlarmBeforeHour.RecordDateTime)
                                {

                                    TB_CreatAlarm creatAlarm1 = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && (ca.AlarmEventUid.Equals(alarmEventUid_Hsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lost) || ca.AlarmEventUid.Equals(alarmEventUid_Copy)) && ca.RecordDateTime.Equals(Convert.ToDateTime(creatAlarm.RecordDateTime).AddHours(-1)) && ca.MonitoringPointUid.Equals(ca_mp) && ca.ItemCode.Equals(ca_ic)).FirstOrDefault();
                                    WriteTextLog("当前测点一小时钱以及因子的数据", ca_mp + "," + ca_ic + "," + creatAlarm1.AlarmUid, DateTime.Now);

                                    //string startTimeStr = ConfigurationManager.AppSettings["startTime"];
                                    //DateTime startTime = Convert.ToDateTime(startTimeStr);

                                    if (creatAlarm1 == null)
                                    {
                                        if (creatAlarm.RecordDateTime >= todayBeginTime)
                                            list_CreateAlarm.Add(creatAlarm);
                                    }
                                    else if (creatAlarm1.RecordDateTime < todayBeginTime && creatAlarm.RecordDateTime >= todayBeginTime)
                                    {
                                        list_CreateAlarm.Add(creatAlarm);
                                    }
                                }
                            }
                            else if (creatAlarm != null && creatAlarmBeforeHour == null)
                            {
                                TB_CreatAlarm creatAlarm1 = mSend.TB_CreatAlarms.Where(ca => ca.ApplicationUid.Equals(app) && (ca.AlarmEventUid.Equals(alarmEventUid_Hsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lsp) || ca.AlarmEventUid.Equals(alarmEventUid_Lost) || ca.AlarmEventUid.Equals(alarmEventUid_Copy)) && ca.RecordDateTime.Equals(Convert.ToDateTime(creatAlarm.RecordDateTime).AddHours(-1)) && ca.MonitoringPointUid.Equals(ca_mp) && ca.ItemCode.Equals(ca_ic)).FirstOrDefault();
                                // WriteTextLog("当前测点一小时钱以及因子的数据", ca_mp + "," + ca_ic + "," + creatAlarm1.AlarmUid, DateTime.Now);
                                if (creatAlarm1 == null)
                                {
                                    if (creatAlarm.RecordDateTime >= todayBeginTime)
                                        list_CreateAlarm.Add(creatAlarm);
                                }
                                else if (creatAlarm1.RecordDateTime < todayBeginTime && creatAlarm.RecordDateTime >= todayBeginTime)
                                {
                                    list_CreateAlarm.Add(creatAlarm);
                                }
                            }
                        }


                        if (list_CreateAlarm.Count > 0)
                        {
                            // list_CreateAlarm.Sort();
                            StringBuilder sb = new StringBuilder();
                            sb.Append("[环境空气报警信息]");
                            DateTime dt = Convert.ToDateTime(list_CreateAlarm[0].RecordDateTime);
                            string point = list_CreateAlarm[0].MonitoringPoint;
                            sb.Append(dt.Month + "月" + dt.Day + "日" + dt.Hour + "时，" + point + "点位");
                            foreach (TB_CreatAlarm ca in list_CreateAlarm)
                            {
                                if (ca.AlarmEventUid == alarmEventUid_Lost)
                                {
                                    sb.Append(ca.ItemName + "(缺失)、");
                                }
                                else if (ca.AlarmEventUid == alarmEventUid_Hsp || ca.AlarmEventUid == alarmEventUid_Lsp)
                                {
                                    string mesureUnit = GetMesureUnit(ca.ItemCode);
                                    string decimalStr = GetDecimal(ca.ItemCode);
                                    // int.Parse((m_StandardSolution.GetDecimalDigit(PollutantNameCell.Text) == null ? "3" : m_StandardSolution.GetDecimalDigit(PollutantNameCell.Text)))).ToString();
                                    int decimalNumber = int.Parse(decimalStr == null ? "3" : decimalStr);
                                    decimal value = GetPollutantValue(Convert.ToDecimal(ca.ItemValue), decimalNumber);
                                    string valueStr = string.Empty;
                                    if ("μg/m3".Equals(mesureUnit.Trim()))
                                    {
                                        valueStr = (value * 1000).ToString();
                                    }
                                    else
                                    {
                                        valueStr = value.ToString();
                                    }
                                    sb.Append(ca.ItemName + "(" + valueStr + mesureUnit + ")、");
                                }
                                else if (ca.AlarmEventUid.Equals(alarmEventUid_Copy))
                                {
                                    sb.Append(ca.ItemName + "(重复)、");
                                }
                            }
                            sb.Remove(sb.Length - 1, 1);
                            sb.Append("数值异常。");

                            list_SendContent.Add(sb.ToString());
                        }
                    }
                }

                return list_SendContent;
            }
            catch (Exception e)
            {
                WriteTextLog("异常情况", e.ToString(), DateTime.Now);
                return null;

            }
        }
        /// <summary>
        /// 获取因子的小数位
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetDecimal(string pollutantcode)
        {
            string decimalStr = null;
            try
            {
                if (!string.IsNullOrEmpty(pollutantcode))
                {
                    string sql = "select DecimalDigit from [AMS_BaseDataSZ].[Standard].[TB_PollutantCode] where PollutantCode='" + pollutantcode + "' and DecimalDigit is not null";
                    string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AMS_BaseDataSZConnection"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            con.Open();
                            decimalStr = cmd.ExecuteScalar().ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {

                return decimalStr;
            }
            return decimalStr;
        }
        /// <summary>
        /// 根据因子获取因子单位
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetMesureUnit(string factor)
        {
            string MesureUnit = "";
            try
            {
                if (!string.IsNullOrEmpty(factor))
                {
                    string sql = "select t2.ItemValue from Standard.TB_PollutantCode as t1 left join [EQMS_FrameworkSZ].[dbo].[V_CodeMainItem] as t2 on t1.MeasureUnitUid=t2.ItemGuid where t2.CodeName='计量单位' and PollutantCode= '" + factor + "'";
                    string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AMS_BaseDataSZConnection"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            con.Open();
                            MesureUnit = cmd.ExecuteScalar().ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {

                return MesureUnit;
            }
            return MesureUnit;
        }

        #region 公司短信网关
        /// <summary>
        /// 通过公司短信网关发送短信
        /// </summary>
        /// <param name="smsSend"></param>
        private void SendSMSBySionyd(TB_NotifySend notifySend)
        {
            //获取短信账号，密码，地址
            //string userName = ConfigurationManager.AppSettings["SinoydSMS_UserName"];
            //string password = ConfigurationManager.AppSettings["SinoydSMS_Password"];
            //string url = ConfigurationManager.AppSettings["SinoydSMS_Url"];

            //if (!string.IsNullOrEmpty(notifySend.ReceiveUserAddresses))
            //{
            //    //获取联系方式
            //    string mobileNums = notifySend.ReceiveUserAddresses.Trim(';').ToString();
            //    string[] array_monbileNum = mobileNums.Split(';');

            //    //获取发送内容
            //    string content = notifySend.SendContent.ToString();
            //    content = HttpUtility.UrlEncode(content, System.Text.Encoding.UTF8);

            //    //循环联系方式发送短信
            //    foreach (string number in array_monbileNum)
            //    {
            //        string postData = string.Format("userName={0}&password={1}&mobile={2}&content={3}&extcode=&senddate=&batchID=", userName, password, number, content);

            //        XMLHTTP xmlhttp = new XMLHTTP();
            //        xmlhttp.open("POST", url + "?" + postData, false, null, null);
            //        xmlhttp.send(null);
            //    }
            //}
        }
        private string SendSMSBySionyd(string sendContent, string mobile)
        {
            //string Spcode = System.Configuration.ConfigurationManager.AppSettings["JCSpcode"]; ;  //企业账号
            //string LoginName = System.Configuration.ConfigurationManager.AppSettings["JCLoginName"]; ;  //用户名
            //string Password = System.Configuration.ConfigurationManager.AppSettings["JCPassword"]; ;  //密码
            //string ExtendAccessNum = System.Configuration.ConfigurationManager.AppSettings["JCExtendAccessNum"]; ;  //接入号扩展号
            //object objData = WebServiceHelper.InvokeWebService(m_MessageSendUrl, "PostSendMessage", new object[] { mobile, sendContent, Spcode, LoginName, Password, ExtendAccessNum });
            ////string strResult = manage.PostSendMessage(telephone, content);
            //string strResult = objData as string;
            //return strResult;
            //获取短信账号，密码，地址
            string userName = ConfigurationManager.AppSettings["SinoydSMS_UserName"];
            string password = ConfigurationManager.AppSettings["SinoydSMS_Password"];
            string url = ConfigurationManager.AppSettings["SinoydSMS_Url"];
            string a = string.Empty;
            if (!string.IsNullOrEmpty(mobile))
            {
              //获取联系方式
              string[] array_monbileNum = mobile.Split(',');

              //获取发送内容
              string content = HttpUtility.UrlEncode(sendContent, System.Text.Encoding.UTF8);

              //循环联系方式发送短信
              foreach (string number in array_monbileNum)
              {
                string postData = string.Format("userName={0}&password={1}&mobile={2}&content={3}&extcode=&senddate=&batchID=", userName, password, number, content);

                // string postData = string.Format("userName={0}&password={1}&mobile={2}&content={3}&extcode=&senddate=&batchID=", userName, password, mobile, content);

                XMLHTTP xmlhttp = new XMLHTTP();
                xmlhttp.open("POST", url + "?" + postData, false, null, null);
                xmlhttp.send(null);
                a = xmlhttp.responseText;
                a += a;
              }
            }
            return a;
        }
        #endregion

        #region 发送邮件
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">要发送的邮箱</param>
        /// <param name="mailSubject">邮箱主题</param>
        /// <param name="mailContent">邮箱内容</param>
        /// <returns>返回发送邮箱的结果</returns>
        public static bool SendEmail(string mailTo, string mailSubject, string mailContent)
        {
            // 设置发送方的邮件信息,例如使用网易的smtp
            string smtpServer = "smtp.sinoyd.com"; //SMTP服务器
            //string mailFrom = "lvyun@sinoyd.com"; //登陆用户名
            //string userPassword = "lv2015";//登陆密码
            string mailFrom = "xuyang@sinoyd.com"; //登陆用户名
            string userPassword = "vn?d0O";//登陆密码

            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码

            // 发送邮件设置        
            MailMessage mailMessage = new MailMessage(); // 将要发送的电子邮件实例
            mailMessage.From = new MailAddress(mailFrom); // 发送人
            mailMessage.Subject = mailSubject;//主题
            mailMessage.Body = mailContent;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Low;//优先级

            //遍历收件人邮箱地址，并添加到此邮件的收件人里         
            if (mailTo.Length != 0)
            {
                string[] receivers = mailTo.Split(';');
                for (int i = 0; i < receivers.Length; i++)
                {
                    if (receivers[i].Length > 0)
                    {
                        mailMessage.To.Add(receivers[i]);                               //为该电子邮件添加联系人  
                    }
                }
            }

            try
            {
                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch (SmtpException ex)
            {
                return false;
            }
        }

        #endregion

        /// <summary>  
        /// 写入日志到文本文件  
        /// </summary>  
        /// <param name="action">动作</param>  
        /// <param name="strMessage">日志内容</param>  
        /// <param name="time">时间</param>  
        public static void WriteTextLog(string action, string strMessage, DateTime time)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"System\Log\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".System.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + time.ToString() + "\r\n");
            str.Append("Action:  " + action + "\r\n");
            str.Append("Message: " + strMessage + "\r\n");
            str.Append("-----------------------------------------------------------\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        /// <summary>
        /// 取得数据库中取得的因子浓度实际值
        /// 排除直接用银行家算法时因数据库小数位多、补零而导致的数据异常
        /// </summary>
        /// <param name="value">因子浓度</param>
        /// <param name="decimalNum">小数位</param>
        /// <returns></returns>
        public static decimal GetPollutantValue(decimal value, int decimalNum)
        {
            if (decimalNum < 0)
                return value;
            decimal valuePow = value * Convert.ToInt32(Math.Pow(10, decimalNum));
            if (valuePow - Convert.ToDecimal(Math.Floor(valuePow)) == 0M)
                return Math.Round(value, decimalNum);
            else
                return Math.Round(value, decimalNum, MidpointRounding.ToEven);
        }

    }
}
