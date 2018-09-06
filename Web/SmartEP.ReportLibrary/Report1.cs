namespace SmartEP.ReportLibrary
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for Report1.
    /// </summary>
    public partial class Report1 : Telerik.Reporting.Report
    {
        public static string pointId = "14";
        public static DateTime searchtime = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        public Report1()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        public static string FormatDateTime(DateTime dt)
        {
            return dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.ToShortTimeString();
        }

        public static DateTime currentDate()
        {
            return searchtime;
            //return Convert.ToDateTime(DateTime.Now.ToShortDateString());
        }

        public static string GetPortID()
        {
            return pointId;
        }

        public static string GetPortName()
        {
            return new Report1().ReportParameters["pointId"].AvailableValues.DisplayMember;
        }

        /// <summary>
        /// 判断零跨是否一级超标
        /// </summary>
        /// <param name="calTypeCode">校准类型</param>
        /// <param name="deviationRatio">漂移</param>
        /// <returns></returns>
        public static bool noticeYellow(string calTypeCode, decimal deviationRatio)
        {
            bool boolRetValue = false;
            switch (calTypeCode)
            {
                case "Z":
                    if ((deviationRatio < -2 && deviationRatio >= -5) || (deviationRatio > 2 && deviationRatio <= 5))
                        boolRetValue = true;
                    break;
                case "z":
                    if ((deviationRatio < -2 && deviationRatio >= -5) || (deviationRatio > 2 && deviationRatio <= 5))
                        boolRetValue = true;
                    break;
                default:
                    if ((deviationRatio < -5 && deviationRatio >= -10) || (deviationRatio > 5 && deviationRatio <= 10))
                        boolRetValue = true;
                    break;
            }
            return boolRetValue;
        }
        /// <summary>
        /// 判断零跨是否二级超标
        /// </summary>
        /// <param name="calTypeCode">校准类型</param>
        /// <param name="deviationRatio">漂移</param>
        /// <returns></returns>
        public static bool noticeRed(string calTypeCode, decimal deviationRatio)
        {
            bool boolRetValue = false;
            switch (calTypeCode)
            {
                case "Z":
                    if (deviationRatio < -5 || deviationRatio > 5)
                        boolRetValue = true;
                    break;
                case "z":
                    if (deviationRatio < -5 || deviationRatio > 5)
                        boolRetValue = true;
                    break;
                default:
                    if (deviationRatio < -10 || deviationRatio > 10)
                        boolRetValue = true;
                    break;
            }
            return boolRetValue;
        }

        /// <summary>
        /// 判断超标等级
        /// </summary>
        /// <param name="calTypeCode">校准类型</param>
        /// <param name="deviationRatio">漂移</param>
        /// <returns></returns>
        public static string ExcessiveState(string calTypeCode, decimal deviationRatio)
        {
            string strRetValue = "正常";
            switch (calTypeCode)
            {
                case "Z":
                    if ((deviationRatio < -2 && deviationRatio >= -5) || (deviationRatio > 2 && deviationRatio <= 5))
                        strRetValue = "超调节控制限";
                    if (deviationRatio < -5 || deviationRatio > 5)
                        strRetValue = "超漂移控制限";
                    break;
                case "z":
                    if ((deviationRatio < -2 && deviationRatio >= -5) || (deviationRatio > 2 && deviationRatio <= 5))
                        strRetValue = "超调节控制限";
                    if (deviationRatio < -5 || deviationRatio > 5)
                        strRetValue = "超漂移控制限";
                    break;
                default:
                    if ((deviationRatio < -5 && deviationRatio >= -10) || (deviationRatio > 5 && deviationRatio <= 10))
                        strRetValue = "超调节控制限";
                    if (deviationRatio < -10 || deviationRatio > 10)
                        strRetValue = "超漂移控制限";
                    break;
            }
            return strRetValue;
        }
    }
}