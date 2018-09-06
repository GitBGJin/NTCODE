using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Utilities.AdoData;

namespace SmartEP.WebUI.Pages.EnvAir.RemoteControl
{
    public partial class RemoteControlLog : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void BindGrid()
        {
            DataView myView = null;
            myView = new DatabaseHelper().ExecuteDataView(creatSql(), "AMS_AirAutoMonitorConnection");
            RadGrid1.DataSource = myView;
        }

        protected string creatSql()
        {
            string strTimeWhere = "", strStartTime = "", strEndTime = "", strOrderBy = "";
            string strSelect = "SELECT * FROM TB_OriginalPacketRequest where 1=1 ";
            strStartTime = startTime.SelectedDate.ToString();
            strEndTime = endTime.SelectedDate.ToString();
            if (!strStartTime.Equals(""))
            {
                strTimeWhere += " and operaterTime>='" + strStartTime + "'";
            }
            if (!strEndTime.Equals(""))
            {
                strTimeWhere += " and operaterTime<='" + strEndTime + "'";
            }
            strOrderBy = " order by operaterTime desc";
            return strSelect + strTimeWhere + strOrderBy;

        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void RadGrid1_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Delete":
                    for (int i = 0; i < RadGrid1.MasterTableView.Items.Count; i++)
                    {
                        if (this.RadGrid1.MasterTableView.Items[i].Selected == true)
                        {
                            try
                            {
                                int id = Convert.ToInt32(RadGrid1.MasterTableView.DataKeyValues[i]["id"].ToString());
                                Delete(id);
                            }
                            catch
                            {
                            }
                        }
                    }
                    break;

                case "ExportToExcel":
                    RadGrid1.ExportSettings.OpenInNewWindow = true;
                    RadGrid1.ExportSettings.IgnorePaging = true;
                    RadGrid1.ExportSettings.ExportOnlyData = true;
                    RadGrid1.MasterTableView.Columns[0].Visible = false;
                    RadGrid1.MasterTableView.ExportToExcel();
                    break;

                case "ExportToWord":
                    RadGrid1.ExportSettings.OpenInNewWindow = true;
                    RadGrid1.ExportSettings.IgnorePaging = true;
                    RadGrid1.ExportSettings.ExportOnlyData = true;
                    RadGrid1.MasterTableView.Columns[0].Visible = false;
                    RadGrid1.MasterTableView.ExportToWord();
                    break;

                case "ExportToCsv":
                    RadGrid1.ExportSettings.OpenInNewWindow = true;
                    RadGrid1.ExportSettings.IgnorePaging = true;
                    RadGrid1.ExportSettings.ExportOnlyData = true;
                    RadGrid1.MasterTableView.Columns[0].Visible = false;
                    RadGrid1.MasterTableView.ExportToCSV();
                    break;
            }
            this.RadGrid1.Rebind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
            RadGrid1.Rebind();
        }

        protected void Delete(int id)
        {
            string sql = "DELETE FROM TB_OriginalPacketRequest WHERE id=" + id;
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AMS_AirAutoMonitorConnection"].ConnectionString);
            SqlCommand myCommand = new SqlCommand(sql, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("添加记录时产生错误，错误代码为：" + e.ToString());
            }
            finally
            {
                myCommand.Dispose();
                myConn.Close();
                myConn.Dispose();
            }
        }
    }
}