using System;
using System.Text; 
using System.Net; 
using System.IO; 
using System.Threading;
using System.Text.RegularExpressions;

namespace SmartEP.Utilities.Web.UI
{

    public class HTMLHelper
    {
        #region 私有字段
        private static CookieContainer cc = new CookieContainer();
        private static string contentType = "application/x-www-form-urlencoded";
        private static string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
        private static string userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
        private static Encoding encoding = Encoding.GetEncoding("utf-8");
        private static int delay = 1000;
        private static int maxTry = 300;
        private static int currentTry = 0;
        #endregion

        #region 公有属性
        /// <summary> 
        /// Cookie
        /// </summary> 
        public static CookieContainer CookieContainer
        {
            get
            {
                return cc;
            }
        }

        /// <summary> 
        /// 语言
        /// </summary> 
        public static Encoding Encoding
        {
            get
            {
                return encoding;
            }
            set
            {
                encoding = value;
            }
        }

        public static int NetworkDelay
        {
            get
            {
                System.Random r = new System.Random();
                return (r.Next(delay, delay * 2));
            }
            set
            {
                delay = value;
            }
        }

        public static int MaxTry
        {
            get
            {
                return maxTry;
            }
            set
            {
                maxTry = value;
            }
        }
        #endregion

        #region 获取HTML
        /// <summary>
        /// 获取HTML
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postData">post 提交的字符串</param>
        /// <param name="isPost">是否是post</param>
        /// <param name="cookieContainer">CookieContainer</param>
        public static string GetHtml(string url, string postData, bool isPost, CookieContainer cookieContainer)
        {
            if (string.IsNullOrEmpty(postData)) return GetHtml(url, cookieContainer);
            Thread.Sleep(NetworkDelay);
            currentTry++;
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                byte[] byteRequest = Encoding.Default.GetBytes(postData);
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = contentType;
                httpWebRequest.ServicePoint.ConnectionLimit = maxTry;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = accept;
                httpWebRequest.UserAgent = userAgent;
                httpWebRequest.Method = isPost ? "POST" : "GET";
                httpWebRequest.ContentLength = byteRequest.Length;
                Stream stream = httpWebRequest.GetRequestStream();
                stream.Write(byteRequest, 0, byteRequest.Length);
                stream.Close();
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, encoding);
                string html = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                currentTry = 0;
                httpWebRequest.Abort();
                httpWebResponse.Close();
                return html;
            }
            catch (Exception e)
            {
                if (currentTry <= maxTry) GetHtml(url, postData, isPost, cookieContainer);
                currentTry--;
                if (httpWebRequest != null) httpWebRequest.Abort();
                if (httpWebResponse != null) httpWebResponse.Close();
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取HTML
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">CookieContainer</param>
        public static string GetHtml(string url, CookieContainer cookieContainer)
        {
            Thread.Sleep(NetworkDelay);
            currentTry++;
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = contentType;
                httpWebRequest.ServicePoint.ConnectionLimit = maxTry;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = accept;
                httpWebRequest.UserAgent = userAgent;
                httpWebRequest.Method = "GET";
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, encoding);
                string html = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                currentTry--;
                httpWebRequest.Abort();
                httpWebResponse.Close();
                return html;
            }
            catch (Exception e)
            {
                if (currentTry <= maxTry) GetHtml(url, cookieContainer);
                currentTry--;
                if (httpWebRequest != null) httpWebRequest.Abort();
                if (httpWebResponse != null) httpWebResponse.Close();
                return string.Empty;
            }
        }
        #endregion

        #region 获取字符流
        /// <summary>
        /// 获取字符流
        /// </summary>
        //---------------------------------------------------------------------------------------------------------------
        // 示例:
        // System.Net.CookieContainer cookie = new System.Net.CookieContainer(); 
        // Stream s = HttpHelper.GetStream("http://ptlogin2.qq.com/getimage?aid=15000102&0.43878429697395826", cookie);
        // picVerify.Image = Image.FromStream(s);
        //---------------------------------------------------------------------------------------------------------------
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">cookieContainer</param>
        public static Stream GetStream(string url, CookieContainer cookieContainer)
        {
            currentTry++;

            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = contentType;
                httpWebRequest.ServicePoint.ConnectionLimit = maxTry;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = accept;
                httpWebRequest.UserAgent = userAgent;
                httpWebRequest.Method = "GET";

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                currentTry--;
                return responseStream;
            }
            catch (Exception e)
            {
                if (currentTry <= maxTry)
                {
                    GetHtml(url, cookieContainer);
                }

                currentTry--;

                if (httpWebRequest != null)
                {
                    httpWebRequest.Abort();
                } if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
                return null;
            }
        }
        #endregion

        #region 清除HTML标记
        ///<summary>   
        ///清除HTML标记   
        ///</summary>   
        ///<param name="NoHTML">包括HTML的源码</param>   
        ///<returns>已经去除后的文字</returns>   
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML   
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            Htmlstring = regex.Replace(Htmlstring, "");
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");

            return Htmlstring;
        }
        #endregion

        #region 匹配页面的链接
        /// <summary>
        /// 获取页面的链接正则
        /// </summary>
        public string GetHref(string HtmlCode)
        {
            string MatchVale = "";
            string Reg = @"(h|H)(r|R)(e|E)(f|F) *= *('|"")?((\w|\\|\/|\.|:|-|_)+)[\S]*";
            foreach (Match m in Regex.Matches(HtmlCode, Reg))
            {
                MatchVale += (m.Value).ToLower().Replace("href=", "").Trim() + "|";
            }
            return MatchVale;
        }
        #endregion

        #region 匹配页面的图片地址
        /// <summary>
        /// 匹配页面的图片地址
        /// </summary>
        /// <param name="imgHttp">要补充的http://路径信息</param>
        public string GetImgSrc(string HtmlCode, string imgHttp)
        {
            string MatchVale = "";
            string Reg = @"<img.+?>";
            foreach (Match m in Regex.Matches(HtmlCode.ToLower(), Reg))
            {
                MatchVale += GetImg((m.Value).ToLower().Trim(), imgHttp) + "|";
            }

            return MatchVale;
        }

        /// <summary>
        /// 匹配<img src="" />中的图片路径实际链接
        /// </summary>
        /// <param name="ImgString"><img src="" />字符串</param>
        public string GetImg(string ImgString, string imgHttp)
        {
            string MatchVale = "";
            string Reg = @"src=.+\.(bmp|jpg|gif|png|)";
            foreach (Match m in Regex.Matches(ImgString.ToLower(), Reg))
            {
                MatchVale += (m.Value).ToLower().Trim().Replace("src=", "");
            }
            if (MatchVale.IndexOf(".net") != -1 || MatchVale.IndexOf(".com") != -1 || MatchVale.IndexOf(".org") != -1 || MatchVale.IndexOf(".cn") != -1 || MatchVale.IndexOf(".cc") != -1 || MatchVale.IndexOf(".info") != -1 || MatchVale.IndexOf(".biz") != -1 || MatchVale.IndexOf(".tv") != -1)
                return (MatchVale);
            else
                return (imgHttp + MatchVale);
        }
        #endregion

        #region 抓取远程页面内容
        /// <summary>
        /// 以GET方式抓取远程页面内容
        /// </summary>
        public static string Get_Http(string tUrl)
        {
            string strResult;
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(tUrl);
                hwr.Timeout = 19600;
                HttpWebResponse hwrs = (HttpWebResponse)hwr.GetResponse();
                Stream myStream = hwrs.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder sb = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    sb.Append(sr.ReadLine() + "\r\n");
                }
                strResult = sb.ToString();
                hwrs.Close();
            }
            catch (Exception ee)
            {
                strResult = ee.Message;
            }
            return strResult;
        }

        /// <summary>
        /// 以POST方式抓取远程页面内容
        /// </summary>
        /// <param name="postData">参数列表</param>
        public static string Post_Http(string url, string postData, string encodeType)
        {
            string strResult = null;
            try
            {
                Encoding encoding = Encoding.GetEncoding(encodeType);
                byte[] POST = encoding.GetBytes(postData);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = POST.Length;
                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(POST, 0, POST.Length); //设置POST
                newStream.Close();
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
                strResult = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            return strResult;
        }
        #endregion

        #region 压缩HTML输出
        /// <summary>
        /// 压缩HTML输出
        /// </summary>
        public static string ZipHtml(string Html)
        {
            Html = Regex.Replace(Html, @">\s+?<", "><");//去除HTML中的空白字符
            Html = Regex.Replace(Html, @"\r\n\s*", "");
            Html = Regex.Replace(Html, @"<body([\s|\S]*?)>([\s|\S]*?)</body>", @"<body$1>$2</body>", RegexOptions.IgnoreCase);
            return Html;
        }
        #endregion

        #region 过滤指定HTML标签
        /// <summary>
        /// 过滤指定HTML标签
        /// </summary>
        /// <param name="s_TextStr">要过滤的字符</param>
        /// <param name="html_Str">a img p div</param>
        public static string DelHtml(string s_TextStr, string html_Str)
        {
            string rStr = "";
            if (!string.IsNullOrEmpty(s_TextStr))
            {
                rStr = Regex.Replace(s_TextStr, "<" + html_Str + "[^>]*>", "", RegexOptions.IgnoreCase);
                rStr = Regex.Replace(rStr, "</" + html_Str + ">", "", RegexOptions.IgnoreCase);
            }
            return rStr;
        }
        #endregion

        #region 加载文件块
        /// <summary>
        /// 加载文件块
        /// </summary>
        public static string File(string Path, System.Web.UI.Page p)
        {
            return @p.ResolveUrl(Path);
        }
        #endregion

        #region 加载CSS样式文件
        /// <summary>
        /// 加载CSS样式文件
        /// </summary>
        public static string CSS(string cssPath, System.Web.UI.Page p)
        {
            return @"<link href=""" + p.ResolveUrl(cssPath) + @""" rel=""stylesheet"" type=""text/css"" />" + "\r\n";
        }
        #endregion

        #region 加载JavaScript脚本文件
        /// <summary>
        /// 加载javascript脚本文件
        /// </summary>
        public static string JS(string jsPath, System.Web.UI.Page p)
        {
            return @"<script type=""text/javascript"" src=""" + p.ResolveUrl(jsPath) + @"""></script>" + "\r\n";
        }
        #endregion

        #region 分页
        public static string paging(string url, string para, int sumpage, int page)
        {
            string result = string.Empty;
            if (sumpage == 1)
            {
                return result;
            }
            if (sumpage > 500)
            {
                sumpage = 500;
            }
            if (page > sumpage)
            {
                page = 1;
            }
            StringBuilder sb = new StringBuilder();
            if (sumpage > 0)
            {
                switch (page)
                {
                    case 1:
                        sb.Append(string.Format("<p class=\"next\"><a href=\"{0}?page={1}{2}\">{3}</a> ", new object[] { url, page + 1, para, "下一页" }));
                        break;
                    default:
                        if (sumpage == page)
                        {
                            sb.Append(string.Format("<p class=\"next\"><a href=\"{0}?page={1}{2}\">{3}</a> ", new object[] { url, page - 1, para, "上一页" }));
                        }
                        else
                        {
                            sb.Append(string.Format("<p class=\"next\"><a href=\"{0}?page={1}{2}\">{3}</a> <a href=\"{4}?page={5}{6}\">{7}</a> ",
                                new object[] { url, page + 1, para, "下一页", url, page - 1, para, "上一页" }));
                        }
                        break;
                }
                sb.Append(string.Format("第{0}/{1}页</p>", new object[] { page, sumpage }));
            }
            return sb.ToString();
        }

        public static string paging(string url, string para, int sumpage, int page, System.Web.UI.UserControl myPaging)
        {
            myPaging.Visible = false;
            string result = string.Empty;
            if (sumpage == 1)
            {
                return result;
            }
            if (sumpage > 500)
            {
                sumpage = 500;
            }
            if (page > sumpage)
            {
                page = 1;
            }
            StringBuilder sb = new StringBuilder();
            if (sumpage > 0)
            {
                myPaging.Visible = true;
                switch (page)
                {
                    case 1:
                        sb.Append(string.Format("<a href=\"{0}?page={1}{2}\">{3}</a> ", new object[] { url, page + 1, para, "下一页" }));
                        break;
                    default:
                        if (sumpage == page)
                        {
                            sb.Append(string.Format("<a href=\"{0}?page={1}{2}\">{3}</a> ", new object[] { url, page - 1, para, "上一页" }));
                        }
                        else
                        {
                            sb.Append(string.Format("<a href=\"{0}?page={1}{2}\">{3}</a> <a href=\"{4}?page={5}{6}\">{7}</a> ",
                                new object[] { url, page + 1, para, "下一页", url, page - 1, para, "上一页" }));
                        }
                        break;
                }
                sb.Append(string.Format("第{0}/{1}页", new object[] { page, sumpage }));
            }
            return sb.ToString();
        }

        public static string paging(string para, int sumpage, int page, int count)
        {
            string result = string.Empty;
            if (page > sumpage)
            {
                page = 1;
            }
            StringBuilder sb = new StringBuilder();
            if (sumpage > 0)
            {
                if (sumpage != 1)
                {
                    switch (page)
                    {
                        case 1:
                            sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a> ", new object[] { page + 1, para, "下一页" }));
                            break;
                        default:
                            if (sumpage == page)
                            {
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a> ", new object[] { page - 1, para, "上一页" }));
                            }
                            else
                            {
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a> <a href=\"?page={3}{4}\">{5}</a> ",
                                    new object[] { page - 1, para, "上一页", page + 1, para, "下一页" }));
                            }
                            break;
                    }
                }
                sb.Append(string.Format("第{0}/{1}页 共{2}条", new object[] { page, sumpage, count }));
            }
            return sb.ToString();
        }

        public static void paging(string clinktail, int sumpage, int page, System.Web.UI.WebControls.Label page_view)
        {
            if (sumpage > 0)
            {
                int n = sumpage;    //总页数
                int x = page;   //得到当前页
                int i;
                int endpage;
                string pageview = "", pageviewtop = "";
                if (x > 1)
                {
                    pageview += "&nbsp;&nbsp;<a class='pl' href='?page=1" + clinktail + "'>第1页</a> | ";
                    pageviewtop += "&nbsp;&nbsp;<a class='pl' href='?page=1" + clinktail + "'>第1页</a> | ";
                }
                else
                {
                    pageview += "&nbsp;&nbsp;<font color='#666666'> 第1页 </font> | ";
                    pageviewtop += "&nbsp;&nbsp;<font color='#666666'> 第1页 </font> | ";
                }

                if (x > 1)
                {
                    pageviewtop += " <a class='pl' href='?page=" + (x - 1) + "" + clinktail + "'>上1页</a> ";
                }
                else
                {
                    pageviewtop += " <font color='#666666'>上1页</font> ";
                }

                if (x > ((x - 1) / 10) * 10 && x > 10)
                {
                    pageview += "<a class='pl' href='?page=" + ((x - 1) / 10) * 10 + "" + clinktail + "' onclink='return false;'>上10页</a>";
                }

                //if (((x-1) / 10) * 10 + 10) >= n )
                if (((x - 1) / 10) * 10 + 10 >= n)
                {
                    endpage = n;
                }
                else
                {
                    endpage = ((x - 1) / 10) * 10 + 10;
                }

                for (i = ((x - 1) / 10) * 10 + 1; i <= endpage; ++i)
                {
                    if (i == x)
                    {
                        pageview += " <font color='#FF0000'><b>" + i + "</b></font>";
                    }
                    else
                    {
                        pageview += " <a class='pl' href='?page=" + i + "" + clinktail + "'>" + i + "</a>";
                    }
                }

                if (x < n)
                {
                    pageviewtop += " <a class='pl' href='?page=" + (x + 1) + "" + clinktail + "'>下1页</a> ";
                }
                else
                {
                    pageviewtop += " <font color='#666666'>下1页</font> ";
                }

                if (endpage != n)
                {
                    pageview += " <a class='pl' href='?page=" + (endpage + 1) + "" + clinktail + "' class='pl' onclink='return false;'>下10页</a> | ";
                }
                else
                {
                    pageview += " | ";
                }
                if (x < n)
                {
                    pageview += " <a class='pl' href='?page=" + n + "" + clinktail + "' class='pl'>第" + n + "页</a> ";
                    pageviewtop += " |  <a class='pl' href='?page=" + n + "" + clinktail + "' class='pl'>第" + n + "页</a> ";
                }
                else
                {
                    pageview += "<font color='#666666'> 第" + n + "页 </font>";
                    pageviewtop += " | <font color='#666666'> 第" + n + "页 </font>";
                }
                page_view.Text = pageview.ToString();
            }
            else
            {
                page_view.Text = "";
            }
        }

        //带第一页和最后一页
        public static string paging2(string para, int sumpage, int page, int count)
        {
            string result = string.Empty;
            if (page > sumpage)
            {
                page = 1;
            }
            StringBuilder sb = new StringBuilder();
            if (sumpage > 0)
            {
                if (sumpage != 1)
                {
                    //第一页
                    sb.Append(string.Format("<a href=\"?page={0}{1}\"><img src=\"images/first-icon.gif\" border=\"0\"/></a>&nbsp;&nbsp;", new object[] { 1, para }));
                    switch (page)
                    {
                        case 1:
                            //前一页图片
                            sb.Append(string.Format("<a>{0}</a>", new object[] { "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                            sb.Append(string.Format("<a>上一页</a><a href=\"?page={0}{1}\">{2}</a> ", new object[] { page + 1, para, "下一页" }));
                            //后一页图片
                            sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page + 1, para, "<img src=\"images/right-icon.gif\" border=\"0\"/>" }));
                            break;
                        default:
                            if (sumpage == page)
                            {
                                //前一页图片
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page - 1, para, "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a><a>下一页</a> ", new object[] { page - 1, para, "上一页" }));
                                //后一页图片
                                sb.Append(string.Format("<a>{0}</a>", new object[] { "<img src=\"images/right-icon.gif\" />" }));
                            }
                            else
                            {
                                //前一页图片
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page - 1, para, "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a> <a href=\"?page={3}{4}\">{5}</a> ",
                                    new object[] { page - 1, para, "上一页", page + 1, para, "下一页" }));
                                //后一页图片
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page + 1, para, "<img src=\"images/right-icon.gif\" border=\"0\"/>" }));
                            }
                            break;
                    }
                    //最后一页图片
                    sb.Append(string.Format("&nbsp;&nbsp;<a href=\"?page={0}{1}\"><img src=\"images/last-icon.gif\" border=\"0\"/></a>&nbsp;&nbsp;", new object[] { sumpage, para }));
                }
                sb.Append(string.Format("第{0}页/共{1}页 共{2}条", new object[] { page, sumpage, count }));
            }
            return sb.ToString();
        }

        public static string paging3(string url, string para, int sumpage, int page, int count)
        {
            string result = string.Empty;
            if (page > sumpage)
            {
                page = 1;
            }
            StringBuilder sb = new StringBuilder();
            if (sumpage > 0)
            {
                if (sumpage != 1)
                {
                    //第一页
                    sb.Append(string.Format("<a href=\"{2}?page={0}{1}\">首页</a>", new object[] { 1, para, url }));
                    switch (page)
                    {
                        case 1:
                            //前一页图片
                            // sb.Append(string.Format("<a>{0}</a>", new object[] { "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                            sb.Append(string.Format("<a>上一页</a><a href=\"{3}?page={0}{1}\">{2}</a> ", new object[] { page + 1, para, "下一页", url }));
                            //后一页图片
                            // sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page + 1, para, "<img src=\"images/right-icon.gif\" border=\"0\"/>" }));
                            break;
                        default:
                            if (sumpage == page)
                            {
                                //前一页图片
                                //sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page - 1, para, "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                                sb.Append(string.Format("<a href=\"{3}?page={0}{1}\">{2}</a><a>下一页</a> ", new object[] { page - 1, para, "上一页", url }));
                                //后一页图片
                                //sb.Append(string.Format("<a>{0}</a>", new object[] { "<img src=\"images/right-icon.gif\" />" }));
                            }
                            else
                            {
                                //前一页图片
                                //sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page - 1, para, "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                                sb.Append(string.Format("<a href=\"{6}?page={0}{1}\">{2}</a> <a href=\"{6}?page={3}{4}\">{5}</a> ",
                                    new object[] { page - 1, para, "上一页", page + 1, para, "下一页", url }));
                                //后一页图片
                                //sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page + 1, para, "<img src=\"images/right-icon.gif\" border=\"0\"/>" }));
                            }
                            break;
                    }
                    //最后一页图片
                    sb.Append(string.Format("<a href=\"{2}?page={0}{1}\">末页</a>&nbsp;&nbsp;", new object[] { sumpage, para, url }));
                }
                sb.Append(string.Format("第{0}页/共{1}页 共{2}条", new object[] { page, sumpage, count }));
            }
            return sb.ToString();
        }
        #endregion
    }
}
