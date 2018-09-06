using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.Core.Enums;
using SmartEP.WebControl.CbxRsm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Service.DataAuditing.AuditBaseInfo;

namespace SmartEP.WebUI.Controls
{
    public partial class FactorRsmAuditHgCopy : System.Web.UI.UserControl
    {
        #region 属性
        /// <summary>  
        /// 定义委托  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        public delegate void SelectedChangeHandler();
        public event SelectedChangeHandler SelectedChanged;
        CbxRsmControl myRSM = new CbxRsmControl();
        AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
        public static IQueryable<PointPollutantInfo> pollutantList = null;
        public static IList<PointPollutantInfo> pList = null;
        public static List<PointPollutantInfo> selectFactor = new List<PointPollutantInfo>();
        public static int firstLoad = 1;

        /// <summary>
        /// 复选
        /// </summary>
        private bool _multiSelected = true;
        public bool MultiSelected
        {
            set { this._multiSelected = value; }
        }

        /// <summary>
        /// 应用类型
        /// </summary>
        private ApplicationType _applicationType = ApplicationType.Air;
        public ApplicationType ApplicationType
        {
            set { this._applicationType = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                firstLoad = 1;
                //Session["FactorCode"] = "";
                //Session["FactorName"] = "";
                //Session["isEdit"] = "";
                //Session["PollutantDecimalNum"] = "";
                //Session["PollutantUnit"] = "";
                //FactorBind();//因子绑定
            }
        }

        #region 数据绑定
        /// <summary>
        /// 单站
        /// </summary>
        /// <param name="portid"></param>
        public void FactorBind(int portid)
        {
            pollutantList = null;
            pollutantList = pollutantService.RetrieveHgSiteMapPollutantList(portid, Session["applicationUID"].ToString(), Session["UserGUID"].ToString()); ;
            RSM.DataSource = pollutantList;
            RSM.DataFieldParentID = "PGuid";
            RSM.DataFieldID = "CGuid";
            RSM.DataTextField = "PName";
            RSM.DataValueField = "PID";
            RSM.DataBind();
        }

