﻿/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using SmartEP.Utilities.IO.ExtensionMethods;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Globalization;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System.IO.Compression;
#endregion

namespace SmartEP.Utilities.Web.ExtensionMethods
{
    /// <summary>
    /// Set of HTML related extensions
    /// </summary>
    public static class HTMLExtensions
    {
        #region AbsoluteRoot

        /// <summary>
        /// Returns the absolute root
        /// </summary>
        public static Uri AbsoluteRoot(this HttpContext Context)
        {
            Context.ThrowIfNull("Context");
            if (Context.Items["absoluteurl"] == null)
                Context.Items["absoluteurl"] = new Uri(Context.Request.Url.GetLeftPart(UriPartial.Authority) + Context.RelativeRoot());
            return Context.Items["absoluteurl"] as Uri;
        }


        #endregion

        #region AddScriptFile

        /// <summary>
        /// Adds a script file to the header of the current page
        /// </summary>
        /// <param name="File">Script file</param>
        /// <param name="Directory">Script directory</param>
        public static void AddScriptFile(this System.Web.UI.Page Page, FileInfo File)
        {
            File.ThrowIfNull("File");
            if (!File.Exists)
                throw new ArgumentException("File does not exist");
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(typeof(System.Web.UI.Page), File.FullName))
                Page.ClientScript.RegisterClientScriptInclude(typeof(System.Web.UI.Page), File.FullName, File.FullName);
        }

        #endregion

        #region ContainsHTML

        /// <summary>
        /// Decides if the string contains HTML
        /// </summary>
        /// <param name="Input">Input string to check</param>
        /// <returns>false if it does not contain HTML, true otherwise</returns>
        public static bool ContainsHTML(this string Input)
        {
            return string.IsNullOrEmpty(Input) ? false : STRIP_HTML_REGEX.IsMatch(Input);
        }

        /// <summary>
        /// Decides if the file contains HTML
        /// </summary>
        /// <param name="Input">Input file to check</param>
        /// <returns>false if it does not contain HTML, true otherwise</returns>
        public static bool ContainsHTML(this FileInfo Input)
        {
            Input.ThrowIfNull("Input");
            return Input.Exists ? Input.Read().ContainsHTML() : false;
        }

        #endregion

        #region HTTPCompress

        /// <summary>
        /// Adds HTTP compression to the current context
        /// </summary>
        /// <param name="Context">Current context</param>
        public static void HTTPCompress(this HttpContext Context)
        {
            Context.ThrowIfNull("Context");
            if (Context.Request.UserAgent != null && Context.Request.UserAgent.Contains("MSIE 6"))
                return;

            if (Context.IsEncodingAccepted(GZIP))
            {
                Context.Response.Filter = new GZipStream(Context.Response.Filter, CompressionMode.Compress);
                Context.SetEncoding(GZIP);
            }
            else if (Context.IsEncodingAccepted(DEFLATE))
            {
                Context.Response.Filter = new DeflateStream(Context.Response.Filter, CompressionMode.Compress);
                Context.SetEncoding(DEFLATE);
            }
        }

        #endregion

        #region IsEncodingAccepted

        /// <summary>
        /// Checks the request headers to see if the specified
        /// encoding is accepted by the client.
        /// </summary>
        public static bool IsEncodingAccepted(this HttpContext Context, string Encoding)
        {
            if (Context == null)
                return false;
            return Context.Request.Headers["Accept-encoding"] != null && Context.Request.Headers["Accept-encoding"].Contains(Encoding);
        }

        #endregion

        #region RelativeRoot

        /// <summary>
        /// Gets the relative root of the web site
        /// </summary>
        /// <param name="Context">Current context</param>
        /// <returns>The relative root of the web site</returns>
        public static string RelativeRoot(this HttpContext Context)
        {
            return VirtualPathUtility.ToAbsolute("~/");
        }
        
        #endregion

        #region RemoveURLIllegalCharacters

        /// <summary>
        /// Removes illegal characters (used in uri's, etc.)
        /// </summary>
        /// <param name="Input">string to be converted</param>
        /// <returns>A stripped string</returns>
        public static string RemoveURLIllegalCharacters(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            Input = Input.Replace(":", string.Empty)
                        .Replace("/", string.Empty)
                        .Replace("?", string.Empty)
                        .Replace("#", string.Empty)
                        .Replace("[", string.Empty)
                        .Replace("]", string.Empty)
                        .Replace("@", string.Empty)
                        .Replace(".", string.Empty)
                        .Replace("\"", string.Empty)
                        .Replace("&", string.Empty)
                        .Replace("'", string.Empty)
                        .Replace(" ", "-");
            Input = RemoveExtraHyphen(Input);
            Input = RemoveDiacritics(Input);
            return Input.URLEncode().Replace("%", string.Empty);
        }

        #endregion

        #region SetEncoding

        /// <summary>
        /// Adds the specified encoding to the response headers.
        /// </summary>
        /// <param name="Encoding">Encoding to set</param>
        public static void SetEncoding(this HttpContext Context, string Encoding)
        {
            Context.ThrowIfNull("Context");
            Context.Response.AppendHeader("Content-encoding", Encoding);
        }

