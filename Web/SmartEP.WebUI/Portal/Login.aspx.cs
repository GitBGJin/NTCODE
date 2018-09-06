using SmartEP.WebUI.Common;
using SmartEP.DomainModel;
using SmartEP.Utilities.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SmartEP.Utilities.AdoData;
using log4net;
using System.Configuration;
using SmartEP.Utilities.Web.NetWork;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net;

namespace SmartEP.WebUI.Portal
{
    /// <summary>
    /// 名称：Login.aspx.cs
    /// 创建人：吕云
    /// 创建日期：2017-5-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：登陆页面
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器

        FrameworkModel _userModel = new FrameworkModel();
        public  readonly string myKey = "q0m3sd8l";
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Helper.SetTelerikTheme(this, Helper.GetTheme());
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            //Session["UserGuid"] = "8f2c499d-fd0c-49de-a31e-ff577969e82a";
            Helper.SetPageTheme(this, Helper.GetTheme());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
             
                System.Web.HttpContext.Current.Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();
               
                //判断是否有cookie值，有的话就读取出来
                if (!IsPostBack)
                {
                  HttpCookie cookies = Request.Cookies["platform"];
                   HttpCookie cookiesTo = Request.Cookies["platforms"];
                  if (cookies != null && cookies.HasKeys)
                  {
                    RadTxtUser.Text = cookies["Name"];
                    //tbxPwd.Text= cookies["Pwd"];
                    string password = Decrypt(cookies["Pwd"], myKey);
                    RadTxtPwd.Attributes.Add("value", password);
                    this.passRem.Checked = true;
                  }
                  else if (cookiesTo != null && cookiesTo.HasKeys)
                  {
                    RadTxtUser.Text = cookiesTo["Name"];
                  }
                }
                
            }
            catch
            {
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                #region 获取页面url
                string InnerIP = System.Configuration.ConfigurationManager.AppSettings["InnerIP"].ToString();
                string InnerPortalUrl = System.Configuration.ConfigurationManager.AppSettings["InnerPortalUrl"].ToString();
                bool IsInnerNetWork = Networks.IsInnerNetWork(InnerIP);
                
                string frameUrl = ConfigurationManager.AppSettings["PortalUrl"].ToString();
                if (IsInnerNetWork == true)//指定ip处于内网则切换到内网ip
                {
                    //frameUrl = InnerPortalUrl;
                }
                string homePageUrl = frameUrl + "/" + ConfigurationManager.AppSettings["PortalName"].ToString();
                frameUrl += "/" + ConfigurationManager.AppSettings["FrameworkName"].ToString();
                string gISUrl = ConfigurationManager.AppSettings["GISUrl"].ToString();
                #endregion

                //log.Error("test");
                string name = RadTxtUser.Text;
                string pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(RadTxtPwd.Text, "SHA1");

                Frame_UserEntity userObj = _userModel.Frame_UserEntities.FirstOrDefault(x => x.LoginID.Equals(name) && x.Password.Equals(pwd) && x.IsEnabled != (Nullable<int>)null && x.IsEnabled.Value == 1);

                if (userObj != null)
                {
                    SessionHelper.Add("UserGuid", userObj.RowGuid);
                    SessionHelper.Add("DisplayName", userObj.DisplayName);
                    SessionHelper.Add("LoginID", userObj.LoginID);
                    System.Web.HttpContext.Current.Session.Timeout = 600;

                    string desKey = System.Configuration.ConfigurationManager.AppSettings["DesKey"].ToString();
                    string token = Com.Sinoyd.Security.Security.EncryptDES(userObj.RowGuid, desKey);
                    
                    // 记录登录成功日志 add by wangtq 2016.5.24
                    DatabaseHelper dbinsert = new DatabaseHelper();
                    string loginID = userObj.LoginID;
                    string displayName = userObj.DisplayName;
                    string loginNote = "登录成功";
                    HttpRequest request = HttpContext.Current.Request;
                    string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                    string loginTime = DateTime.Now.ToString();
                    //测试
                    //string sss=GetIpAddress();
                    string sql = @"insert into TB_Frame_LoginLog (RowGuid,UserGuid,DisplayName,LoginID,LoginTime,LoginIP,LoginNote,RowStatus)  values(newid(),'" + userObj.RowGuid + "','" + displayName + "','" + loginID + "','" + loginTime + "','" + ip + "','" + loginNote + "','1')";
                    dbinsert.ExecuteNonQuery(sql, "Frame_Connection");
                    if (passRem.Checked)
                    {  if (Response.Cookies["platforms"] != null)
                      Response.Cookies["platforms"].Expires = DateTime.Now;
                      HttpCookie cookie = new HttpCookie("platform");
                      cookie.Values.Add("Name", RadTxtUser.Text.Trim());
                      string password = Encrypt(RadTxtPwd.Text.Trim(), myKey);
                      cookie.Values.Add("Pwd", password);
                      cookie.Expires = System.DateTime.Now.AddDays(7.0);
                      HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                    else
                    {
                      if (Response.Cookies["platform"] != null)
                       Response.Cookies["platform"].Expires = DateTime.Now;
                      HttpCookie cookies = new HttpCookie("platforms");
                      cookies.Values.Add("Name", RadTxtUser.Text.Trim());
                      cookies.Expires = System.DateTime.Now.AddDays(7.0);
                      HttpContext.Current.Response.Cookies.Add(cookies);
                    }
                    Response.Redirect(homePageUrl + "/Portal/HomePage.aspx");

                    //Response.Redirect(homePageUrl + "/Portal/HomePage.aspx?token=" + token);
                    //Response.Redirect("HomePage.aspx?token=" + token);
                    //Response.Redirect("MidPage.aspx?Type=Air");
                    //Response.Redirect(frameUrl + "/FrameAir.aspx?Token=" + token);

                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(
                   this,
                   this.GetType(),
                   Guid.NewGuid().ToString(),
                   "alert('" + HttpUtility.HtmlEncode("系统不能完成您的登录请求，请检查您的用户名和密码是否匹配!!") + "');",
                   true);

                    // 记录登录失败日志 add by wangtq 2016.5.25
                    DatabaseHelper dbinsert = new DatabaseHelper();
                    System.Guid guid = new Guid();
                    guid = Guid.NewGuid();
                    string userid = guid.ToString(); ;
                    string loginID = this.RadTxtUser.Text;
                    string displayName = "";
                    string loginNote = "登录失败";
                    HttpRequest request = HttpContext.Current.Request;
                    string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                    string loginTime = DateTime.Now.ToString();
                    string sql = @"insert into TB_Frame_LoginLog (RowGuid,UserGuid,DisplayName,LoginID,LoginTime,LoginIP,LoginNote,RowStatus)  values(newid(),'" + userid + "','" + displayName + "','" + loginID + "','" + loginTime + "','" + ip + "','" + loginNote + "','0')";
                    dbinsert.ExecuteNonQuery(sql, "Frame_Connection");

                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #region 加密方法
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="pToEncrypt">需要加密字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns>加密后的字符串</returns>
        protected  string Encrypt(string pToEncrypt, string sKey)
        {
          try
          {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中


            //原来使用的UTF8编码，我改成Unicode编码了，不行
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);

            //建立加密对象的密钥和偏移量


            //使得输入密码必须输入英文文本
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
              ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
          }
          catch (Exception ex)
          {
            log.Error(ex.ToString());
          }

          return "";
        }
        #endregion

        #region 解密方法
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="pToDecrypt">需要解密的字符串</param>
        /// <param name="sKey">密匙</param>
        /// <returns>解密后的字符串</returns>
        protected  string Decrypt(string pToDecrypt, string sKey)
        {
          try
          {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
              int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
              inputByteArray[x] = (byte)i;
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
          }
          catch (Exception ex)
          {
            log.Error(ex.ToString());

          }
          return "";
        }
        #endregion
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.RadTxtUser.Text = string.Empty;
            this.RadTxtPwd.Text = string.Empty;
         
        }
        /// <summary>
        /// 获取ip地址
        /// </summary>
        /// <returns></returns>
        public string GetIpAddress()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            IPAddress localaddr = localhost.AddressList[0];

            return localaddr.ToString();
        }
        /// <summary>
        /// 获取ip地址
        /// </summary>
        /// <returns></returns>
        public string GetIpAddressNew()
        {
            HttpRequest request = HttpContext.Current.Request;
            string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                result = "0.0.0.0";
            }

            return result;
        }
        public string GetIpAddressNewnew()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }
            return IP4Address;
        }
    }
}