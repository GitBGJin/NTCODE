using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Service.BaseData.BusinessRule;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.BaseData
{
    public partial class UserPersonalizedConfig : SmartEP.WebUI.Common.BasePage
    {
        private const string POLLUTANT_TYPE_CHANNEL = "8D89B62D-36E1-4F05-B00D-3A585F6A90D7";

        private const string PARAMTYPE_POINT = "port";

        private const string PARAMTYPE_POLLUTANT = "pollutant";

        private string APP_UID = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Air);

        private PersonalizedSetService personalSetService = new PersonalizedSetService();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRSM(RSMPoint);
            }
        }

        /// <summary>
        /// 绑定SiteMap
        /// </summary>
        /// <param name="RSM">SiteMap对象</param>
        private void BindRSM(RadSiteMap RSM)
        {
            String DefaultName = "";
            DataView personalizedDv = null;

            string userGuid = Session["UserGuid"].ToString();

            switch (RSM.ID)
            {
                case "RSMPoint":
                    personalizedDv = personalSetService.GetPersonalizedSetPointGroupByUserGuid(userGuid, APP_UID);
                    break;
                case "RSMFactor":
                    personalizedDv = personalSetService.GetPersonalizedSetPollutantGroupByUserGuid(userGuid, POLLUTANT_TYPE_CHANNEL);
                    break;
                default:
                    break;
            }

            if (personalizedDv != null && personalizedDv.Count > 0)
            {
                #region 绑定数据
                RSM.DataSource = personalizedDv;
                RSM.DataFieldParentID = "PGuid";
                RSM.DataFieldID = "CGuid";
                RSM.DataTextField = "PName";
                RSM.DataValueField = "GID";
                RSM.DataBind();
                #endregion
                
                #region 设置默认值
                personalizedDv.RowFilter = " EnableCustomOrNot=1";
                int i = 0, j = 0;
                foreach (RadSiteMapNode L1 in RSM.Nodes)
                {
                    i = 0;
                    j = 0;
                    foreach (RadSiteMapNode L2 in L1.Nodes)
                    {
                        foreach (Control C in L2.Controls)
                        {
                            if (C.GetType().Name == "CheckBox" && DefaultName.Split(';').Length > 0)
                            {
                                foreach (DataRowView DRV in personalizedDv)
                                {
                                    if (((CheckBox)C).Text == DRV["PName"])
                                    {
                                        ((CheckBox)C).Checked = true;
                                        j++;
                                    }
                                }
                            }
                            if (C.GetType().Name == "CheckBox") i++;
                        }
                    }
                    if (i == j && j != 0) ((CheckBox)L1.Controls[1]).Checked = true;
                }
                #endregion
            }

        }

        protected void SaveData(RadSiteMap RSM, String ParameterType)
        {
            #region 获取值
            try
            {
                CheckBox cBox = null;
                string userGuid = Session["UserGuid"].ToString();
                string paramUid = null;
                string enable = null;

                foreach (RadSiteMapNode L1 in RSM.Nodes)
                {
                    foreach (RadSiteMapNode L2 in L1.Nodes)
                    {
                        foreach (Control C in L2.Controls)
                        {
                            if (C.GetType().Name == "CheckBox" && ((CheckBox)C).Text != "")
                            {
                                cBox = (CheckBox)C;
                                paramUid = cBox.ToolTip;
                                enable = (cBox.Checked) ? "1" : "0";

                                // 更新用户授权因子
                                personalSetService.UpdatePersonalizedSetByUserGuid(userGuid, paramUid, ParameterType, enable);
                            }
                        }
                    }
                }
                Alert("保存成功！");
            }
            catch (Exception Err)
            {
                Alert("保存失败，请重新登录后尝试！" + Err.Message);
            }
            #endregion
        }
        protected void RadBtnSave_Click(object sender, EventArgs e)
        {
            if (((RadButton)sender).ID == "RadBtnPointSave") SaveData(RSMPoint, PARAMTYPE_POINT);
            if (((RadButton)sender).ID == "RadBtnFactorSave") SaveData(RSMFactor, PARAMTYPE_POLLUTANT);
        }

        protected void RadBtnPointChange_Click(object sender, EventArgs e)
        {
            BindRSM(RSMPoint);
        }

        protected void RadTabStrip1_TabClick(object sender, RadTabStripEventArgs e)
        {
            if ((((RadTabStrip)sender)).SelectedTab.Text == "关注因子")
            {
                BindRSM(RSMFactor);
            }
            
            e.Tab.PageView.Selected = true;
        }
    }

}