        #endregion

        #region StripHTML

        /// <summary>
        /// Removes HTML elements from a string
        /// </summary>
        /// <param name="HTML">HTML laiden string</param>
        /// <returns>HTML-less string</returns>
        public static string StripHTML(this string HTML)
        {
            if (string.IsNullOrEmpty(HTML))
                return "";
            HTML = STRIP_HTML_REGEX.Replace(HTML, string.Empty);
            return HTML.Replace("&nbsp;", " ")
                       .Replace("&#160;", string.Empty);
        }

        /// <summary>
        /// Removes HTML elements from a string
        /// </summary>
        /// <param name="HTML">HTML laiden file</param>
        /// <returns>HTML-less string</returns>
        public static string StripHTML(this FileInfo HTML)
        {
            HTML.ThrowIfNull("HTML");
            if (!HTML.Exists)
                throw new ArgumentException("File does not exist");
            return HTML.Read().StripHTML();
        }

        #endregion

        #region URLDecode

        /// <summary>
        /// URL decodes a string
        /// </summary>
        /// <param name="Input">Input to decode</param>
        /// <returns>A decoded string</returns>
        public static string URLDecode(this string Input)
        {
            if (Input.IsNullOrEmpty())
                return "";
            return HttpUtility.UrlDecode(Input);
        }

        #endregion

        #region URLEncode

        /// <summary>
        /// URL encodes a string
        /// </summary>
        /// <param name="Input">Input to encode</param>
        /// <returns>An encoded string</returns>
        public static string URLEncode(this string Input)
        {
            if (Input.IsNullOrEmpty())
                return "";
            return HttpUtility.UrlEncode(Input);
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Removes extra hyphens from a string
        /// </summary>
        /// <param name="Input">string to be stripped</param>
        /// <returns>Stripped string</returns>
        private static string RemoveExtraHyphen(string Input)
        {
            while (Input.Contains("--"))
                Input = Input.Replace("--", "-");
            return Input;
        }

        /// <summary>
        /// Removes special characters (Diacritics) from the string
        /// </summary>
        /// <param name="Input">String to strip</param>
        /// <returns>Stripped string</returns>
        private static string RemoveDiacritics(string Input)
        {
            string Normalized = Input.Normalize(NormalizationForm.FormD);
            StringBuilder Builder = new StringBuilder();
            for (int i = 0; i < Normalized.Length; i++)
            {
                Char TempChar = Normalized[i];
                if (CharUnicodeInfo.GetUnicodeCategory(TempChar) != UnicodeCategory.NonSpacingMark)
                    Builder.Append(TempChar);
            }
            return Builder.ToString();
        }

        #endregion

        #region Variables
        private static readonly Regex STRIP_HTML_REGEX = new Regex("<[^>]*>", RegexOptions.Compiled);
        #endregion

        #region Constants
        private const string GZIP = "gzip";
        private const string DEFLATE = "deflate";
        #endregion

        /// <summary>
        /// 转换为静态html
        /// </summary>
        public static void transHtml(string path, string outpath)
        {
            Page page = new Page();
            StringWriter writer = new StringWriter();
            page.Server.Execute(path, writer);
            FileStream fs;
            if (File.Exists(page.Server.MapPath("") + "\\" + outpath))
            {
                File.Delete(page.Server.MapPath("") + "\\" + outpath);
                fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            else
            {
                fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            byte[] bt = Encoding.Default.GetBytes(writer.ToString());
            fs.Write(bt, 0, bt.Length);
            fs.Close();
        }

        /// <summary>
        /// 替换html字符
        /// </summary>
        public static string EncodeHtml(string strHtml)
        {
            if (strHtml != "")
            {
                strHtml = strHtml.Replace(",", "&def");
                strHtml = strHtml.Replace("'", "&dot");
                strHtml = strHtml.Replace(";", "&dec");
                return strHtml;
            }
            return "";
        }

        /// <summary>
        /// 返回 HTML 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 返回 HTML 字符串的解码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            return Regex.Replace(content, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }

        /// <summary>
        /// 从HTML中获取文本,保留br,p,img
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string GetTextFromHTML(string HTML)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"</?(?!br|/?p|img)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return regEx.Replace(HTML, "");
        }

        #region HTML转行成TEXT
        /// <summary>
        /// HTML转行成TEXT
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string HtmlToTxt(string strHtml)
        {
            string[] aryReg ={
            @"<script[^>]*?>.*?</script>",
            @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
            @"([\r\n])[\s]+",
            @"&(quot|#34);",
            @"&(amp|#38);",
            @"&(lt|#60);",
            @"&(gt|#62);", 
            @"&(nbsp|#160);", 
            @"&(iexcl|#161);",
            @"&(cent|#162);",
            @"&(pound|#163);",
            @"&(copy|#169);",
            @"&#(\d+);",
            @"-->",
            @"<!--.*\n"
            };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, string.Empty);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");


            return strOutput;
        }
        #endregion
    }
}