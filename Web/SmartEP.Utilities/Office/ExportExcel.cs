
using System;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using Aspose.Cells;
using System.Data;
using System.Collections.Generic;
using SmartEP.Utilities.IO;

namespace SmartEP.Utilities.Office
{
    public class ExportExcel
    {

        protected void ExportData(string strContent, string FileName)
        {

            FileName = FileName + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "gb2312";
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //this.Page.EnableViewState = false; 
            // 添加头信息，为"文件下载/另存为"对话框指定默认文件名 
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ".xls");
            // 把文件流发送到客户端 
            HttpContext.Current.Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            HttpContext.Current.Response.Write(strContent);
            HttpContext.Current.Response.Write("</body></html>");
            // 停止页面的执行 
            //Response.End();
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="obj"></param>
        public void ExportData(GridView obj)
        {
            try
            {
                string style = "";
                if (obj.Rows.Count > 0)
                {
                    style = @"<style> .text { mso-number-format:\@; } </script> ";
                }
                else
                {
                    style = "no data.";
                }

                HttpContext.Current.Response.ClearContent();
                DateTime dt = DateTime.Now;
                string filename = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=ExportData" + filename + ".xls");
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Charset = "GB2312";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                obj.RenderControl(htw);
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 导出EXCEL并保存文件
        /// </summary>
        /// <param name="dataTableList"></param>
        /// <param name="TempPath"></param>
        /// <param name="SavePath"></param>
        /// <param name="SaveFileName"></param>
        /// <param name="IsBackup"></param>
        public void DataTableToExcel(List<DataTable> dataTableList, string TempPath, string SavePath, string SaveFileName, bool IsBackup = false)
        {
            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //创建一个workbookdesigner对象
            WorkbookDesigner designer = new WorkbookDesigner();

            //制定报表模板
            string path = System.IO.Path.Combine(TempPath);
            designer.Open(path);

            //设置Datatable对象 
            foreach (DataTable dt in dataTableList)
            {
                designer.SetDataSource(dt);
            }

            //根据数据源处理生成报表内容
            designer.Process();

            if (IsBackup)
            {
                //保存Excel文件
                string fileToSave = System.IO.Path.Combine(SavePath + "/" + SaveFileName);
                if (File.Exists(fileToSave))
                {
                    File.Delete(fileToSave);
                }
                else
                {
                    FileManager.CreateFile(SaveFileName, SavePath);
                }
                designer.Save(fileToSave, FileFormatType.Excel2003);

            }
            designer.Save(HttpUtility.UrlEncode(SaveFileName, System.Text.Encoding.UTF8), SaveType.OpenInExcel, FileFormatType.Excel2003, System.Web.HttpContext.Current.Response);
        }
    }
}