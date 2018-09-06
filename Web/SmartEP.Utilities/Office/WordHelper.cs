using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.Utilities.Office
{
    public class WordHelper
    {
        /// <summary>
        /// DataTable导出到Word
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="fileName"></param>
        public static void DataTableToWord(System.Data.DataTable dtData, String fileName, Page page)
        {
            fileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            //完全路径
            String savePath = page.Server.MapPath("~/") + "\\Files\\TempFile\\Word\\" + fileName;
            if (CreateWord(dtData, savePath))
            {
                FileInfo DownloadFile = new FileInfo(savePath);
                if (DownloadFile.Exists)
                {
                    HttpResponse curResponse = HttpContext.Current.Response;
                    curResponse.Clear();
                    curResponse.ClearHeaders();
                    curResponse.Buffer = false;
                    curResponse.ContentType = "application/octet-stream";
                    curResponse.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFile.Name, System.Text.Encoding.UTF8));
                    curResponse.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                    curResponse.WriteFile(DownloadFile.FullName);
                    curResponse.Flush();
                    if (File.Exists(savePath)) File.Delete(savePath);
                    curResponse.End();
                }
            }
        }

        /// <summary>
        /// 创建table到制定路径的Word文件
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="fileName">路径名(包含后缀doc的文件路径)</param>
        /// <returns></returns>
        public static bool CreateWord(System.Data.DataTable dtData, String fileName)
        {
            try
            {
                Aspose.Words.Document doc = new Aspose.Words.Document();
                Aspose.Words.DocumentBuilder builder = new Aspose.Words.DocumentBuilder(doc);
                builder.RowFormat.HeadingFormat = true;
                builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
                //添加标题
                for (var iCol = 0; iCol < dtData.Columns.Count; iCol++)
                {
                    builder.InsertCell();// 添加一个单元格
                    builder.CellFormat.Borders.LineStyle = Aspose.Words.LineStyle.Single;
                    builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                    //垂直居中对齐
                    builder.CellFormat.VerticalAlignment = Aspose.Words.Tables.CellVerticalAlignment.Center;
                    //水平居中对齐
                    builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
                    builder.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                    builder.Write(dtData.Columns[iCol].ColumnName);
                }
                builder.EndRow();
                for (var i = 0; i < dtData.Rows.Count; i++)
                {
                    for (var j = 0; j < dtData.Columns.Count; j++)
                    {
                        //添加一个单元格
                        builder.InsertCell();
                        builder.CellFormat.Borders.LineStyle = Aspose.Words.LineStyle.Single;
                        builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                        //垂直居中对齐
                        builder.CellFormat.VerticalAlignment = Aspose.Words.Tables.CellVerticalAlignment.Center;
                        //水平居中对齐
                        builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
                        builder.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                        builder.Write(dtData.Rows[i][j].ToString());
                    }
                    builder.EndRow();
                }
                builder.EndTable();
                doc.Save(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
