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
    public partial class Defaultd : System.Web.UI.Page
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

                //log.Info("时间段："+sTime + "-----" + eTime);

                DataTable dtdepol = d_DAL.GetLadarData("depol", sTime, eTime);

                DataTable dtd = new DataTable();

                
                dtd.Columns.Add("xValue", typeof(string));
                dtd.Columns.Add("yValue", typeof(string));
                dtd.Columns.Add("zValue", typeof(string));

                
                foreach (DataRow dr in dtdepol.Rows)
                {
                    DataRow drNewd = dtd.NewRow();
                    drNewd["xValue"] = Convert.ToDateTime(dr["DateTime"].ToString()).ToString("MM/dd HH:mm:ss");
                    drNewd["yValue"] = dr["Height"].ToString().Substring(0, dr["Height"].ToString().Length - 2);
                    drNewd["zValue"] = dr["Number"];
                    dtd.Rows.Add(drNewd);
                }

                HiddenDatad.Value = JsonHelper.ToJson(dtd);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
    }
}