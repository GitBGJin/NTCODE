using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAnalyze.Air;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Forecast
{
    /// <summary>
    /// 名称：ForecastAdd.aspx.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-08-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：空气质量预报新增
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class ForecastAdd : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        RPT_AQIForecastService m_AQIForecast = Singleton<RPT_AQIForecastService>.GetInstance();

        override protected void OnPreInit(EventArgs e)
        {
            Page.Theme = "";
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 初始化
                ViewState["RTBCommandname"] = "";
                ViewState["LoginID"] = Session["LoginID"];
                //ViewState["UserGuid"] = Session["UserGuid"].ToString();
                ViewState["DisplayName"] = Session["Displayname"];
                RadDT.SelectedDate = DateTime.Now;
                #endregion

                RadTxtAQITimeA.Text = DateTime.Now.Day.ToString() + "日夜间";
                RadTxtAQITimeB.Text = (DateTime.Now.AddDays(1).Day).ToString() + "日上午";
                RadTxtAQITimeC.Text = (DateTime.Now.AddDays(1).Day).ToString() + "日下午";
                if (ConfigurationManager.AppSettings["AQIForecastDefaultUnit"] != null && ConfigurationManager.AppSettings["AQIForecastDefaultUnit"].ToString() != "")
                    RadTxtIssuedUnit.Text = ConfigurationManager.AppSettings["AQIForecastDefaultUnit"].ToString();

                BindCbx(RadCbxAQIClassA, 0);
                BindCbx(RadCbxAQIClassB, 0);
                BindCbx(RadCbxAQIClassC, 0);

                BindCbx(RadCbxPrimaryPollutantA, 1);
                BindCbx(RadCbxPrimaryPollutantB, 1);
                BindCbx(RadCbxPrimaryPollutantC, 1);

                ChkIsIssused.Checked = true;
            }
        }

        private void BindCbx(RadComboBox RadCbx, int Type)
        {
            switch (Type)
            {
                case 0:
                    RadCbx.Items.Clear();
                    RadCbx.Items.Add(new RadComboBoxItem("优", "优"));
                    RadCbx.Items.Add(new RadComboBoxItem("良", "良"));
                    RadCbx.Items.Add(new RadComboBoxItem("轻度污染", "轻度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("中度污染", "中度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("重度污染", "重度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("严重污染", "严重污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("优~良", "优~良"));
                    RadCbx.Items.Add(new RadComboBoxItem("良~轻度污染", "良~轻度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("轻度污染~中度污染", "轻度污染~中度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("中度污染~重度污染", "中度污染~重度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("重度污染~严重污染", "重度污染~严重污染"));
                    RadCbx.DataBind();
                    RadCbx.SelectedIndex = 1;
                    break;
                default:
                    RadCbx.Items.Clear();
                    RadCbx.Items.Add(new RadComboBoxItem("--", "--"));
                    RadCbx.Items.Add(new RadComboBoxItem("SO2", "SO2"));
                    RadCbx.Items.Add(new RadComboBoxItem("NO2", "NO2"));
                    RadCbx.Items.Add(new RadComboBoxItem("PM10", "PM10"));
                    RadCbx.Items.Add(new RadComboBoxItem("CO", "CO"));
                    RadCbx.Items.Add(new RadComboBoxItem("O3", "O3"));
                    RadCbx.Items.Add(new RadComboBoxItem("PM2.5", "PM2.5"));
                    RadCbx.DataBind();
                    RadCbx.SelectedIndex = 6;
                    break;
            }
        }

        protected void RadBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                RPT_AQIForecastEntity entity = new RPT_AQIForecastEntity();

                entity.AQITimeA = RadTxtAQITimeA.Text;
                entity.AQIClassA = RadCbxAQIClassA.SelectedValue;
                entity.PrimaryPollutantA = RadCbxPrimaryPollutantA.SelectedValue;
                entity.AQIA = RadMTxtAQIA.Text;

                entity.AQITimeB = RadTxtAQITimeB.Text;
                entity.AQIClassB = RadCbxAQIClassB.SelectedValue;
                entity.PrimaryPollutantB = RadCbxPrimaryPollutantB.SelectedValue;
                entity.AQIB = RadMTxtAQIB.Text;

                entity.AQITimeC = RadTxtAQITimeC.Text;
                entity.AQIClassC = RadCbxAQIClassC.SelectedValue;
                entity.PrimaryPollutantC = RadCbxPrimaryPollutantC.SelectedValue;
                entity.AQIC = RadMTxtAQIC.Text;

                entity.Description = RadTxtDescription.Text;
                entity.IssuedUnit = RadTxtIssuedUnit.Text;
                entity.IssuedTime = Convert.ToDateTime(RadDT.SelectedDate.Value);
                entity.IsIssued = Convert.ToBoolean(Convert.ToInt32(ChkIsIssused.Checked));

                m_AQIForecast.Add(entity);
                Alert("保存成功！");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);

            }
            catch (Exception ex)
            {
                Alert("保存失败！");
            }
        }
    }
}