        /// <summary>
        /// 多站
        /// </summary>
        /// <param name="portid"></param>
        public void FactorBind(string[] portid, int PointType,bool ischeckALL=false)
        {
            if (portid != null && !portid.Contains(""))
            {
                pollutantList = null;
                pollutantList = pollutantService.RetrieveHgSiteMapPollutantList(portid, Session["applicationUID"].ToString(), Session["UserGUID"].ToString(), PointType); ;
                RSM.DataSource = pollutantList;
                RSM.DataFieldParentID = "PGuid";
                RSM.DataFieldID = "CGuid";
                RSM.DataTextField = "PName";
                RSM.DataValueField = "PID";
                RSM.DataBind();

                pList = pollutantList.ToList();
                int i = 0;
                int j = 0;
                if (pollutantList != null)
                {
                    foreach (RadSiteMapNode L1 in RSM.Nodes)
                    {
                        i = 0;
                        j = 0;
                        foreach (RadSiteMapNode L2 in L1.Nodes)
                        {
                            foreach (Control C in L2.Controls)
                            {
                                if (C.GetType().Name == "CheckBox")
                                {
                                    try
                                    {
                                        CheckBox checkBox = ((CheckBox)C);
                                        checkBox.ToolTip = L2.Value.Split(':')[0];
                                        if (ischeckALL==false&&Session["FactorCode"] != null && !Session["FactorCode"].Equals(""))
                                        {
                                            if (Session["FactorCode"].ToString().Split(';').Contains(L2.Value.Split(':')[0].ToString()))
                                            {
                                                checkBox.Checked = true;
                                                j++;
                                            }
                                            else
                                                checkBox.Checked = false;
                                        }
                                        else
                                        {
                                            checkBox.Checked = true;
                                            j++;
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (C.GetType().Name == "CheckBox") i++;
                            }
                        }
                        if (i == j && j != 0) ((CheckBox)L1.Controls[1]).Checked = true;
                        else ((CheckBox)L1.Controls[1]).Checked = false;
                    }
                }
                //pList = pollutantList.ToList();
                //if (firstLoad == 1 && Session["FactorCode"] != null && !Session["FactorCode"].Equals(""))
                //{
                //    if (pollutantList != null)
                //        foreach (RadSiteMapNode N in RSM.GetAllNodes())
                //        {
                //            foreach (Control C in N.Controls)
                //            {

                //                if (C is CheckBox && !String.IsNullOrEmpty(N.Value))
                //                {
                //                    CheckBox checkBox = ((CheckBox)C);
                //                    //if (Session["FactorCode"] != null)
                //                    //{
                //                    //    if (Session["FactorCode"].ToString().Split(';').Contains(N.Value.ToString()))
                //                    //        checkBox.Checked = true;
                //                    //    else
                //                    //        checkBox.Checked = false;
                //                    //}
                //                    //else
                //                    //{
                //                        checkBox.Checked = true;
                //                    //}
                //                }

                //            }
                //        }
                //}
            }
        }

        #endregion

        #region 事件
        protected void RsmChkA_CheckedChanged(object sender, EventArgs e)
        {
            GetFactors();
            //将自定义事件绑定到控件事件上  
            if (SelectedChanged != null)
            {
                //RadCBoxPoint
                SelectedChanged();
            }
        }

        /// <summary>
        /// 因子选择变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFactorChange_Click(object sender, EventArgs e)
        {
            //将自定义事件绑定到控件事件上  
            if (SelectedChanged != null)
            {
                //RadCBoxPoint
                SelectedChanged();
            }
        }

        /// <summary>
        /// 因子选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RsmChkB_CheckedChanged(object sender, EventArgs e)
        {
            GetFactors();
            //将自定义事件绑定到控件事件上  
            if (SelectedChanged != null)
            {
                //RadCBoxPoint
                SelectedChanged();
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取选中的因子
        /// </summary>
        /// <returns></returns>
        public List<PointPollutantInfo> GetFactors()
        {
            List<PointPollutantInfo> rsmFactorList = new List<PointPollutantInfo>();
            string selCode = "";
            string selName = "";
            string isEdit = "";
            string PollutantDecimalNum = "";
            string PollutantUnit = "";
            foreach (RadSiteMapNode N in RSM.GetAllNodes())
            {
                foreach (Control C in N.Controls)
                {
                    if (C is CheckBox && ((CheckBox)C).Checked == true && !String.IsNullOrEmpty(N.Value))
                    {
                        try
                        {
                            selCode += selCode == "" ? N.Value.Split(':')[0] : ";" + N.Value.Split(':')[0];
                            selName += selName == "" ? ((CheckBox)C).Text : ";" + ((CheckBox)C).Text;
                            PollutantDecimalNum += PollutantDecimalNum == "" ? N.Value.Split(':')[1] : ";" + N.Value.Split(':')[1];
                            //PollutantUnit += PollutantUnit == "" ? N.Value.Split(':')[2] : ";" + N.Value.Split(':')[2];
                            PollutantUnit += N.Value.Split(':')[2] + ";";
                            isEdit += isEdit == "" ? N.Value.Split(':')[3] : ";" + N.Value.Split(':')[3];

                            PointPollutantInfo factor = new PointPollutantInfo();
                            factor.PID = N.Value.Split(':')[0];
                            factor.PName = ((CheckBox)C).Text;
                            rsmFactorList.Add(factor);
                        }
                        catch
                        {
                        }

                    }
                }
            }
            Session["FactorCodeHg"] = selCode;
            Session["FactorNameHg"] = selName;
            Session["isEditHg"] = isEdit;
            Session["PollutantDecimalNumHg"] = PollutantDecimalNum;
            Session["PollutantUnitHg"] = !PollutantUnit.Equals("")?PollutantUnit.Substring(0,PollutantUnit.Length-1):"";
            return rsmFactorList;
        }
        #region 注释
        //public List<PointPollutantInfo> GetFactors()
        //{
        //    List<PointPollutantInfo> rsmFactorList = new List<PointPollutantInfo>();
        //    string selCode = "";
        //    string selName = "";
        //    string isEdit = "";
        //    string PollutantDecimalNum = "";
        //    string PollutantUnit = "";
        //    foreach (RadSiteMapNode N in RSM.GetAllNodes())
        //    {
        //        foreach (Control C in N.Controls)
        //        {
        //            if (C is CheckBox && ((CheckBox)C).Checked == true && !String.IsNullOrEmpty(N.Value))
        //            {
        //                selCode += selCode == "" ? ((CheckBox)C).ToolTip.Split(':')[0] : ";" + ((CheckBox)C).ToolTip.Split(':')[0];
        //                selName += selName == "" ? ((CheckBox)C).Text : ";" + ((CheckBox)C).Text;
        //                //获取因子只读标记                    
        //                if (pollutantList != null)
        //                {
        //                    PointPollutantInfo factor = pollutantList.Where(x => x.PID == N.Value.ToString()).FirstOrDefault();
        //                    if (factor.Tag == null)
        //                        isEdit += isEdit == "" ? "1" : ";1";
        //                    else
        //                        isEdit += isEdit == "" ? factor.Tag : ";" + factor.Tag;
        //                    PollutantUnit += PollutantUnit == "" ? factor.PollutantUnit.ToString() : ";" + factor.PollutantUnit.ToString();
        //                    PollutantDecimalNum += PollutantDecimalNum == "" ? factor.PollutantDecimalNum.ToString() : ";" + factor.PollutantDecimalNum.ToString();
        //                    rsmFactorList.Add(factor);
        //                }
        //                else
        //                {
        //                    isEdit += isEdit == "" ? "1" : ";1";
        //                }
        //            }
        //        }
        //    }
        //    Session["FactorCode"] = selCode;
        //    Session["FactorName"] = selName;
        //    Session["isEdit"] = isEdit;
        //    Session["PollutantDecimalNum"] = PollutantDecimalNum;
        //    Session["PollutantUnit"] = PollutantUnit;
        //    return rsmFactorList;
        //}
        #endregion
        #endregion



    }
}