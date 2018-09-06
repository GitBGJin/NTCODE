using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.QualityControl
{
    public partial class ExcelShow : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 生成的Html文件路径（包含文件名）
        /// </summary>
        private string _FileFullName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitForm();
            }
        }
        protected override void OnUnload(EventArgs e)
        {
            //该事件首先针对每个控件发生，继而针对该页发生。在控件中，使用该事件对特定控件执行最后清理，
            //如关闭控件特定数据库连接。
            //对于页自身，使用该事件来执行最后清理工作，如：关闭打开的文件和数据库连接，或完成日志记录或其他请求特定任务。
            //注意
            //在卸载阶段，页及其控件已被呈现，因此无法对响应流做进一步更改。
            //如果尝试调用方法（如 Response.Write 方法），则该页将引发异常。

            if (!string.IsNullOrWhiteSpace(_FileFullName))
            {
                //DeleteFiles(new string[] { _FileFullName });
                string filePath = _FileFullName.Substring(0, _FileFullName.LastIndexOf('\\'));
                DeleteFilesByDateTime(DateTime.Now.AddDays(-1), filePath);
            }
        }
        /// <summary>
        /// 界面数据初始化
        /// </summary> 
        private void InitForm()
        {
            string newFileName = string.Empty;

            string FilePath = System.Configuration.ConfigurationManager.AppSettings["FileExcelPath"];
            string FileName = PageHelper.GetQueryString("FileName");
            _FileFullName = ExcelToHtml(FilePath, FileName, ref newFileName);
            iframeTaskInfo.Attributes["src"] = "TempHtmlFolder/" + newFileName;
        }
        /// <summary>
        /// Excel转换成Html
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns>Html的存放路径（包括文件名）</returns>
        public string ExcelToHtml(string filePath, string fileName, ref string newFileName)
        {
            //实例化Excel  
            Microsoft.Office.Interop.Excel.Application repExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            //打开文件，n.FullPath是文件路径  
            workbook = repExcel.Application.Workbooks.Open(filePath + fileName, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            string filesavefilename = fileName.ToString();
            string strsavefilepath = Server.MapPath("TempHtmlFolder/");
            if (!Directory.Exists(strsavefilepath))
            {
                Directory.CreateDirectory(strsavefilepath);
            }
            string strsavefilename = string.Empty;
            if (filesavefilename.LastIndexOf(".") >= 0)
            {
                strsavefilename = filesavefilename.Substring(0, filesavefilename.LastIndexOf("."))
                                     + DateTime.Now.ToFileTime().ToString() + ".html";
            }
            else
            {
                strsavefilename = filesavefilename + DateTime.Now.ToFileTime().ToString() + ".html";
            }
            newFileName = strsavefilename;
            object savefilename = (object)(strsavefilepath + strsavefilename);
            object ofmt = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
            //进行另存为操作    
            workbook.SaveAs(savefilename, ofmt, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);
            object osave = false;
            //逐步关闭所有使用的对象  
            workbook.Close(osave, Type.Missing, Type.Missing);
            repExcel.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            worksheet = null;
            //垃圾回收  
            GC.Collect();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            workbook = null;
            GC.Collect();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(repExcel.Application.Workbooks);
            GC.Collect();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(repExcel);
            repExcel = null;
            GC.Collect();
            //依据时间杀灭进程  
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("EXCEL");
            foreach (System.Diagnostics.Process p in process)
            {
                if (DateTime.Now.Second - p.StartTime.Second > 0 && DateTime.Now.Second - p.StartTime.Second < 5)
                {
                    p.Kill();
                }
            }

            return savefilename.ToString();
        }
        /// <summary>
        /// 根据时间删除文件
        /// </summary>
        /// <param name="dtime"></param>
        /// <param name="filePath"></param>
        private void DeleteFilesByDateTime(DateTime dtime, string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                return;
            }
            string[] fileNames = Directory.GetFiles(filePath);
            string[] directorys = Directory.GetDirectories(filePath);
            filePath = filePath.TrimEnd('\\') + "\\";
            foreach (string fileName in fileNames)
            {
                if (File.Exists(fileName) && File.GetCreationTime(fileName) < dtime)
                {
                    File.Delete(fileName);
                }
            }
            foreach (string directory in directorys)
            {
                if (Directory.Exists(directory) && Directory.GetCreationTime(directory) < dtime)
                {
                    Directory.Delete(directory, true);
                }
            }
        }

        protected void btnExcel_Click(object sender, ImageClickEventArgs e)
        {
            string FilePath = System.Configuration.ConfigurationManager.AppSettings["FileExcelPath"];
            string FileName = PageHelper.GetQueryString("FileName");

            string imagesurl1 = FilePath + FileName;
            string tmpRootDir = Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string imagesurl2 = imagesurl1.Replace(tmpRootDir, "../../.."); //转换成相对路径

            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(System.IO.Path.Combine(imagesurl2, ""));
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "gb2312";
            string[] strs = FileName.Split('.');
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(strs[0])));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.BinaryWrite(workbook.SaveToStream().ToArray());
            Response.End();
        }
    }
}