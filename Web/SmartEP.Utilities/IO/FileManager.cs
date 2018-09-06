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

        #region ˽���ֶ�
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

        #region ��������
        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// �ļ���Ŀ¼������Ŀ¼
        /// </summary>
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        /// <summary>
        ///  ����ʱ��
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
        /// �ļ���С
        /// </summary>
        public long Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        /// <summary>
        /// �ϴη���ʱ��
        /// </summary>
        public DateTime LastAccessDate
        {
            get { return _LastAccessDate; }
            set { _LastAccessDate = value; }
        }

        /// <summary>
        /// �ϴζ�дʱ��
        /// </summary>
        public DateTime LastWriteDate
        {
            get { return _LastWriteDate; }
            set { _LastWriteDate = value; }
        }

        /// <summary>
        /// �ļ�����
        /// </summary>
        public int FileCount
        {
            get { return _FileCount; }
            set { _FileCount = value; }
        }

        /// <summary>
        /// Ŀ¼����
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
        #region ���캯��
        private static string strRootFolder;
        static FileManager()
        {
            strRootFolder = HttpContext.Current.Request.PhysicalApplicationPath + "File\\";
            strRootFolder = strRootFolder.Substring(0, strRootFolder.LastIndexOf(@"\"));
        }
        #endregion

        #region Ŀ¼
        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        public static string GetRootPath()
        {
            return strRootFolder;
        }

        /// <summary>
        /// д��Ŀ¼
        /// </summary>
        public static void SetRootPath(string path)
        {
            strRootFolder = path;
        }

        /// <summary>
        /// ��ȡĿ¼�б�
        /// </summary>
        public static List<FileItem> GetDirectoryItems()
        {
            return GetDirectoryItems(strRootFolder);
        }

        /// <summary>
        /// ��ȡĿ¼�б�
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

        #region �ļ�
        /// <summary>
        /// ��ȡ�ļ��б�
        /// </summary>
        public static List<FileItem> GetFileItems()
        {
            return GetFileItems(strRootFolder);
        }

        /// <summary>
        /// ��ȡ�ļ��б�
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
        /// �����ļ�
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
        /// �����ļ�
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
        /// ��ȡ�ļ�
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
        /// ��ȡ�ļ���Ϣ
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
        /// д��һ�����ļ������ļ���д�����ݣ�Ȼ��ر��ļ������Ŀ���ļ��Ѵ��ڣ����д���ļ��� 
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
        /// ɾ���ļ�
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
        /// �ƶ��ļ�
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

        #region �ļ���
        /// <summary>
        /// �����ļ���
        /// </summary>
        public static void CreateFolder(string name, string parentName)
        {
            DirectoryInfo di = new DirectoryInfo(parentName);
            di.CreateSubdirectory(name);
        }

        /// <summary>
        /// ɾ���ļ���
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
        /// �ƶ��ļ���
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
        /// �����ļ���
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

        #region ����ļ�
        /// <summary>
        /// �ж��Ƿ�Ϊ��ȫ�ļ���
        /// </summary>
        /// <param name="str">�ļ���</param>
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
        ///  �ж��Ƿ�Ϊ����ȫ�ļ���
        /// </summary>
        /// <param name="str">�ļ������ļ�����</param>
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
        ///  �ж��Ƿ�Ϊ�ɱ༭�ļ�
        /// </summary>
        /// <param name="str">�ļ������ļ�����</param>
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
        /// �����ļ�
        /// </summary>
        /// <param name="sourceFileName">Դ�ļ���</param>
        /// <param name="destFileName">Ŀ���ļ���</param>
        /// <param name="overwrite">��Ŀ���ļ�����ʱ�Ƿ񸲸�</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!System.IO.File.Exists(sourceFileName))
                throw new FileNotFoundException(sourceFileName + "�ļ������ڣ�");

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
        /// �����ļ�,��Ŀ���ļ�����ʱ����
        /// </summary>
        /// <param name="sourceFileName">Դ�ļ���</param>
        /// <param name="destFileName">Ŀ���ļ���</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }

        /// <summary>
        /// �ָ��ļ�
        /// </summary>
        /// <param name="backupFileName">�����ļ���</param>
        /// <param name="targetFileName">Ҫ�ָ����ļ���</param>
        /// <param name="backupTargetFileName">Ҫ�ָ��ļ��ٴα��ݵ�����,���Ϊnull,���ٱ��ݻָ��ļ�</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!System.IO.File.Exists(backupFileName))
                    throw new FileNotFoundException(backupFileName + "�ļ������ڣ�");

                if (backupTargetFileName != null)
                {
                    if (!System.IO.File.Exists(targetFileName))
                        throw new FileNotFoundException(targetFileName + "�ļ������ڣ��޷����ݴ��ļ���");
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
        /// ת�����ļ���Ϊ���ļ���
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
        /// ��ȡָ���ļ�����չ��
        /// </summary>
        /// <param name="fileName">ָ���ļ���</param>
        /// <returns>��չ��</returns>
        public static string GetFileExtName(string fileName)
        {
            if (StringExtensions.IsNullOrEmpty(fileName) || fileName.IndexOf('.') <= 0)
                return "";

            fileName = fileName.ToLower().Trim();

            return fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
        }
        #endregion

        #region �����ļ�
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
                message("�ļ�������");
            }
        }
        #endregion

        /// <summary>
        /// ������Ϣ��,��ʾ����(msg),����ת
        /// </summary>
        /// <param name="msg">��ʾ����</param>
        private static void message(string msg)
        {
            System.Web.HttpContext.Current.Response.Write("<script language=javascript>alert('" + msg + "');</script>");
        }
    }
}