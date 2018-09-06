using Aspose.Words;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class LargeMonthReport : SmartEP.WebUI.Common.BasePage
    {        /// <summary>
        /// 数据处理服务
        /// </summary>
        ReportLogService ReportLogService = new ReportLogService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (SessionHelper.Get("DisplayName") != null)
                {
                    string strPerson = SessionHelper.Get("DisplayName").ToString();
                    hdPerson.Value = strPerson;
                }
                else
                {
                    hdPerson.Value = "";
                }
                InitControl();
                BindGrid();
            }
        }

        private void InitControl()
        {
            dayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-01"));
            dayEnd.SelectedDate = DateTime.Now;
            this.ViewState["pageTypeID"] = PageHelper.GetQueryString("pageTypeID") != "" ? PageHelper.GetQueryString("pageTypeID") : "AirMonthReport";
            hdPageId.Value = this.ViewState["pageTypeID"].ToString();
            ViewState["waterOrAirType"] = 1;
        }
        public void BindGrid()
        {
            DateTime dtBegin = Convert.ToDateTime(Convert.ToDateTime(dayBegin.SelectedDate).ToString("yyyy-MM-dd 00:00:00"));
            DateTime dtEnd = Convert.ToDateTime(Convert.ToDateTime(dayEnd.SelectedDate.Value.AddMonths(+1).AddDays(-1)).ToString("yyyy-MM-dd 23:59:59"));
            IQueryable<ReportLogEntity> customDatumData = ReportLogService.CustomDatumRetrieve(this.ViewState["pageTypeID"].ToString(), Convert.ToInt32(this.ViewState["waterOrAirType"])).Where(it => it.StartDateTime >= dtBegin && it.EndDateTime <= dtEnd);
            DataTable dt = ConvertToDataTable(customDatumData);
            DataView dv = new DataView(dt);
            dv.Sort = "CreatDateTime DESC";
            gridList.DataSource = dv;
            gridList.VirtualItemCount = dv.Count;
            if (this.ViewState["pageTypeID"].ToString() == "RoutineMonthReport")
            {
                gridList.Columns[6].Visible = false;
            }
        }

        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(ReportLogEntity)))
            {

                dataTable.Columns.Add(pd.Name);

            }
            foreach (ReportLogEntity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(ReportLogEntity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }

        protected void gridList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridList.Rebind();
        }

        protected void gridList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "download")
            {
                string pageid = this.ViewState["pageTypeID"].ToString();
                string path = e.CommandArgument.ToString();
                try
                {
                    Document doc = new Document(Server.MapPath(path));
                    var docStream = new MemoryStream();
                    doc.Save(this.Response, "AirMonthReport.doc", Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
                }
                catch (Exception ex)
                {
                    //Alert("数据异常！");
                }
            }
            else if (e.CommandName == "modify")
            {
                int path = e.CommandArgument.ToString().Split('/').Length;
                string name = e.CommandArgument.ToString().Split('/')[path - 1];
                RadScriptManager.RegisterStartupScript(this, GetType(), "ShowWebOffice", "<script>ShowWebOffice(escape('" + name + "'));</script>", false);
            }
        }
        ///   <summary>   
        ///   文件下载   
        ///   </summary>   
        ///   <param   name="FullFileName"></param>   
        private void FileDownload(string FullFileName, string FileName)
        {
            FileInfo DownloadFile = new FileInfo(FullFileName);
            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = false;
            Response.ContentType = "application/ms-access";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
            Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
            Response.WriteFile(DownloadFile.FullName);
            Response.Flush();
            Response.End();
        }
        protected void gridList_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                int id = Convert.ToInt32(dataItem.GetDataKeyValue("id").ToString());
                string path = Server.MapPath(ReportLogService.ReportLogRetrieveByid(id).FirstOrDefault().ReportName);
                ReportLogService.Delete(id);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                gridList.Controls.Add(new LiteralControl("Unable to delete . Reason: " + ex.Message));
                e.Canceled = true;
            }
        }
    }
}