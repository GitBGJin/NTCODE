using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace SmartEP.WebUI.Pages.EnvAir.BaseData
{
    public partial class UserAuthConfigDetail : SmartEP.WebUI.Common.BasePage
    {
        private const string PARAMTYPE_POINT = "port";

        private const string PARAMTYPE_POLLUTANT = "pollutant";

        private string APP_UID = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Air);

        private PersonalizedSetService personalSetService = new PersonalizedSetService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userUid = PageHelper.GetQueryString("UserUid");
                UserGuid.Value = userUid;

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

            string userGuid = UserGuid.Value;

            switch (RSM.ID)
            {
                case "RSMPoint":
                    personalizedDv = personalSetService.GetAuthPointGroupByUserGuid(userGuid, APP_UID);
                    break;
                case "RSMFactor":
                    personalizedDv = personalSetService.GetAuthPollutantGroupByUserGuid(userGuid, APP_UID);
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
                string userUid = UserGuid.Value;
                string paramUid = null;
                List<string> paramUids = new List<string>();

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
                                if (cBox.Checked)
                                {
                                    paramUids.Add(paramUid);
                                }
                            }
                        }
                    }
                }
                
                if (paramUids.Count == 0)
                {
                    // 删除用户原有的授权设置
                    personalSetService.DelPersonalizedSetByUserGuid(userUid, APP_UID, ParameterType);
                }
                else
                {
                    // 删除用户原有的授权设置
                    string notDelPointIds = string.Join("','", paramUids.ToArray());
                    personalSetService.DelPersonalizedSetByUserGuid(userUid, APP_UID, ParameterType, notDelPointIds);

                    AddPersonalizedSet(userUid, ParameterType, paramUids);
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
            if ((((RadTabStrip)sender)).SelectedTab.Text == "授权因子")
            {
                BindRSM(RSMFactor);
            }

            e.Tab.PageView.Selected = true;
        }

        private void AddPersonalizedSet(string userUid, string matchParamType, List<string> paramUids)
        {
            DataTable personalSetDt = personalSetService.GetPersonalizedSetByUserGuid(userUid, APP_UID);

            string paramType = null;
            string paramUid = null;
            // 过滤之前已经授权个性化的测点与因子，避免重复插入
            foreach (DataRow dr in personalSetDt.Rows)
            {
                paramUid = dr["ParameterUid"].ToString();
                paramType = dr["ParameterType"].ToString();
                if (matchParamType.Equals(paramType))
                {
                    paramUids.Remove(paramUid);
                }
            }

            // 添加用户授权设置
            personalSetService.AddPersonalizedSet(userUid, APP_UID, matchParamType, paramUids.ToArray());
        }
    }

}