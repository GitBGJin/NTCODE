using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Utilities.IO
{
    [Serializable]
    public class FileItem
    {
        public FileItem()
        { }

        #region 私有字段
        private string _Name;
        private string _FullName;
        private DateTime _CreationDate;
        private bool _IsFolder;
        private long _Size;
        private DateTime _LastAccessDate;
        private DateTime _LastWriteDate;
        private int _FileCount;
        private int _SubFolderCount;
        #endregion

        #region 公有属性
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// 文件或目录的完整目录
        /// </summary>
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime CreationDate
        {
            get { return _CreationDate; }
            set { _CreationDate = value; }
        }

        public bool IsFolder
        {
            get { return _IsFolder; }
            set { _IsFolder = value; }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        /// <summary>
        /// 上次访问时间
        /// </summary>
        public DateTime LastAccessDate
        {
            get { return _LastAccessDate; }
            set { _LastAccessDate = value; }
        }

        /// <summary>
        /// 上次读写时间
        /// </summary>
        public DateTime LastWriteDate
        {
            get { return _LastWriteDate; }
            set { _LastWriteDate = value; }
        }

        /// <summary>
        /// 文件个数
        /// </summary>
        public int FileCount
        {
            get { return _FileCount; }
            set { _FileCount = value; }
        }

        /// <summary>
        /// 目录个数
        /// </summary>
        public int SubFolderCount
        {
            get { return _SubFolderCount; }
            set { _SubFolderCount = value; }
        }
        #endregion
    }

    public class FileManager
    {
        #region 构造函数
        private static string strRootFolder;
        static FileManager()
        {
            strRootFolder = HttpContext.Current.Request.PhysicalApplicationPath + "File\\";
            strRootFolder = strRootFolder.Substring(0, strRootFolder.LastIndexOf(@"\"));
        }
        #endregion

        #region 目录
        /// <summary>
        /// 读根目录
        /// </summary>
        public static string GetRootPath()
        {
            return strRootFolder;
        }

        /// <summary>
        /// 写根目录
        /// </summary>
        public static void SetRootPath(string path)
        {
            strRootFolder = path;
        }

        /// <summary>
        /// 读取目录列表
        /// </summary>
        public static List<FileItem> GetDirectoryItems()
        {
            return GetDirectoryItems(strRootFolder);
        }

        /// <summary>
        /// 读取目录列表
        /// </summary>
        public static List<FileItem> GetDirectoryItems(string path)
        {
            List<FileItem> list = new List<FileItem>();
            string[] folders = Directory.GetDirectories(path);
            foreach (string s in folders)
            {
                FileItem item = new FileItem();
                DirectoryInfo di = new DirectoryInfo(s);
                item.Name = di.Name;
                item.FullName = di.FullName;
                item.CreationDate = di.CreationTime;
                item.IsFolder = false;
                list.Add(item);
            }
            return list;
        }
        #endregion

        #region 文件
        /// <summary>
        /// 读取文件列表
        /// </summary>
        public static List<FileItem> GetFileItems()
        {
            return GetFileItems(strRootFolder);
        }

        /// <summary>
        /// 读取文件列表
        /// </summary>
        public static List<FileItem> GetFileItems(string path)
        {
            List<FileItem> list = new List<FileItem>();
            string[] files = Directory.GetFiles(path);
            foreach (string s in files)
            {
                FileItem item = new FileItem();
                FileInfo fi = new FileInfo(s);
                item.Name = fi.Name;
                item.FullName = fi.FullName;
                item.CreationDate = fi.CreationTime;
                item.IsFolder = true;
                item.Size = fi.Length;
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        public static bool CreateFile(string filename, string path)
        {
            try
            {
                FileStream fs = File.Create(path + "\\" + filename);
                fs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        public static bool CreateFile(string filename, string path, byte[] contents)
        {
            try
            {
                FileStream fs = File.Create(path + "\\" + filename);
                fs.Write(contents, 0, contents.Length);
                fs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        public static string OpenText(string parentName)
        {
            StreamReader sr = File.OpenText(parentName);
            StringBuilder output = new StringBuilder();
            string rl;
            while ((rl = sr.ReadLine()) != null)
            {
                output.Append(rl);
            }
            sr.Close();
            return output.ToString();
        }

        /// <summary>
        /// 读取文件信息
        /// </summary>
        public static FileItem GetItemInfo(string path)
        {
            FileItem item = new FileItem();
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                item.Name = di.Name;
                item.FullName = di.FullName;
                item.CreationDate = di.CreationTime;
                item.IsFolder = true;
                item.LastAccessDate = di.LastAccessTime;
                item.LastWriteDate = di.LastWriteTime;
                item.FileCount = di.GetFiles().Length;
                item.SubFolderCount = di.GetDirectories().Length;
            }
            else
            {
                FileInfo fi = new FileInfo(path);
                item.Name = fi.Name;
                item.FullName = fi.FullName;
                item.CreationDate = fi.CreationTime;
                item.LastAccessDate = fi.LastAccessTime;
                item.LastWriteDate = fi.LastWriteTime;
                item.IsFolder = false;
                item.Size = fi.Length;
            }
            return item;
        }

        /// <summary>
        /// 写入一个新文件，在文件中写入内容，然后关闭文件。如果目标文件已存在，则改写该文件。 
        /// </summary>
        public static bool WriteAllText(string parentName, string contents)
        {
            try
            {
                File.WriteAllText(parentName, contents, Encoding.Unicode);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        public static bool MoveFile(string oldPath, string newPath)
        {
            try
            {
                File.Move(oldPath, newPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 文件夹
        /// <summary>
        /// 创建文件夹
        /// </summary>
        public static void CreateFolder(string name, string parentName)
        {
            DirectoryInfo di = new DirectoryInfo(parentName);
            di.CreateSubdirectory(name);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public static bool DeleteFolder(string path)
        {
            try
            {
                Directory.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        public static bool MoveFolder(string oldPath, string newPath)
        {
            try
            {
                Directory.Move(oldPath, newPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        public static bool CopyFolder(string source, string destination)
        {
            try
            {
                String[] files;
                if (destination[destination.Length - 1] != Path.DirectorySeparatorChar) destination += Path.DirectorySeparatorChar;
                if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
                files = Directory.GetFileSystemEntries(source);
                foreach (string element in files)
                {
                    if (Directory.Exists(element))
                        CopyFolder(element, destination + Path.GetFileName(element));
                    else
                        File.Copy(element, destination + Path.GetFileName(element), true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 检测文件
        /// <summary>
        /// 判断是否为安全文件名
        /// </summary>
        /// <param name="str">文件名</param>
        public static bool IsSafeName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] arrExtension = { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap", ".jpg", ".gif", ".png", ".rar", ".zip" };
            for (int i = 0; i < arrExtension.Length; i++)
            {
                if (strExtension.Equals(arrExtension[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///  判断是否为不安全文件名
        /// </summary>
        /// <param name="str">文件名、文件夹名</param>
        public static bool IsUnsafeName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] arrExtension = { ".", ".asp", ".aspx", ".cs", ".net", ".dll", ".config", ".ascx", ".master", ".asmx", ".asax", ".cd", ".browser", ".rpt", ".ashx", ".xsd", ".mdf", ".resx", ".xsd" };
            for (int i = 0; i < arrExtension.Length; i++)
            {
                if (strExtension.Equals(arrExtension[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///  判断是否为可编辑文件
        /// </summary>
        /// <param name="str">文件名、文件夹名</param>
        public static bool IsCanEdit(string strExtension)
        {
            strExtension = strExtension.ToLower();

            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] arrExtension = { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap" };
            for (int i = 0; i < arrExtension.Length; i++)
            {
                if (strExtension.Equals(arrExtension[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 备份文件
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!System.IO.File.Exists(sourceFileName))
                throw new FileNotFoundException(sourceFileName + "文件不存在！");

            if (!overwrite && System.IO.File.Exists(destFileName))
                return false;

            try
            {
                System.IO.File.Copy(sourceFileName, destFileName, true);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 备份文件,当目标文件存在时覆盖
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }

        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <param name="backupTargetFileName">要恢复文件再次备份的名称,如果为null,则不再备份恢复文件</param>
        /// <returns>操作是否成功</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!System.IO.File.Exists(backupFileName))
                    throw new FileNotFoundException(backupFileName + "文件不存在！");

                if (backupTargetFileName != null)
                {
                    if (!System.IO.File.Exists(targetFileName))
                        throw new FileNotFoundException(targetFileName + "文件不存在！无法备份此文件！");
                    else
                        System.IO.File.Copy(targetFileName, backupTargetFileName, true);
                }
                System.IO.File.Delete(targetFileName);
                System.IO.File.Copy(backupFileName, targetFileName);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public static bool RestoreFile(string backupFileName, string targetFileName)
        {
            return RestoreFile(backupFileName, targetFileName, null);
        }

        /// <summary>
        /// 转换长文件名为短文件名
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="repstring"></param>
        /// <param name="leftnum"></param>
        /// <param name="rightnum"></param>
        /// <param name="charnum"></param>
        /// <returns></returns>
        public static string ConvertSimpleFileName(string fullname, string repstring, int leftnum, int rightnum, int charnum)
        {
            string simplefilename = "", leftstring = "", rightstring = "", filename = "";
            string extname = GetFileExtName(fullname);

            if (StringExtensions.IsNullOrEmpty(extname))
                return fullname;

            int filelength = 0, dotindex = 0;

            dotindex = fullname.LastIndexOf('.');
            filename = fullname.Substring(0, dotindex);
            filelength = filename.Length;
            if (dotindex > charnum)
            {
                leftstring = filename.Substring(0, leftnum);
                rightstring = filename.Substring(filelength - rightnum, rightnum);
                if (repstring == "" || repstring == null)
                    simplefilename = leftstring + rightstring + "." + extname;
                else
                    simplefilename = leftstring + repstring + rightstring + "." + extname;
            }
            else
                simplefilename = fullname;

            return simplefilename;
        }

        /// <summary>
        /// 获取指定文件的扩展名
        /// </summary>
        /// <param name="fileName">指定文件名</param>
        /// <returns>扩展名</returns>
        public static string GetFileExtName(string fileName)
        {
            if (StringExtensions.IsNullOrEmpty(fileName) || fileName.IndexOf('.') <= 0)
                return "";

            fileName = fileName.ToLower().Trim();

            return fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
        }
        #endregion

        #region 下载文件
        public static void DownLoad(string filePath, String NewFileName)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            if (fileInfo.Exists == true)
            {
                int intStart = filePath.LastIndexOf("\\") + 1;
                string saveFileName = filePath.Substring(intStart, filePath.Length - intStart);
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Buffer = true;
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(NewFileName, System.Text.Encoding.UTF8));

                System.Web.HttpContext.Current.Response.WriteFile(filePath);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.Close();
                System.Web.HttpContext.Current.Response.End();
            }
            else
            {
                message("文件不存在");
            }
        }
        #endregion

        /// <summary>
        /// 弹出消息框,显示内容(msg),无跳转
        /// </summary>
        /// <param name="msg">显示内容</param>
        private static void message(string msg)
        {
            System.Web.HttpContext.Current.Response.Write("<script language=javascript>alert('" + msg + "');</script>");
        }
    }
}