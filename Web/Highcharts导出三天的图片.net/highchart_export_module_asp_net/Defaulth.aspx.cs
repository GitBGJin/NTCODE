using log4net;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace highchart_export_module_asp_net
{
    public partial class Defaulth : System.Web.UI.Page
    {
        ILog log = LogManager.GetLogger("App.Logging");//获取一个日志记录器
        DAL d_DAL = new DAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(this.exporting));
            if (!IsPostBack)
            {
                GetData();

            }
        }
        private void GetData()
        {
            try
            {
                //string sTime = "2017-07-01";
                //string eTime = "2017-07-02";
                string sTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00");
                string eTime = DateTime.Now.ToString("yyyy-MM-dd HH:00");

                DataView dtH = d_DAL.GetHeightData("height", sTime, eTime);

                if (dtH.ToTable().Rows.Count > 0)
                {
                    DataTable dtTime = dtH.ToTable();
                    dtTime.Columns.Remove("Number");
                    hdCount.Value = dtTime.Rows.Count.ToString();
                    DataTable dtBorder = dtH.ToTable();
                    dtBorder.Columns.Remove("DateTime");
                    hdTime.Value = ToStringNew(dtTime);
                    hdBorder.Value = ToString(dtBorder);
                }
                else
                {
                    hdCount.Value = "1";
                    hdTime.Value = "[]";
                    hdBorder.Value = "[]";
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        public static string ToString(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                string strValue = drc[i][0].ToString();

                jsonString.Append(strValue + ",");

            }

            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        public static string ToStringNew(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                string strValue = Convert.ToDateTime(drc[i][0].ToString()).ToString("MM-dd HH:mm:ss");

                jsonString.Append("\"" + strValue + "\",");

            }

            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
    }
}