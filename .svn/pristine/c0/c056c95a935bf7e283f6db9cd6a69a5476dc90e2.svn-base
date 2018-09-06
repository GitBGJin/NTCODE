
#region Usings
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace SmartEP.Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// DateTime extension methods
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Extension Methods

        #region AddWeeks

        /// <summary>
        /// Adds the number of weeks to the date
        /// </summary>
        /// <param name="Date">Date input</param>
        /// <param name="NumberOfWeeks">Number of weeks to add</param>
        /// <returns>The date after the number of weeks are added</returns>
        public static DateTime AddWeeks(this DateTime Date,int NumberOfWeeks)
        {
            return Date.AddDays(NumberOfWeeks * 7);
        }

        #endregion

        #region Age

        /// <summary>
        /// Calculates age based on date supplied
        /// </summary>
        /// <param name="Date">Birth date</param>
        /// <param name="CalculateFrom">Date to calculate from</param>
        /// <returns>The total age in years</returns>
        public static int Age(this DateTime Date, DateTime CalculateFrom = default(DateTime))
        {
            if (CalculateFrom.IsDefault())
                CalculateFrom = DateTime.Now;
            return (CalculateFrom-Date).Years();
        }

        #endregion

        #region BeginningOfDay

        /// <summary>
        /// Returns the beginning of the day
        /// </summary>
        /// <param name="Input">Input date</param>
        /// <returns>The beginning of the day</returns>
        public static DateTime BeginningOfDay(this DateTime Input)
        {
            return new DateTime(Input.Year, Input.Month, Input.Day, 0, 0, 0);
        }

        #endregion

        #region ConvertToTimeZone

        /// <summary>
        /// Converts a DateTime to a specific time zone
        /// </summary>
        /// <param name="Date">DateTime to convert</param>
        /// <param name="TimeZone">Time zone to convert to</param>
        /// <returns>The converted DateTime</returns>
        public static DateTime ConvertToTimeZone(this DateTime Date, TimeZoneInfo TimeZone)
        {
            return TimeZoneInfo.ConvertTime(Date, TimeZone);
        }

        #endregion

        #region DaysInMonth

        /// <summary>
        /// Returns the number of days in the month
        /// </summary>
        /// <param name="Date">Date to get the month from</param>
        /// <returns>The number of days in the month</returns>
        public static int DaysInMonth(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return Date.LastDayOfMonth().Day;
        }

        #endregion

        #region DaysLeftInMonth

        /// <summary>
        /// Gets the number of days left in the month based on the date passed in
        /// </summary>
        /// <param name="Date">The date to check against</param>
        /// <returns>The number of days left in a month</returns>
        public static int DaysLeftInMonth(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return Thread.CurrentThread.CurrentCulture.Calendar.GetDaysInMonth(Date.Year, Date.Month) - Date.Day;
        }

        #endregion

        #region DaysLeftInYear

        /// <summary>
        /// Gets the number of days left in a year based on the date passed in
        /// </summary>
        /// <param name="Date">The date to check against</param>
        /// <returns>The number of days left in a year</returns>
        public static int DaysLeftInYear(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return Thread.CurrentThread.CurrentCulture.Calendar.GetDaysInYear(Date.Year) - Date.DayOfYear;
        }

        #endregion

        #region DaysLeftInWeek

        /// <summary>
        /// Gets the number of days left in a week
        /// </summary>
        /// <param name="Date">The date to check against</param>
        /// <returns>The number of days left in a week</returns>
        public static int DaysLeftInWeek(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return 7 - ((int)Date.DayOfWeek + 1);
        }

        #endregion

        #region EndOfDay

        /// <summary>
        /// Returns the end of the day
        /// </summary>
        /// <param name="Input">Input date</param>
        /// <returns>The end of the day</returns>
        public static DateTime EndOfDay(this DateTime Input)
        {
            return new DateTime(Input.Year, Input.Month, Input.Day, 23, 59, 59);
        }

        #endregion

        #region FirstDayOfMonth

        /// <summary>
        /// Returns the first day of a month based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the first day of the month from</param>
        /// <returns>The first day of the month</returns>
        public static DateTime FirstDayOfMonth(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return new DateTime(Date.Year, Date.Month, 1);
        }

        #endregion

        #region FirstDayOfQuarter

        /// <summary>
        /// Returns the first day of a quarter based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the first day of the quarter from</param>
        /// <param name="Quarter1Start">Beginning of the first quarter (defaults to the beginning of the year)</param>
        /// <returns>The first day of the quarter</returns>
        public static DateTime FirstDayOfQuarter(this DateTime Date, DateTime Quarter1Start = default(DateTime))
        {
            Date.ThrowIfNull("Date");
            if (Quarter1Start.IsDefault())
                Quarter1Start = Date.FirstDayOfYear();
            if (Date.Between(Quarter1Start, Quarter1Start.AddMonths(3).AddDays(-1).EndOfDay()))
                return Quarter1Start.Date;
            else if (Date.Between(Quarter1Start.AddMonths(3), Quarter1Start.AddMonths(6).AddDays(-1).EndOfDay()))
                return Quarter1Start.AddMonths(3).Date;
            else if (Date.Between(Quarter1Start.AddMonths(6), Quarter1Start.AddMonths(9).AddDays(-1).EndOfDay()))
                return Quarter1Start.AddMonths(6).Date;
            return Quarter1Start.AddMonths(9).Date;
        }

        #endregion

        #region FirstDayOfWeek

        /// <summary>
        /// Returns the first day of a week based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the first day of the week from</param>
        /// <param name="CultureInfo">The culture to use (defaults to current culture)</param>
        /// <returns>The first day of the week</returns>
        public static DateTime FirstDayOfWeek(this DateTime Date,CultureInfo CultureInfo=null)
        {
            Date.ThrowIfNull("Date");
            return Date.AddDays(CultureInfo.NullCheck(CultureInfo.CurrentCulture).DateTimeFormat.FirstDayOfWeek - Date.DayOfWeek).Date;
        }

        #endregion

        #region FirstDayOfYear

        /// <summary>
        /// Returns the first day of a year based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the first day of the year from</param>
        /// <returns>The first day of the year</returns>
        public static DateTime FirstDayOfYear(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return new DateTime(Date.Year, 1, 1);
        }

        #endregion

        #region FromUnixTime

        /// <summary>
        /// Returns the Unix based date as a DateTime object
        /// </summary>
        /// <param name="Date">Unix date to convert</param>
        /// <returns>The Unix Date in DateTime format</returns>
        public static DateTime FromUnixTime(this int Date)
        {
            return new DateTime((Date * TimeSpan.TicksPerSecond) + new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns the Unix based date as a DateTime object
        /// </summary>
        /// <param name="Date">Unix date to convert</param>
        /// <returns>The Unix Date in DateTime format</returns>
        public static DateTime FromUnixTime(this long Date)
        {
            return new DateTime((Date * TimeSpan.TicksPerSecond) + new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks, DateTimeKind.Utc);
        }

        #endregion

        #region IsInFuture

        /// <summary>
        /// Determines if the date is some time in the future
        /// </summary>
        /// <param name="Date">Date to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsInFuture(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return DateTime.Now < Date;
        }

        #endregion

        #region IsInPast

        /// <summary>
        /// Determines if the date is some time in the past
        /// </summary>
        /// <param name="Date">Date to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsInPast(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return DateTime.Now > Date;
        }

        #endregion

        #region IsToday

        /// <summary>
        /// Is this today?
        /// </summary>
        /// <param name="Date">Date to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsToday(this DateTime Date)
        {
            return (Date.Date.BeginningOfDay() == DateTime.Today);
        }

        #endregion

        #region IsWeekDay

        /// <summary>
        /// Determines if this is a week day
        /// </summary>
        /// <param name="Date">Date to check against</param>
        /// <returns>Whether this is a week day or not</returns>
        public static bool IsWeekDay(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return (int)Date.DayOfWeek < 6 && (int)Date.DayOfWeek > 0;
        }

        #endregion

        #region IsWeekEnd

        /// <summary>
        /// Determines if this is a week end
        /// </summary>
        /// <param name="Date">Date to check against</param>
        /// <returns>Whether this is a week end or not</returns>
        public static bool IsWeekEnd(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return !IsWeekDay(Date);
        }

        #endregion

        #region LastDayOfMonth

        /// <summary>
        /// Returns the last day of the month based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the last day from</param>
        /// <returns>The last day of the month</returns>
        public static DateTime LastDayOfMonth(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return Date.AddMonths(1).FirstDayOfMonth().AddDays(-1).Date;
        }

        #endregion

        #region LastDayOfQuarter

        /// <summary>
        /// Returns the last day of a quarter based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the last day of the quarter from</param>
        /// <param name="Quarter1Start">Beginning of the first quarter (defaults to the beginning of the year)</param>
        /// <returns>The last day of the quarter</returns>
        public static DateTime LastDayOfQuarter(this DateTime Date, DateTime Quarter1Start = default(DateTime))
        {
            Date.ThrowIfNull("Date");
            if (Quarter1Start.IsDefault())
                Quarter1Start = Date.FirstDayOfYear();
            if (Date.Between(Quarter1Start, Quarter1Start.AddMonths(3).AddDays(-1).EndOfDay()))
                return Quarter1Start.AddMonths(3).AddDays(-1).Date;
            else if (Date.Between(Quarter1Start.AddMonths(3), Quarter1Start.AddMonths(6).AddDays(-1).EndOfDay()))
                return Quarter1Start.AddMonths(6).AddDays(-1).Date;
            else if (Date.Between(Quarter1Start.AddMonths(6), Quarter1Start.AddMonths(9).AddDays(-1).EndOfDay()))
                return Quarter1Start.AddMonths(9).AddDays(-1).Date;
            return Quarter1Start.AddYears(1).AddDays(-1).Date;
        }

        #endregion

        #region LastDayOfWeek

        /// <summary>
        /// Returns the last day of a week based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the last day of the week from</param>
        /// <param name="CultureInfo">The culture to use (defaults to current culture)</param>
        /// <returns>The last day of the week</returns>
        public static DateTime LastDayOfWeek(this DateTime Date, CultureInfo CultureInfo = null)
        {
            Date.ThrowIfNull("Date");
            return Date.FirstDayOfWeek(CultureInfo.NullCheck(CultureInfo.CurrentCulture)).AddDays(6);
        }

        #endregion

        #region LastDayOfYear

        /// <summary>
        /// Returns the last day of the year based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the last day from</param>
        /// <returns>The last day of the year</returns>
        public static DateTime LastDayOfYear(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return new DateTime(Date.Year, 12, 31);
        }

        #endregion

        #region LocalTimeZone

        /// <summary>
        /// Gets the local time zone
        /// </summary>
        /// <param name="Date">Date object</param>
        /// <returns>The local time zone</returns>
        public static TimeZoneInfo LocalTimeZone(this DateTime Date)
        {
            return TimeZoneInfo.Local;
        }

        #endregion

        #region ToUnix

        /// <summary>
        /// Returns the date in Unix format
        /// </summary>
        /// <param name="Date">Date to convert</param>
        /// <returns>The date in Unix format</returns>
        public static int ToUnix(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return (int)((Date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks / TimeSpan.TicksPerSecond);
        }

        #endregion

        #region SetTime

        /// <summary>
        /// Sets the time portion of a specific date
        /// </summary>
        /// <param name="Date">Date input</param>
        /// <param name="Hour">Hour to set</param>
        /// <param name="Minutes">Minutes to set</param>
        /// <param name="Seconds">Seconds to set</param>
        /// <returns>Sets the time portion of the specified date</returns>
        public static DateTime SetTime(this DateTime Date, int Hour, int Minutes, int Seconds)
        {
            return Date.SetTime(new TimeSpan(Hour, Minutes, Seconds));
        }

        /// <summary>
        /// Sets the time portion of a specific date
        /// </summary>
        /// <param name="Date">Date input</param>
        /// <param name="Time">Time to set</param>
        /// <returns>Sets the time portion of the specified date</returns>
        public static DateTime SetTime(this DateTime Date, TimeSpan Time)
        {
            return Date.Date.Add(Time);
        }

        #endregion

        #region UTCOffset

        /// <summary>
        /// Gets the UTC offset
        /// </summary>
        /// <param name="Date">Date to get the offset of</param>
        /// <returns>UTC offset</returns>
        public static double UTCOffset(this DateTime Date)
        {
            return (Date - Date.ToUniversalTime()).TotalHours;
        }

        #endregion

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return StrToDateTime(obj.ToString());
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            return StrToDateTime(obj.ToString(), defValue);
        }

        /// <summary>
        /// 转换时间为unix时间戳
        /// </summary>
        /// <param name="date">需要传递UTC时间,避免时区误差,例:DataTime.UTCNow</param>
        /// <returns></returns>
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        /// <summary>
        /// 将时间格式化成 年月日 的形式,如果时间为null，返回当前系统时间
        /// </summary>
        /// <param name="dt">年月日分隔符</param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static string GetFormatDate(DateTime dt, char Separator)
        {
            if (dt != null && !dt.Equals(DBNull.Value))
            {
                string tem = string.Format("yyyy{0}MM{1}dd", Separator, Separator);
                return dt.ToString(tem);
            }
            else
            {
                return GetFormatDate(DateTime.Now, Separator);
            }
        }
        /// <summary>
        /// 将时间格式化成 时分秒 的形式,如果时间为null，返回当前系统时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static string GetFormatTime(DateTime dt, char Separator)
        {
            if (dt != null && !dt.Equals(DBNull.Value))
            {
                string tem = string.Format("hh{0}mm{1}ss", Separator, Separator);
                return dt.ToString(tem);
            }
            else
            {
                return GetFormatDate(DateTime.Now, Separator);
            }
        }
        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int SecondToMinute(int Second)
        {
            decimal mm = (decimal)((decimal)Second / (decimal)60);
            return Convert.ToInt32(Math.Ceiling(mm));
        }

        #region 返回某年某月最后一天
        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate(int year, int month)
        {
            DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth(year, month));
            int Day = lastDay.Day;
            return Day;
        }
        #endregion

        #region 返回时间差
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts = DateTime2 - DateTime1;
                if (ts.Days >= 1)
                {
                    dateDiff = DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        dateDiff = ts.Hours.ToString() + "小时前";
                    }
                    else
                    {
                        dateDiff = ts.Minutes.ToString() + "分钟前";
                    }
                }
            }
            catch
            { }
            return dateDiff;
        }
        #endregion

        #region 获得两个日期的间隔
        /// <summary>
        /// 获得两个日期的间隔
        /// </summary>
        /// <param name="DateTime1">日期一。</param>
        /// <param name="DateTime2">日期二。</param>
        /// <returns>日期间隔TimeSpan。</returns>
        public static TimeSpan DateDiff2(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts;
        }
        #endregion

        #region 格式化日期时间
        /// <summary>
        /// 格式化日期时间
        /// </summary>
        /// <param name="dateTime1">日期时间</param>
        /// <param name="dateMode">显示模式</param>
        /// <returns>0-9种模式的日期</returns>
        public static string FormatDate(DateTime dateTime1, string dateMode)
        {
            switch (dateMode)
            {
                case "0":
                    return dateTime1.ToString("yyyy-MM-dd");
                case "1":
                    return dateTime1.ToString("yyyy-MM-dd HH:mm:ss");
                case "2":
                    return dateTime1.ToString("yyyy/MM/dd");
                case "3":
                    return dateTime1.ToString("yyyy年MM月dd日");
                case "4":
                    return dateTime1.ToString("MM-dd");
                case "5":
                    return dateTime1.ToString("MM/dd");
                case "6":
                    return dateTime1.ToString("MM月dd日");
                case "7":
                    return dateTime1.ToString("yyyy-MM");
                case "8":
                    return dateTime1.ToString("yyyy/MM");
                case "9":
                    return dateTime1.ToString("yyyy年MM月");
                default:
                    return dateTime1.ToString();
            }
        }
        #endregion

        #region 得到随机日期
        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="time1">起始日期</param>
        /// <param name="time2">结束日期</param>
        /// <returns>间隔日期之间的 随机日期</returns>
        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            System.Random random = new System.Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            System.TimeSpan ts = new System.TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }
        #endregion

        #region 回每个月所有周的开始时间及周数
        /// <summary>
        /// 回每个月所有周的开始时间及周数
        /// </summary>
        /// <param name="cuMonth"></param>
        /// <returns></returns>
        public static List<object> GetWeekOfMonth(DateTime cuMonth)
        {
            cuMonth = DateTime.ParseExact(cuMonth.ToString("yyyy-MM-01"), "yyyy-MM-dd", null);
            DateTime nextMonth = cuMonth.AddMonths(1);
            List<object> weeks = new List<object>();
            int startDayOfWeek = (int)cuMonth.DayOfWeek;
            if (startDayOfWeek == 0) startDayOfWeek = 7;
            cuMonth = cuMonth.AddDays(-startDayOfWeek + 1);

            int i = 1;
            while (DateTime.Compare(cuMonth, nextMonth) < 0 && DateTime.Compare(cuMonth, DateTime.Now) < 0)
            {
                string week = string.Format("第{0}周", i);
                if (!weeks.Contains(week))
                {
                    weeks.Add(new { value = cuMonth.ToString("yyyy-MM-dd"), text = week });
                    i++;
                }
                cuMonth = cuMonth.AddDays(7);
            }
            return weeks;
        }
        #endregion

        #region 获取一年中的第几周
        /// <summary>
        /// 获取一年中的第几周
        /// </summary>
        /// <param name="day">日期</param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime day)
        {
            int weeknum;
            DateTime fDt = DateTime.Parse(day.Year.ToString() + "-01-01");
            int k = Convert.ToInt32(fDt.DayOfWeek);//得到该年的第一天是周几 
            if (k == 0)
            {
                k = 7;
            }
            int l = Convert.ToInt32(day.DayOfYear);//得到当天是该年的第几天 
            l = l - (7 - k + 1);
            if (l <= 0)
            {
                weeknum = 1;
            }
            else
            {
                if (l % 7 == 0)
                {
                    weeknum = l / 7 + 1;
                }
                else
                {
                    weeknum = l / 7 + 2;//不能整除的时候要加上前面的一周和后面的一周 
                }
            }
            return weeknum;
        }
        #endregion

        #endregion
    }
}