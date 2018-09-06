using SmartEP.Core.Generic;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class FrequencySetting : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        FrequencyService g_FrequencyService = Singleton<FrequencyService>.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["type"] = PageHelper.GetQueryString("type");
                if (this.ViewState["type"].ToString() == "water")
                {
                    this.ViewState["ApplictionUid"] = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Water);
                }
                else
                {
                    this.ViewState["ApplictionUid"] = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Air);
                }
                this.ViewState["factorcode"] = PageHelper.GetQueryString("factorcode");
                this.ViewState["factorname"] = PageHelper.GetQueryString("factorname");
                this.ViewState["unit"] = PageHelper.GetQueryString("unit");
                BindGridView();
                unit.Text = this.ViewState["unit"].ToString();
            }

        }
        protected void Page_Prerender(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["dv"];
            for (int i = 0; i < gvAtt.Rows.Count; i++)
            {
                if (i < dt.Rows.Count)
                {
                    (gvAtt.Rows[i].FindControl("Upper") as TextBox).Text = dt.Rows[i]["Upper"].ToString();
                    (gvAtt.Rows[i].FindControl("Lower") as TextBox).Text = dt.Rows[i]["Lower"].ToString();
                }
            }
        }
        /// <summary>
        /// GridView初始化
        /// </summary>
        private void BindGridView()
        {
            gvAtt.DataSource = GetInitDataTable();
            gvAtt.DataBind();
        }
        /// <summary>
        /// 初始化附表
        /// </summary>
        /// <returns></returns>
        private DataTable GetInitDataTable()
        {
            DataView dtFrequency = g_FrequencyService.GetSetData(this.ViewState["ApplictionUid"].ToString(), this.ViewState["factorcode"].ToString());
            if (dtFrequency.Count > 0)
            {
                ViewState["dv"] = dtFrequency.ToTable();
                return dtFrequency.ToTable();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Num", typeof(int));
                dt.Columns.Add("Upper", typeof(string));
                dt.Columns.Add("Lower", typeof(string));
                dt.Columns.Add("Range", typeof(string));
                dt.Columns.Add("PollutantCode", typeof(string));
                dt.Columns.Add("PollutantName", typeof(string));
                for (int i = 1; i <= 3; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Num"] = i;
                    dr["PollutantCode"] = this.ViewState["factorcode"].ToString();
                    dr["PollutantName"] = this.ViewState["factorname"].ToString();
                    dt.Rows.Add(dr);
                }
                ViewState["dv"] = dt;
                return dt;
            }
        }
        protected void gvAtt_DataBound(object sender, EventArgs e)
        {
            ViewState["gv"] = gvAtt.DataSource;
        }
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow row in gvAtt.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Attributes["ondblclick"] = ClientScript.GetPostBackEventReference(gvAtt, "Edit$" + row.RowIndex.ToString(), true);
                    row.Attributes["style"] = "cursor:pionter";
                    row.Attributes["title"] = "双击进入编辑";
                    if (row.RowIndex == gvAtt.EditIndex)
                    {
                        row.Attributes.Remove("ondblclick");
                        row.Attributes.Remove("style");
                        row.Attributes["title"] = "编辑行";
                        row.Attributes["ondblclick"] = ClientScript.GetPostBackEventReference(gvAtt, "Update$" + row.RowIndex.ToString(), true);
                    }
                }
            }
            base.Render(writer);
        }
        protected void gvAtt_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAtt.EditIndex = e.NewEditIndex;
        }

        protected void gvAtt_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            gvAtt.EditIndex = -1;
        }

        protected void AddRow_Click(object sender, EventArgs e)
        {
            DataTable dt = ((DataTable)ViewState["gv"]).Clone();
            for (int i = 0; i < gvAtt.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                string OederNum = (i + 1).ToString();
                string Upper = (gvAtt.Rows[i].FindControl("Upper") as TextBox).Text;
                string Lower = (gvAtt.Rows[i].FindControl("Lower") as TextBox).Text;
                dr["num"] = OederNum;
                if (Upper != "")
                {
                    dr["Upper"] = Upper;
                }
                if (Lower != "")
                {
                    dr["Lower"] = Lower;
                }
                dr["PollutantCode"] = this.ViewState["factorcode"].ToString();
                dr["PollutantName"] = this.ViewState["factorname"].ToString();
                dt.Rows.Add(dr);
            }
            DataRow dr1 = dt.NewRow();
            dr1["Num"] = dt.Rows.Count + 1;
            dr1["PollutantCode"] = this.ViewState["factorcode"].ToString();
            dr1["PollutantName"] = this.ViewState["factorname"].ToString();
            dt.Rows.Add(dr1);
            ViewState["dv"] = dt;
            gvAtt.DataSource = dt;
            gvAtt.DataBind();
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ApplicationUid", typeof(string));
            dt.Columns.Add("OederNum", typeof(string));
            dt.Columns.Add("Upper", typeof(string));
            dt.Columns.Add("Lower", typeof(string));
            dt.Columns.Add("Range", typeof(string));
            dt.Columns.Add("PollutantCode", typeof(string));
            dt.Columns.Add("PollutantName", typeof(string));
            for (int i = 0; i < gvAtt.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                string OederNum = (i + 1).ToString();
                string Upper = (gvAtt.Rows[i].FindControl("Upper") as TextBox).Text;
                string Lower = (gvAtt.Rows[i].FindControl("Lower") as TextBox).Text;
                string Range = "";
                if (Upper != "" && Lower != "")
                {
                    Range = Lower + "~" + Upper;
                }
                if (Upper == "" && Lower != "")
                {
                    Range = "≥" + Lower;
                }
                if (Upper != "" && Lower == "")
                {
                    Range = "＜" + Upper;
                }
                dr["ApplicationUid"] = this.ViewState["ApplictionUid"].ToString();
                dr["OederNum"] = OederNum;
                dr["Upper"] = Upper;
                dr["Lower"] = Lower;
                dr["Range"] = Range;
                dr["PollutantCode"] = this.ViewState["factorcode"].ToString();
                dr["PollutantName"] = this.ViewState["factorname"].ToString();
                if (Upper != "" || Lower != "")
                {
                    dt.Rows.Add(dr);
                }
            }
            ViewState["dv"] = dt;
            g_FrequencyService.insertTable(dt, this.ViewState["ApplictionUid"].ToString(), this.ViewState["factorcode"].ToString());
            Alert("保存成功！");
        }
    }
}