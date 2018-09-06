using log4net;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
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

    public partial class _Default : System.Web.UI.Page
    {
        ILog log = LogManager.GetLogger("App.Logging");//获取一个日志记录器
        DAL d_DAL = new DAL();
        DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(this.exporting));
            if (!IsPostBack)
            {
                log.Info("绑定数据源");
                GetData();
            }
            if (Request.Form["svg532"] != null)
            {
                log.Info("保存532");
                string tSvg = Request.Form["svg532"].ToString();
                saveImage("532", tSvg);
            }
            if (Request.Form["svg355"] != null)
            {
                log.Info("保存355");
                string tSvg = Request.Form["svg355"].ToString();
                saveImage("355", tSvg);
            }
            if (Request.Form["svgtuipian"] != null)
            {
                log.Info("保存tuipian");
                string tSvg = Request.Form["svgtuipian"].ToString();
                saveImage("tuipian", tSvg);
            }
            if (Request.Form["svghight"] != null)
            {
                log.Info("保存height");
                string tSvg = Request.Form["svghight"].ToString();
                saveImage("height", tSvg);
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
                string sqlch = string.Empty;
                DataTable dvch = null;
                string[] stch = null;

                string sqlche = string.Empty;
                DataTable dvche = null;
                string[] stche = null;

                string sqlchd = string.Empty;
                DataTable dvchd = null;
                string[] stchd = null;
                //log.Info("时间段："+sTime + "-----" + eTime);

                sqlch = string.Format("select Hight,Min,Max,HightMin from [dbo].[DT_JGLDColorHight] where DataType='{0}'", "extin532");
                dvch = g_DatabaseHelper.ExecuteDataTable(sqlch, "AMS_BaseDataConnection");
                stch = dtToArr(dvch);

                sqlche = string.Format("select Hight,Min,Max,HightMin from [dbo].[DT_JGLDColorHight] where DataType='{0}'", "extin355");
                dvche = g_DatabaseHelper.ExecuteDataTable(sqlche, "AMS_BaseDataConnection");
                stche = dtToArr(dvche);

                sqlchd = string.Format("select Hight,Min,Max,HightMin from [dbo].[DT_JGLDColorHight] where DataType='{0}'", "depol");
                dvchd = g_DatabaseHelper.ExecuteDataTable(sqlchd, "AMS_BaseDataConnection");
                stchd = dtToArr(dvchd);

                hdMin.Value = stch[1];
                hdMax.Value = stch[2];

                hdMine.Value = stche[1];
                hdMaxe.Value = stche[2];

                hdMind.Value = stchd[1];
                hdMaxd.Value = stchd[2];

                DataTable dt = d_DAL.GetLadarData("extin532", sTime, eTime, stch[0],stch[3]);
                DataTable dtex355 = d_DAL.GetLadarData("extin355", sTime, eTime, stche[0],stche[3]);
                DataTable dtdepol = d_DAL.GetLadarData("depol", sTime, eTime, stchd[0],stchd[3]);
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

                DataTable dts = new DataTable();
                DataTable dte = new DataTable();
                DataTable dtd = new DataTable();

                dts.Columns.Add("xValue", typeof(string));
                dts.Columns.Add("yValue", typeof(string));
                dts.Columns.Add("zValue", typeof(string));

                dte.Columns.Add("xValue", typeof(string));
                dte.Columns.Add("yValue", typeof(string));
                dte.Columns.Add("zValue", typeof(string));

                dtd.Columns.Add("xValue", typeof(string));
                dtd.Columns.Add("yValue", typeof(string));
                dtd.Columns.Add("zValue", typeof(string));

                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drNew = dts.NewRow();
                    drNew["xValue"] = Convert.ToDateTime(dr["DateTime"].ToString()).ToString("MM/dd HH:mm:ss");
                    drNew["yValue"] = dr["Height"].ToString().Substring(0, dr["Height"].ToString().Length - 2);
                    drNew["zValue"] = dr["Number"];
                    dts.Rows.Add(drNew);
                }

                foreach (DataRow dr in dtex355.Rows)
                {
                    DataRow drNewe = dte.NewRow();
                    drNewe["xValue"] = Convert.ToDateTime(dr["DateTime"].ToString()).ToString("MM/dd HH:mm:ss");
                    drNewe["yValue"] = dr["Height"].ToString().Substring(0, dr["Height"].ToString().Length - 2);
                    drNewe["zValue"] = dr["Number"];
                    dte.Rows.Add(drNewe);
                }

                foreach (DataRow dr in dtdepol.Rows)
                {
                    DataRow drNewd = dtd.NewRow();
                    drNewd["xValue"] = Convert.ToDateTime(dr["DateTime"].ToString()).ToString("MM/dd HH:mm:ss");
                    drNewd["yValue"] = dr["Height"].ToString().Substring(0, dr["Height"].ToString().Length - 2);
                    drNewd["zValue"] = dr["Number"];
                    dtd.Rows.Add(drNewd);
                }

                HiddenData.Value = JsonHelper.ToJson(dts);
                log.Info("数据量：" + dts.Rows.Count);
                HiddenDatae.Value = JsonHelper.ToJson(dte);
                HiddenDatad.Value = JsonHelper.ToJson(dtd);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 保存图像
        /// </summary>
        public void saveImage(string fileName,string svg)
        {
            try
            {
                string tFileName = "chart" + DateTime.Now.ToString("yyyyMMddHH") + "-" + fileName;
                string savePath = Server.MapPath("image/imageOne/");

                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                savePath += tFileName + ".svg";
                if (!File.Exists(savePath))
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(savePath, true, Encoding.UTF8);
                    sw.WriteLine(svg);
                    sw.Close();
                }
                else
                {
                    File.Delete(savePath);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(savePath, true, Encoding.UTF8);
                    sw.WriteLine(svg);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        /// <summary>
        /// DataTable转换为一维字符串数组
        /// </summary>
        /// <returns></returns>
        public static string[] dtToArr(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new string[0];
            }
            else
            {
                string[] sr = new string[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (Convert.IsDBNull(dt.Rows[0][i]))
                    {
                        sr[i] = "";
                    }
                    else
                    {
                        sr[i] = dt.Rows[0][i] + "";
                    }
                }
                return sr;
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
                string strValue = Convert.ToDateTime(drc[i][0].ToString()).ToString("MM/dd HH:mm:ss");

                jsonString.Append("\"" + strValue + "\",");

            }

            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

    }
}
