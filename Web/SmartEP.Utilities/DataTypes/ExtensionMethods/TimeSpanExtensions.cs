#region Usings
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
#endregion

namespace SmartEP.Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// TimeSpan extension methods
    /// </summary>
    public static class TimeSpanExtensions
    {
        #region Extension Methods

        #region DaysRemainder

        /// <summary>
        /// Days in the TimeSpan minus the months and years
        /// </summary>
        /// <param name="Span">TimeSpan to get the days from</param>
        /// <returns>The number of days minus the months and years that the TimeSpan has</returns>
        public static int DaysRemainder(this TimeSpan Span)
        {
            return (DateTime.MinValue + Span).Day - 1;
        }

        #endregion

        #region Months

        /// <summary>
        /// Months in the TimeSpan
        /// </summary>
        /// <param name="Span">TimeSpan to get the months from</param>
        /// <returns>The number of months that the TimeSpan has</returns>
        public static int Months(this TimeSpan Span)
        {
            return (DateTime.MinValue + Span).Month - 1;
        }

        #endregion

        #region Years

        /// <summary>
        /// Years in the TimeSpan
        /// </summary>
        /// <param name="Span">TimeSpan to get the years from</param>
        /// <returns>The number of years that the TimeSpan has</returns>
        public static int Years(this TimeSpan Span)
        {
            return (DateTime.MinValue + Span).Year - 1;
        }

        #endregion

        #endregion
    }
} 
