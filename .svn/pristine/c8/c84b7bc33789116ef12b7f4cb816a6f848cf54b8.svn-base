using SmartEP.Utilities.AdoData;
using SmartEP.Core.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.Common.WebControl;
using SmartEP.Core.Generic;

namespace SmartEP.WebControl.CbxRsm
{
    public class CbxRsmControl
    {
        //DatabaseHelper myComm = new DatabaseHelper();
        //const String myConnName = "AMS_BaseDataConnection";
        /// <summary>
        /// 自定义控件DAL
        /// </summary>
        CbxRsmControlDAL g_CbxRsmControlDAL = Singleton<CbxRsmControlDAL>.GetInstance();

        /// <summary>
        /// 绑定超级站站点
        /// </summary>
        /// <param name="objCtrl">绑定对象: RadCombox、 RadSiteMap</param>
        /// <param name="applicationType">应用类型: 水、 气、声</param>
        /// <param name="CbxRsmType">SiteMap种类（0、测点；1、通道因子；2、用户；3、状态因子）</param>
        /// <param name="pointType">测点绑定类型：类型、区域</param>
        /// <param name="defaultStrList">默认选择的值(名称1;名称2;…)</param>
        /// <param name="notIn">不需要显示:目前只支持Guid(逗号分隔)</param>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="IsCheckAll">是否全选(可能选参数：默认为false)</param>
        /// <param name="isAllSel">是否所有参数</param>
        /// <returns></returns>
        public String BindRSM(object objCtrl, ApplicationType applicationType, CbxRsmType rsmType, RsmPointMode pointType, String defaultStrList, String notIn, String userGuid, string _defaultSuper,Boolean IsCheckAll = false, bool isAllSel = false)
        {
            #region 获取控件对象
            RadSiteMap RSM = null;
            RadComboBox RCB = null;

            if (objCtrl is RadComboBox)
            {
                RCB = ((RadComboBox)objCtrl);
                RSM = ((RadSiteMap)RCB.Items[0].FindControl("RSM"));
            }
            else
            { RSM = objCtrl as RadSiteMap; }
            #endregion

            #region 数据源
            DataView myDV = g_CbxRsmControlDAL.GetRsmData(applicationType, rsmType, pointType, defaultStrList, notIn, userGuid, IsCheckAll, isAllSel, _defaultSuper);
            #endregion

            if (true)
            {
                #region 绑定数据
                RSM.DataSource = myDV;
                RSM.DataFieldParentID = "PGuid";
                RSM.DataFieldID = "CGuid";
                RSM.DataTextField = "RsmName";
                RSM.DataValueField = "RsmValue";
                RSM.DataBind();
                #endregion

                if (IsCheckAll)
                {
                    #region 全部选中
                    if (RCB != null) { RCB.EmptyMessage = ""; RCB.Text = ""; }

                    foreach (RadSiteMapNode iNode in RSM.GetAllNodes())
                    {
                        foreach (Control C in iNode.Controls)
                        {
                            if (C is CheckBox)
                            {
                                ((CheckBox)C).Checked = true;
                                if (iNode.Value != "" && RCB != null)
                                {
                                    RCB.EmptyMessage += iNode.Text + ";";
                                    RCB.Text += iNode.Text + ";";
                                }
                            }
                        }
                    }
                    return "";
                    #endregion
                }
                else
                {
                    #region 根据默认值选中
                    if (defaultStrList != "")
                    {
                        #region 验证默认值
                        String CheckDefaultValue = "";
                        int findIndex = -1;
                        myDV.Sort = "RsmName";
                        foreach (String Value in defaultStrList.Split(';'))
                        {
                            findIndex = myDV.Find(Value);
                            if (findIndex >= 0) CheckDefaultValue += myDV[findIndex]["RsmName"] + ";";
                        }
                        myDV.Sort = "POrder,COrder";
                        if (defaultStrList == "" && CheckDefaultValue == "")
                        {
                            CheckDefaultValue = myDV[0]["RsmName"].ToString();
                        }
                        CheckDefaultValue = CheckDefaultValue.Trim(';');
                        #endregion

                        #region 设置默认值
                        if (RCB != null)
                        {
                            RCB.EmptyMessage = CheckDefaultValue;
                            RCB.Text = CheckDefaultValue;
                        }
                        int i = 0, j = 0;
                        foreach (RadSiteMapNode L1 in RSM.Nodes)
                        {
                            i = 0;
                            j = 0;
                            foreach (RadSiteMapNode L2 in L1.Nodes)
                            {
                                foreach (Control C in L2.Controls)
                                {
                                    if (C.GetType().Name == "CheckBox" && CheckDefaultValue.Split(';').Length > 0)
                                    {
                                        foreach (String item in CheckDefaultValue.Split(';'))
                                        {
                                            if (((CheckBox)C).Text == item)
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
                    else
                    {
                        #region 默认值
                        int i = 0;
                        foreach (RadSiteMapNode N in RSM.GetAllNodes())
                        {
                            foreach (Control C in N.Controls)
                            {
                                if (C is CheckBox && !String.IsNullOrEmpty(N.Value) && i == 0 && RCB != null)
                                {
                                    ((CheckBox)C).Checked = true;
                                    RCB.EmptyMessage = N.Text;
                                    RCB.Text = N.Text;
                                    i++;
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    return "";
                    #endregion
                }
            }
            else
            { if (RCB.ID == "RadCBoxPoint") return "没有测点权限，管理员未分配权限！"; else return ""; }
        }
        
        /// <summary>
        /// 绑定站点
        /// </summary>
        /// <param name="objCtrl">绑定对象: RadCombox、 RadSiteMap</param>
        /// <param name="applicationType">应用类型: 水、 气、声</param>
        /// <param name="CbxRsmType">SiteMap种类（0、测点；1、通道因子；2、用户；3、状态因子）</param>
        /// <param name="pointType">测点绑定类型：类型、区域</param>
        /// <param name="defaultStrList">默认选择的值(名称1;名称2;…)</param>
        /// <param name="notIn">不需要显示:目前只支持Guid(逗号分隔)</param>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="IsCheckAll">是否全选(可能选参数：默认为false)</param>
        /// <param name="isAllSel">是否所有参数</param>
        /// <returns></returns>
        public String BindRSM(object objCtrl, ApplicationType applicationType, CbxRsmType rsmType, RsmPointMode pointType, String defaultStrList, String notIn, String userGuid, Boolean IsCheckAll = false, bool isAllSel = false)
        {
            #region 获取控件对象
            RadSiteMap RSM = null;
            RadComboBox RCB = null;

            if (objCtrl is RadComboBox)
            {
                RCB = ((RadComboBox)objCtrl);
                RSM = ((RadSiteMap)RCB.Items[0].FindControl("RSM"));
            }
            else
            { RSM = objCtrl as RadSiteMap; }
            #endregion

            #region 数据源
            DataView myDV = g_CbxRsmControlDAL.GetRsmData(applicationType, rsmType, pointType, defaultStrList, notIn, userGuid, IsCheckAll, isAllSel);
            #endregion

            if (myDV != null && myDV.Count > 0)
            {
                #region 绑定数据
                RSM.DataSource = myDV;
                RSM.DataFieldParentID = "PGuid";
                RSM.DataFieldID = "CGuid";
                RSM.DataTextField = "RsmName";
                RSM.DataValueField = "RsmValue";
                RSM.DataBind();
                #endregion

                if (IsCheckAll)
                {
                    #region 全部选中
                    if (RCB != null) { RCB.EmptyMessage = ""; RCB.Text = ""; }

                    foreach (RadSiteMapNode iNode in RSM.GetAllNodes())
                    {
                        foreach (Control C in iNode.Controls)
                        {
                            if (C is CheckBox)
                            {
                                ((CheckBox)C).Checked = true;
                                if (iNode.Value != "" && RCB != null)
                                {
                                    RCB.EmptyMessage += iNode.Text + ";";
                                    RCB.Text += iNode.Text + ";";
                                }
                            }
                        }
                    }
                    return "";
                    #endregion
                }
                else
                {
                    #region 根据默认值选中
                    if (defaultStrList != "")
                    {
                        #region 验证默认值
                        String CheckDefaultValue = "";
                        int findIndex = -1;
                        myDV.Sort = "RsmName";
                        foreach (String Value in defaultStrList.Split(';'))
                        {
                            findIndex = myDV.Find(Value);
                            if (findIndex >= 0) CheckDefaultValue += myDV[findIndex]["RsmName"] + ";";
                        }
                        myDV.Sort = "POrder,COrder";
                        if (defaultStrList == "" && CheckDefaultValue == "")
                        {
                            CheckDefaultValue = myDV[0]["RsmName"].ToString();
                        }
                        CheckDefaultValue = CheckDefaultValue.Trim(';');
                        #endregion

                        #region 设置默认值
                        if (RCB != null)
                        {
                            RCB.EmptyMessage = CheckDefaultValue;
                            RCB.Text = CheckDefaultValue;
                        }
                        int i = 0, j = 0;
                        foreach (RadSiteMapNode L1 in RSM.Nodes)
                        {
                            i = 0;
                            j = 0;
                            foreach (RadSiteMapNode L2 in L1.Nodes)
                            {
                                foreach (Control C in L2.Controls)
                                {
                                    if (C.GetType().Name == "CheckBox" && CheckDefaultValue.Split(';').Length > 0)
                                    {
                                        foreach (String item in CheckDefaultValue.Split(';'))
                                        {
                                            if (((CheckBox)C).Text == item)
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
                    else
                    {
                        #region 默认值
                        int i = 0;
                        foreach (RadSiteMapNode N in RSM.GetAllNodes())
                        {
                            foreach (Control C in N.Controls)
                            {
                                if (C is CheckBox && !String.IsNullOrEmpty(N.Value) && i == 0 && RCB != null)
                                {
                                    ((CheckBox)C).Checked = true;
                                    RCB.EmptyMessage = N.Text;
                                    RCB.Text = N.Text;
                                    i++;
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    return "";
                    #endregion
                }
            }
            else
            { if (RCB.ID == "RadCBoxPoint") return "没有测点权限，管理员未分配权限！"; else return ""; }
        }
        /// <summary>
        /// 绑定站点
        /// </summary>
        /// <param name="objCtrl">绑定对象: RadCombox、 RadSiteMap</param>
        /// <param name="applicationType">应用类型: 水、 气、声</param>
        /// <param name="CbxRsmType">SiteMap种类（0、测点；1、通道因子；2、用户；3、状态因子）</param>
        /// <param name="pointType">测点绑定类型：类型、区域</param>
        /// <param name="defaultStrList">默认选择的值(名称1;名称2;…)</param>
        /// <param name="notIn">不需要显示:目前只支持Guid(逗号分隔)</param>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="_defaultAudit">是否常规站</param>
        /// <param name="a">区分同名方法字段</param>
        /// <param name="IsCheckAll">是否全选(可能选参数：默认为false)</param>
        /// <param name="isAllSel">是否所有参数</param>
        /// <returns></returns>
        public String BindRSM(object objCtrl, ApplicationType applicationType, CbxRsmType rsmType, RsmPointMode pointType, String defaultStrList, String notIn, String userGuid, String _defaultAudit, String a, Boolean IsCheckAll = false, bool isAllSel = false)
        {
            #region 获取控件对象
            RadSiteMap RSM = null;
            RadComboBox RCB = null;

            if (objCtrl is RadComboBox)
            {
                RCB = ((RadComboBox)objCtrl);
                RSM = ((RadSiteMap)RCB.Items[0].FindControl("RSM"));
            }
            else
            { RSM = objCtrl as RadSiteMap; }
            #endregion

            #region 数据源
            DataView myDV = g_CbxRsmControlDAL.GetRsmData(applicationType, rsmType, pointType, defaultStrList, notIn, userGuid,_defaultAudit,a, IsCheckAll, isAllSel);
            #endregion

            if (myDV != null && myDV.Count > 0)
            {
                #region 绑定数据
                RSM.DataSource = myDV;
                RSM.DataFieldParentID = "PGuid";
                RSM.DataFieldID = "CGuid";
                RSM.DataTextField = "RsmName";
                RSM.DataValueField = "RsmValue";
                RSM.DataBind();
                #endregion

                if (IsCheckAll)
                {
                    #region 全部选中
                    if (RCB != null) { RCB.EmptyMessage = ""; RCB.Text = ""; }

                    foreach (RadSiteMapNode iNode in RSM.GetAllNodes())
                    {
                        foreach (Control C in iNode.Controls)
                        {
                            if (C is CheckBox)
                            {
                                ((CheckBox)C).Checked = true;
                                if (iNode.Value != "" && RCB != null)
                                {
                                    RCB.EmptyMessage += iNode.Text + ";";
                                    RCB.Text += iNode.Text + ";";
                                }
                            }
                        }
                    }
                    return "";
                    #endregion
                }
                else
                {
                    #region 根据默认值选中
                    if (defaultStrList != "")
                    {
                        #region 验证默认值
                        String CheckDefaultValue = "";
                        int findIndex = -1;
                        myDV.Sort = "RsmName";
                        foreach (String Value in defaultStrList.Split(';'))
                        {
                            findIndex = myDV.Find(Value);
                            if (findIndex >= 0) CheckDefaultValue += myDV[findIndex]["RsmName"] + ";";
                        }
                        myDV.Sort = "POrder,COrder";
                        if (defaultStrList == "" && CheckDefaultValue == "")
                        {
                            CheckDefaultValue = myDV[0]["RsmName"].ToString();
                        }
                        CheckDefaultValue = CheckDefaultValue.Trim(';');
                        #endregion

                        #region 设置默认值
                        if (RCB != null)
                        {
                            RCB.EmptyMessage = CheckDefaultValue;
                            RCB.Text = CheckDefaultValue;
                        }
                        int i = 0, j = 0;
                        foreach (RadSiteMapNode L1 in RSM.Nodes)
                        {
                            i = 0;
                            j = 0;
                            foreach (RadSiteMapNode L2 in L1.Nodes)
                            {
                                foreach (Control C in L2.Controls)
                                {
                                    if (C.GetType().Name == "CheckBox" && CheckDefaultValue.Split(';').Length > 0)
                                    {
                                        foreach (String item in CheckDefaultValue.Split(';'))
                                        {
                                            if (((CheckBox)C).Text == item)
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
                    else
                    {
                        #region 默认值
                        int i = 0;
                        foreach (RadSiteMapNode N in RSM.GetAllNodes())
                        {
                            foreach (Control C in N.Controls)
                            {
                                if (C is CheckBox && !String.IsNullOrEmpty(N.Value) && i == 0 && RCB != null)
                                {
                                    ((CheckBox)C).Checked = true;
                                    RCB.EmptyMessage = N.Text;
                                    RCB.Text = N.Text;
                                    i++;
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    return "";
                    #endregion
                }
            }
            else
            { 
                if (RCB.ID == "RadCBoxPoint")
                    return "没有测点权限，管理员未分配权限！"; 
                else 
                    return ""; 
            }
        }
        /// <summary>
        /// 设置选择默认值
        /// </summary>
        /// <param name="objCtrl"></param>
        /// <param name="defaultNameList">默认测站名：以";"分割</param>
        /// <param name="IsCheckAll"></param>
        /// <returns></returns>
        public String SetSelect(object objCtrl, String defaultNameList, Boolean IsCheckAll = false)
        {
            #region 获取控件对象
            RadSiteMap RSM = null;
            RadComboBox RCB = null;

            if (objCtrl is RadComboBox)
            {
                RCB = ((RadComboBox)objCtrl);
                RSM = ((RadSiteMap)RCB.Items[0].FindControl("RSM"));
            }
            else
            { RSM = objCtrl as RadSiteMap; }
            #endregion

            if (IsCheckAll)
            {
                #region 全部选中
                if (RCB != null) { RCB.EmptyMessage = ""; RCB.Text = ""; }

                foreach (RadSiteMapNode iNode in RSM.GetAllNodes())
                {
                    foreach (Control C in iNode.Controls)
                    {
                        if (C is CheckBox)
                        {
                            ((CheckBox)C).Checked = true;
                            if (iNode.Value != "" && RCB != null)
                            {
                                RCB.EmptyMessage += iNode.Text + ";";
                                RCB.Text += iNode.Text + ";";
                            }
                        }
                    }
                }
                return "";
                #endregion
            }
            else
            {
                #region 根据默认值选中
                if (!string.IsNullOrEmpty(defaultNameList))
                {
                    String CheckDefaultValue = "";
                    #region 设置默认值

                    int i = 0, j = 0;
                    foreach (RadSiteMapNode L1 in RSM.Nodes)
                    {
                        i = 0;
                        j = 0;
                        foreach (RadSiteMapNode L2 in L1.Nodes)
                        {
                            foreach (Control C in L2.Controls)
                            {
                                if (C.GetType().Name == "CheckBox" && defaultNameList.Split(';').Length > 0)
                                {
                                    bool isChecked = false;
                                    foreach (String item in defaultNameList.Split(';'))
                                    {
                                        if (((CheckBox)C).Text == item)
                                        {
                                            ((CheckBox)C).Checked = true;
                                            isChecked = true;
                                            j++;
                                            CheckDefaultValue = CheckDefaultValue + ";" + item;
                                        }
                                    }
                                    if (!isChecked)
                                        ((CheckBox)C).Checked = false;
                                }
                                if (C.GetType().Name == "CheckBox") i++;
                            }
                        }
                        if (i == j && j != 0)
                            ((CheckBox)L1.Controls[1]).Checked = true;
                        else
                            ((CheckBox)L1.Controls[1]).Checked = false;
                    }
                    CheckDefaultValue = CheckDefaultValue.Trim(';');
                    if (RCB != null)
                    {
                        RCB.EmptyMessage = CheckDefaultValue;
                        RCB.Text = CheckDefaultValue;
                    }
                    #endregion
                }
                else
                {
                    #region 默认值
                    int i = 0;
                    foreach (RadSiteMapNode N in RSM.GetAllNodes())
                    {
                        foreach (Control C in N.Controls)
                        {
                            if (C is CheckBox && !String.IsNullOrEmpty(N.Value) && i == 0 && RCB != null)
                            {
                                ((CheckBox)C).Checked = true;
                                RCB.EmptyMessage = N.Text;
                                RCB.Text = N.Text;
                                i++;
                                break;
                            }
                        }
                    }
                    #endregion
                }
                return "";
                #endregion
            }

        }
        /// <summary>
        /// 设置选择默认值
        /// </summary>
        /// <param name="objCtrl"></param>
        /// <param name="defaultNameList">默认测站名：以";"分割</param>
        /// <param name="IsCheckAll"></param>
        /// <returns></returns>
        public String SetSelectNew(object objCtrl, String defaultNameList, Boolean IsCheckAll = false)
        {
            #region 获取控件对象
            RadSiteMap RSM = null;
            RadComboBox RCB = null;

            if (objCtrl is RadComboBox)
            {
                RCB = ((RadComboBox)objCtrl);
                RSM = ((RadSiteMap)RCB.Items[0].FindControl("RSM"));
            }
            else
            { RSM = objCtrl as RadSiteMap; }
            #endregion

            if (IsCheckAll)
            {
                #region 全部选中
                if (RCB != null) { RCB.EmptyMessage = ""; RCB.Text = ""; }

                foreach (RadSiteMapNode iNode in RSM.GetAllNodes())
                {
                    foreach (Control C in iNode.Controls)
                    {
                        if (C is CheckBox)
                        {
                            ((CheckBox)C).Checked = true;
                            if (iNode.Value != "" && RCB != null)
                            {
                                RCB.EmptyMessage += iNode.Text + ";";
                                RCB.Text += iNode.Text + ";";
                            }
                        }
                    }
                }
                return "";
                #endregion
            }
            else
            {
                #region 根据默认值选中
                if (!string.IsNullOrEmpty(defaultNameList))
                {
                    String CheckDefaultValue = "";
                    #region 设置默认值

                    int i = 0, j = 0;
                    foreach (RadSiteMapNode L1 in RSM.Nodes)
                    {
                        i = 0;
                        j = 0;
                        foreach (RadSiteMapNode L2 in L1.Nodes)
                        {
                            foreach (Control C in L2.Controls)
                            {
                                if (C.GetType().Name == "CheckBox" && defaultNameList.Split(';').Length > 0)
                                {
                                    bool isChecked = false;
                                    foreach (String item in defaultNameList.Split(';'))
                                    {
                                        if (!string.IsNullOrWhiteSpace(item) && ((CheckBox)C).ToolTip.Contains(item))
                                        {
                                            ((CheckBox)C).Checked = true;
                                            isChecked = true;
                                            j++;
                                            CheckDefaultValue = CheckDefaultValue + ";" + ((CheckBox)C).Text;
                                        }
                                    }
                                    if (!isChecked)
                                        ((CheckBox)C).Checked = false;
                                }
                                if (C.GetType().Name == "CheckBox") i++;
                            }
                        }
                        if (i == j && j != 0)
                            ((CheckBox)L1.Controls[1]).Checked = true;
                        else
                            ((CheckBox)L1.Controls[1]).Checked = false;
                    }
                    CheckDefaultValue = CheckDefaultValue.Trim(';');
                    if (RCB != null)
                    {
                        RCB.EmptyMessage = CheckDefaultValue;
                        RCB.Text = CheckDefaultValue;
                    }
                    #endregion
                }
                else
                {
                    #region 默认值
                    int i = 0;
                    foreach (RadSiteMapNode N in RSM.GetAllNodes())
                    {
                        foreach (Control C in N.Controls)
                        {
                            if (C is CheckBox && !String.IsNullOrEmpty(N.Value) && i == 0 && RCB != null)
                            {
                                ((CheckBox)C).Checked = true;
                                RCB.EmptyMessage = N.Text;
                                RCB.Text = N.Text;
                                i++;
                                break;
                            }
                        }
                    }
                    #endregion
                }
                return "";
                #endregion
            }

        }
        /// <summary>
        /// 根据区域名称获取下属站点的名称
        /// </summary>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public List<string> GetPointNameByRegion(string regionName)
        {
            return g_CbxRsmControlDAL.GetPointNameByRegion(regionName);
        }

        #region << 取得选择值 >>
        public List<IPoint> GetRSMPointValue(object objCtrl)
        {
            RadSiteMap RSM = null;
            if (objCtrl is RadComboBox)
            { RSM = ((RadSiteMap)((RadComboBox)objCtrl).Items[0].FindControl("RSM")); }
            else
            { RSM = objCtrl as RadSiteMap; }


            List<IPoint> rsmPoint = new List<IPoint>();
            String[] selValue = null;
            foreach (RadSiteMapNode N in RSM.GetAllNodes())
            {
                foreach (Control C in N.Controls)
                {
                    if (C is CheckBox && ((CheckBox)C).Checked == true && !String.IsNullOrEmpty(N.Value))
                    {
                        selValue = N.Value.Split(':');
                        rsmPoint.Add(new RsmPoint(N.Text, selValue[0], selValue[1]));
                    }
                }
            }
            return rsmPoint;
        }

        public List<IPoint> GetAllPointValue(object objCtrl)
        {
            RadSiteMap RSM = null;
            if (objCtrl is RadComboBox)
            { RSM = ((RadSiteMap)((RadComboBox)objCtrl).Items[0].FindControl("RSM")); }
            else
            { RSM = objCtrl as RadSiteMap; }


            List<IPoint> rsmPoint = new List<IPoint>();
            String[] selValue = null;
            foreach (RadSiteMapNode N in RSM.GetAllNodes())
            {
                foreach (Control C in N.Controls)
                {
                    if (C is CheckBox && !String.IsNullOrEmpty(N.Value))
                    {
                        selValue = N.Value.Split(':');
                        rsmPoint.Add(new RsmPoint(N.Text, selValue[0], selValue[1]));
                    }
                }
            }
            return rsmPoint;
        }

        public List<IPollutant> GetRSMFactorValue(object objCtrl)
        {
            RadSiteMap RSM = null;
            if (objCtrl is RadComboBox)
            { RSM = ((RadSiteMap)((RadComboBox)objCtrl).Items[0].FindControl("RSM")); }
            else
            { RSM = objCtrl as RadSiteMap; }

            List<IPollutant> rsmFactor = new List<IPollutant>();
            String[] selValue = null;
            foreach (RadSiteMapNode N in RSM.GetAllNodes())
            {
                foreach (Control C in N.Controls)
                {
                    if (C is CheckBox && ((CheckBox)C).Checked == true && !String.IsNullOrEmpty(N.Value))
                    {
                        selValue = N.Value.Split(':');
                        rsmFactor.Add(new RsmFactor(N.Text, selValue[0], selValue[1], selValue[2], selValue[3]));
                    }
                }
            }
            return rsmFactor;
        }

        public List<RsmUser> GetRSMUserValue(object objCtrl)
        {
            RadSiteMap RSM = null;
            if (objCtrl is RadComboBox)
            { RSM = ((RadSiteMap)((RadComboBox)objCtrl).Items[0].FindControl("RSM")); }
            else
            { RSM = objCtrl as RadSiteMap; }

            List<RsmUser> rsmUser = new List<RsmUser>();
            String[] selValue = null;
            foreach (RadSiteMapNode N in RSM.GetAllNodes())
            {
                foreach (Control C in N.Controls)
                {
                    if (C is CheckBox && ((CheckBox)C).Checked == true && !String.IsNullOrEmpty(N.Value))
                    {
                        selValue = N.Value.Split(':');
                        rsmUser.Add(new RsmUser(N.Text, selValue[0], selValue[1], selValue[2], selValue[3], selValue[4]));
                    }
                }
            }
            return rsmUser;
        }

        /// <summary>
        /// 获取Telerik ComboBox RadSiteMap 值(目前完成Factor)
        /// </summary>
        /// <param name="RSM">RadSiteMap对象</param>
        /// <param name="cbxRsmType">ComboBox绑定的类型</param>
        /// <param name="ReturnType">返回值类型</param>
        /// <returns></returns>
        public String GetRSMValue(RadComboBox RCB, CbxRsmType cbxRsmType, CbxRsmReturnType ReturnType)
        {
            RadSiteMap RSM = ((RadSiteMap)RCB.Items[0].FindControl("RSM"));
            #region 获取值
            String selGuid = "", selID = "", selCode = "", selCol = "", selName = "";
            foreach (RadSiteMapNode N in RSM.GetAllNodes())
            {
                foreach (Control C in N.Controls)
                {
                    if (C.GetType().Name == "CheckBox" && ((CheckBox)C).Checked == true && ((CheckBox)C).ToolTip != "")
                    {
                        switch (cbxRsmType)
                        {
                            case CbxRsmType.Point:
                                selID += selID == "" ? ((CheckBox)C).ToolTip.Split(':')[0] : ";" + ((CheckBox)C).ToolTip.Split(':')[0];
                                selGuid += selGuid == "" ? ((CheckBox)C).ToolTip.Split(':')[1] : ";" + ((CheckBox)C).ToolTip.Split(':')[1];
                                selName += selName == "" ? ((CheckBox)C).Text : ";" + ((CheckBox)C).Text;
                                break;
                            case CbxRsmType.ChannelFactor:
                                selCode += selCode == "" ? ((CheckBox)C).ToolTip.Split(':')[0] : ";" + ((CheckBox)C).ToolTip.Split(':')[0];
                                selCol += selCol == "" ? ((CheckBox)C).ToolTip.Split(':')[1] : ";" + ((CheckBox)C).ToolTip.Split(':')[1];
                                selGuid += selGuid == "" ? ((CheckBox)C).ToolTip.Split(':')[3] : ";" + ((CheckBox)C).ToolTip.Split(':')[3];
                                selName += selName == "" ? ((CheckBox)C).Text : ";" + ((CheckBox)C).Text;
                                break;

                            default:
                                selName += selName == "" ? ((CheckBox)C).Text : ";" + ((CheckBox)C).Text;
                                break;
                        }

                    }
                }
            }
            #endregion

            #region 按类型返回值
            switch (ReturnType)
            {
                case CbxRsmReturnType.Guid://Guid
                    return selGuid;
                case CbxRsmReturnType.ID://ID
                    return selID;
                case CbxRsmReturnType.Code://Code
                    return selCode;
                case CbxRsmReturnType.Name://Name
                    return selName;
                case CbxRsmReturnType.ID_Guid_Name:
                    return selID + ":" + selGuid + ":" + selName;
                default:
                    return "";
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RCB"></param>
        /// <param name="IDorName"></param>
        /// <returns></returns>
        public String[] GetRSMValue(RadComboBox RCB, String IDorName)
        {
            RadSiteMap RSM = ((RadSiteMap)RCB.Items[0].FindControl("RSM"));
            #region 获取值
            ArrayList SelGuid = new ArrayList();
            ArrayList SelName = new ArrayList();

            foreach (RadSiteMapNode N in RSM.GetAllNodes())
            {
                foreach (Control C in N.Controls)
                {
                    if (C.GetType().Name == "CheckBox" && ((CheckBox)C).Checked == true && ((CheckBox)C).ToolTip != "")
                    {
                        SelGuid.Add(((CheckBox)C).ToolTip);
                        SelName.Add(((CheckBox)C).Text);
                    }
                }
            }
            #endregion

            switch (IDorName)
            {
                case "Guid":
                    return (String[])SelGuid.ToArray(typeof(String));
                case "Name":
                    return (String[])SelName.ToArray(typeof(String));
                default:
                    return null;
            }
        }
        #endregion
    }
}
