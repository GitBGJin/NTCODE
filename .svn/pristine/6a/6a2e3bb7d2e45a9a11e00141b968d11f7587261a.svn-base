using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace AQIWS.Common
{
    public class CommonFunction
    {
        /// <summary>
         /// 获取指定周数的开始日期和结束日期，开始日期为周日
         /// </summary>
         /// <param name="year">年份</param>
         /// <param name="index">周数</param>
         /// <param name="first">当此方法返回时，则包含参数 year 和 index 指定的周的开始日期的 System.DateTime 值；如果失败，则为 System.DateTime.MinValue。</param>
         /// <param name="last">当此方法返回时，则包含参数 year 和 index 指定的周的结束日期的 System.DateTime 值；如果失败，则为 System.DateTime.MinValue。</param>
         /// <returns></returns>
         public static bool GetDaysOfWeeks(int year, int week, out DateTime first, out DateTime last)
         {
             first = DateTime.MinValue;
             last = DateTime.MinValue;
             if (year < 1700 || year > 9999)
             {
                 //"年份超限"
                 return false;
             }
             if (week < 1 || week > 53)
             {
                 //"周数错误"
                 return false;
             }
            DateTime firstDay = new DateTime(year, 1, 1);
            int add = 0;

        switch (firstDay.DayOfWeek)
        {
            case DayOfWeek.Monday:
                add = -1;
                break;
            case DayOfWeek.Tuesday:
                add = -2;
                break;
            case DayOfWeek.Wednesday:
                add = -3;
                break;
            case DayOfWeek.Thursday:
                add = -4;
                break;
            case DayOfWeek.Friday:
                add = -5;
                break;
            case DayOfWeek.Saturday:
                add = -6;
                break;
            case DayOfWeek.Sunday:
                add = 0;
                break;
        }

        first = new DateTime(year, 1, 1).AddDays((week - 1) * 7).AddDays(add);
        last = new DateTime(year, 1, 1).AddDays((week * 7) - 1).AddDays(add);
          //first = DateTime.MinValue;
          //last = DateTime.MinValue;
          //if (year < 1700 || year > 9999)
          //{
          //    //"年份超限"
          //    return false;
             //}
             //if (index < 1 || index > 53)
             //{
             //    //"周数错误"
             //    return false;
             //}
             //DateTime startDay = new DateTime(year, 1, 1);  //该年第一天
             //DateTime endDay = new DateTime(year + 1, 1, 1).AddMilliseconds(-1);
             //int dayOfWeek = 0;
             //if (Convert.ToInt32(startDay.DayOfWeek.ToString("d")) > 0)
             //    dayOfWeek = Convert.ToInt32(startDay.DayOfWeek.ToString("d"));  //该年第一天为星期几
             //if (dayOfWeek == 7) { dayOfWeek = 0; }
             //if (index == 1)
             //{
             //    first = startDay;
             //    if (dayOfWeek == 6)
             //    {
             //        last = first;
             //    }
             //    else
             //    {
             //        last = startDay.AddDays((6-dayOfWeek));
             //    }
             //}
             //else
             //{
             //    first = startDay.AddDays((7 - dayOfWeek) + (index - 2) * 7); //index周的起始日期
             //    last = first.AddDays(7);
             //    if (last > endDay)
             //    {
             //        last = endDay;
             //    }
             //}
             //if (first > endDay)  //startDayOfWeeks不在该年范围内
             //{
             //    //"输入周数大于本年最大周数";
             //    return false;
             //}            
             return true;
         }

         public static void WriteInfoLog(string message)
         {
             WriteLog("INFO", message);
         }

         public static void WriteErrorLog(string message)
         {
             WriteLog("ERROR", message);
         }

         /// <summary>
         /// 日志输出,按天生成文件
         /// </summary>
         /// <param name="logLevel">日志级别</param>
         /// <param name="message">日志信息</param>
         private static void WriteLog(string logLevel, string message)
         {
             //日志路径
             String logPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Logs\\";
             string logFile = logPath + "WebServiceForMobile_" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";

             if (!Directory.Exists(logPath))
             {// 目录不存在先创建目录
                 Directory.CreateDirectory(logPath);
             }

             if (!File.Exists(logFile))
             {// 文件不存在先创建文件
                 File.Create(logFile).Close();
             }

             StreamWriter sw = File.AppendText(logFile);
             sw.WriteLine(string.Format(@"{0}|{1}|{2}"
                                        , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                        , logLevel
                                        , message));
             sw.Close();
             sw.Dispose();
         }
    }
}