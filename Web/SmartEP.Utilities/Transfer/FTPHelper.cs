using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SmartEP.Utilities.Transfer
{
    /// <summary>
    /// 名称：FTPHelper.cs
    /// 创建人：李飞
    /// 创建日期：2015-12-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// FTP处理工具
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class FTPHelper
    {
        static Regex regexName = new Regex(@"[^\s]*$", RegexOptions.Compiled);
        FtpWebRequest reqFTP;

        #region 属性
        /// <summary>
        /// 获取或设置FTP地址
        /// </summary>
        public string ftpServerIP { get; set; }
        /// <summary>
        /// 获取或设置登录名
        /// </summary>
        public string ftpUserUID { get; set; }
        /// <summary>
        /// 获取或设置密码
        /// </summary>
        public string ftpUserPWD { get; set; }
        #endregion

        #region 构造函数
        public FTPHelper(String ftpServerIP, String ftpUserUID, String ftpUserPWD)
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpUserUID = ftpUserUID;
            this.ftpUserPWD = ftpUserPWD;
        }
        #endregion

        #region 连接
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="path"></param>
        private void Connect(String path)//连接ftp
        {
            // 根据uri创建FtpWebRequest对象
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
            // 指定数据传输类型
            reqFTP.UseBinary = true;
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(ftpUserUID, ftpUserPWD);
        }
        #endregion

        /// <summary>
        /// 从FTP下载文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="ImageSrc"></param>
        /// <param name="ImageName"></param>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserUID"></param>
        /// <param name="ftpUserPWD"></param>
        public void Download(String filePath, String ImageSrc, String ImageName)
        {
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

            using (FileStream OutputStream = new FileStream(filePath + "\\" + ImageName, FileMode.Create))
            {
                FtpWebRequest ReqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + ImageSrc));
                ReqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                ReqFTP.UseBinary = true;
                ReqFTP.Credentials = new NetworkCredential(ftpUserUID, ftpUserPWD);

                using (FtpWebResponse response = (FtpWebResponse)ReqFTP.GetResponse())
                {
                    using (Stream FtpStream = response.GetResponseStream())
                    {
                        long Cl = response.ContentLength;

                        int bufferSize = 2048;
                        byte[] buffer = new byte[bufferSize];
                        int readCount = FtpStream.Read(buffer, 0, bufferSize);

                        while (readCount > 0)
                        {
                            OutputStream.Write(buffer, 0, readCount);
                            readCount = FtpStream.Read(buffer, 0, bufferSize);
                        }
                        FtpStream.Close();
                    }
                    response.Close();
                }
                OutputStream.Close();
            }
        }

        /// <summary>
        /// 从服务器上传文件到FTP上
        /// </summary>
        /// <param name="sFileDstPath">本地路径与文件</param>
        /// <param name="FolderName">服务器路径</param>
        public String UploadSmall(String sFileDstPath, String FolderName)
        {
            try
            {
                FileInfo fileInf = new FileInfo(sFileDstPath);
                if (FolderName != "") FolderName = "/" + FolderName;
                String ftpUri = String.Format("ftp://{0}{1}/{2}", ftpServerIP, FolderName, fileInf.Name);
                //String ftpUri = String.Format("ftp://{0}{1}/", ftpServerIP, FolderName);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpUri));
                reqFTP.Credentials = new NetworkCredential(ftpUserUID, ftpUserPWD);//指定登录ftp服务器的用户名和密码。
                reqFTP.KeepAlive = false;//指定连接是应该关闭还是在请求完成之后关闭，默认为true
                reqFTP.UsePassive = true;//指定使用被动模式，默认为true
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;//指示服务器要传输的是二进制数据.false,指示数据为文本。默认值为true
                reqFTP.ContentLength = fileInf.Length;

                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                using (FileStream fs = fileInf.OpenRead())
                {
                    using (Stream strm = reqFTP.GetRequestStream())
                    {
                        contentLen = fs.Read(buff, 0, buffLength);
                        while (contentLen != 0)
                        {
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }
                        strm.Close();
                    }
                    fs.Close();
                }
                return "上传成功！";
            }
            catch (Exception exErr)
            { return exErr.Message; }
        }

        /// <summary>
        /// 从客户端上传文件到FTP上
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <param name="filename"></param>
        /// <param name="FolderName"></param>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserUID"></param>
        /// <param name="ftpUserPWD"></param>
        public void UploadFtp(HttpPostedFile sFilePath, String filename, String FolderName)
        {
            //获取的服务器路径
            //FileInfo fileInf = new FileInfo(sFilePath);
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + FolderName + "/" + filename));
            reqFTP.Credentials = new NetworkCredential(ftpUserUID, ftpUserPWD);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = sFilePath.ContentLength;

            //设置缓存
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            using (Stream fs = sFilePath.InputStream)
            {
                using (Stream strm = reqFTP.GetRequestStream())
                {
                    contentLen = fs.Read(buff, 0, buffLength);
                    while (contentLen != 0)
                    {
                        strm.Write(buff, 0, contentLen);
                        contentLen = fs.Read(buff, 0, buffLength);
                    }
                    strm.Close();
                }
                fs.Close();
            }
        }

        /// <summary>
        /// 删除服务器上的文件
        /// </summary>
        /// <param name="sFilePath"></param>
        public void DeleteWebServerFile(String sFilePath)
        {
            if (File.Exists(sFilePath)) File.Delete(sFilePath);
        }

        /// <summary>
        /// 删除FTP上的文件
        /// </summary>
        /// <param name="IName"></param>
        /// <param name="FolderName"></param>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserUID"></param>
        /// <param name="ftpUserPWD"></param>
        public void DeleteFtpFile(String[] IName, String FolderName)
        {
            foreach (String ImageName in IName)
            {
                String[] FileList = GetFileList(FolderName, ftpServerIP, ftpUserUID, ftpUserPWD);
                for (int i = 0; i < FileList.Length; i++)
                {
                    String Name = FileList[i].ToString();
                    if (Name == ImageName)
                    {
                        FtpWebRequest ReqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + FolderName + "/" + ImageName));
                        ReqFTP.Credentials = new NetworkCredential(ftpUserUID, ftpUserPWD);
                        ReqFTP.KeepAlive = false;
                        ReqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                        ReqFTP.UseBinary = true;

                        using (FtpWebResponse Response = (FtpWebResponse)ReqFTP.GetResponse())
                        {
                            long size = Response.ContentLength;
                            using (Stream datastream = Response.GetResponseStream())
                            {
                                using (StreamReader sr = new StreamReader(datastream))
                                {
                                    sr.ReadToEnd();
                                    sr.Close();
                                }
                                datastream.Close();
                            }
                            Response.Close();
                        }
                    }
                }

            }

        }

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="FolderName"></param>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserUID"></param>
        /// <param name="ftpUserPWD"></param>
        /// <returns></returns>
        public String[] GetFileList(String FolderName, String ftpSvrIP, String ftpLoginUID, String ftpLoginPWD)
        {
            String[] downloadFiles;
            StringBuilder result = new StringBuilder();
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpSvrIP + "/" + FolderName + "/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpLoginUID, ftpLoginPWD);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;

                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                String line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'        
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                return downloadFiles;
            }
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="FolderName"></param>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserUID"></param>
        /// <param name="ftpUserPWD"></param>
        /// <returns></returns>
        public String CreateDirectory(String FolderName)
        {
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + FolderName));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserUID, ftpUserPWD);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                return "成功！";
            }
            catch
            {
                return "系统忙，请稍后再试！";
            }
        }

        /// <summary>
        /// 检查日期目录和文件是否存在
        /// </summary>
        /// <param name="FolderName"></param>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserUID"></param>
        /// <param name="ftpUserPWD"></param>
        /// <returns></returns>
        public bool CheckFileOrPath(String FolderName)
        {
            //检查一下日期目录是否存在
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/"));
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(ftpUserUID, ftpUserPWD);
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            Stream stream = reqFTP.GetResponse().GetResponseStream();
            using (StreamReader sr = new StreamReader(stream))
            {
                String line = sr.ReadLine();
                while (!String.IsNullOrEmpty(line))
                {
                    GroupCollection gc = regexName.Match(line).Groups;
                    if (gc.Count != 1) throw new ApplicationException("FTP 返回的字串格式不正确");
                    String path = gc[0].Value;
                    if (path == FolderName) return true;
                    line = sr.ReadLine();
                }
            }
            return false;
        }
    }
}
