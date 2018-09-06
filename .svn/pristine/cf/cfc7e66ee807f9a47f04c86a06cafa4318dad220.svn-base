using log4net;
using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Chart
{
    /// <summary>
    /// 名称：LadarBMPThermodynamicChart.aspx
    /// 创建人：刘晋
    /// 创建日期：2017-06-12
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 激光雷达热力图
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>


    public partial class LadarBMPThermodynamicChart : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        //获取一个日志记录器
        ILog log = LogManager.GetLogger("FileLogging");
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }
        public void bind()
        {
            //数据类型
            string quality = PageHelper.GetQueryString("quality");
            string firstValue = "Time,Height,Number \n";
            //string value = string.Empty;
            //传入的时间
            DateTime dtStart = DateTime.TryParse(PageHelper.GetQueryString("dtStart"), out dtStart) ? dtStart : DateTime.Now.AddDays(-1);
            DateTime dtEnd = DateTime.TryParse(PageHelper.GetQueryString("dtEnd"), out dtEnd) ? dtEnd : DateTime.Now;
            DataTable dt = new DataTable();
            string sql = string.Format(@"select DateTime, Height,Number from [dbo].[TB_SuperStation_jiguangleida]
                                                where DateTime<='{2}' and DateTime>='{1}' and DataType='{0}' and Number !='0'"
                , quality, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            hdDtStart.Value=dtStart.ToString("yyyy-MM-dd HH:mm:ss");
            hdDtEnd.Value=dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
            dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                firstValue += dt.Rows[i][0].ToString() + "," + dt.Rows[i][1].ToString() + "," + dt.Rows[i][2].ToString() + " \n";
            }
            //string[] secondValue = firstValue.TrimEnd(';').Split(';');
            //foreach(string key in secondValue)
            //{
            //    value += key;
            //}
            hdjsonData.Value = firstValue;

        }
    }
}