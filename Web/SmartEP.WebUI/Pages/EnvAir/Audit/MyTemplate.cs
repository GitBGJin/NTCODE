﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Drawing;
using System.Data;
using System.Configuration;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Core.Enums;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    #region 自定义GridView模板
    public class MyTemplate : ITemplate
    {
        protected LiteralControl lControl;
        private string colname;
        int DecimalNum = 3;
        string appGuid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
        public MyTemplate()
        {
        }
        public MyTemplate(int factorDecimalNum)
        {
            DecimalNum = factorDecimalNum;
        }
        public MyTemplate(string cName, int factorDecimalNum, string applicationUid)
        {
            colname = cName;
            DecimalNum = factorDecimalNum;
            appGuid = applicationUid;
        }

        //public MyTemplate(bool edit)
        //{
        //    isEdit = edit;
        //}

        /// <summary>
        /// 添加GridView 模板列
        /// </summary>
        /// <param name="container"></param>
        public void InstantiateIn(System.Web.UI.Control container)
        {
            if (colname != null)
            {
                lControl = new LiteralControl();
                if (colname.Equals("DataDateTime"))
                {
                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(appGuid))
                        lControl.DataBinding += new EventHandler(lControl_DataBindingAir);//时间模板
                    else
                        lControl.DataBinding += new EventHandler(lControl_DataBindingWater);//时间模板
                }
                else
                    lControl.DataBinding += new EventHandler(lControl_DataBindingFactor);//因子模板（数据+标记位）
                container.Controls.Add(lControl);
            }
            else
            {
                RadNumericTextBox numTextBox = new RadNumericTextBox();
                numTextBox.NumberFormat.DecimalDigits = DecimalNum;
                //numTextBox.DataBinding += new EventHandler(RadNumericTextBox_Binding);
                numTextBox.ID = "NumericTextBox";
                container.Controls.Add(numTextBox);
            }
        }

        /// <summary>
        /// 时间模板（环境空气）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void lControl_DataBindingAir(object sender, EventArgs e)
        {
            LiteralControl l = (LiteralControl)sender;
            GridDataItem container = (GridDataItem)l.NamingContainer;
            if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-01"))
                l.Text = "样本总数";
            else if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-02"))
                l.Text = "无效样本数";
            else if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-03"))
                l.Text = "有效样本数";
            else if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-04"))
                l.Text = "最大值";
            else if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-05"))
                l.Text = "最小值";
            else if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-06"))
                l.Text = "平均值";
            else
                //l.Text = Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()).AddHours(1).ToString("dd日HH点") + "<br />";
                l.Text = Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()).AddHours(0).ToString("dd日HH点") + "<br />";
        }

        /// <summary>
        /// 时间模板(地表水)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void lControl_DataBindingWater(object sender, EventArgs e)
        {
            LiteralControl l = (LiteralControl)sender;
            GridDataItem container = (GridDataItem)l.NamingContainer;
            if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-01"))
                l.Text = "样本总数";
            else if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-02"))
                l.Text = "无效样本数";
            else if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-03"))
                l.Text = "有效样本数";
            else if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-04"))
                l.Text = "最大值";
            else if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-05"))
                l.Text = "最小值";
            else if (Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()) == Convert.ToDateTime("1900-01-06"))
                l.Text = "平均值";
            else
                l.Text = Convert.ToDateTime(((DataRowView)container.DataItem)[colname].ToString()).ToString("yyyy-MM-dd HH:mm") + "<br />";
        }

        /// <summary>
        /// 因子模板（数据+标记位）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void lControl_DataBindingFactor(object sender, EventArgs e)
        {
            //DecimalNum = Session["FactorCode"] != null ? Convert.ToInt32(Session["PollutantDecimalNum"].ToString().Split(';')[Session["FactorCode"].ToString().IndexOf(CurrUName)]) : 3;
            LiteralControl l = (LiteralControl)sender;
            try
            {
                GridDataItem container = (GridDataItem)l.NamingContainer;
                if (Convert.ToDateTime(((DataRowView)container.DataItem)["DataDateTime"].ToString()) == Convert.ToDateTime("1900-01-01")
                    || Convert.ToDateTime(((DataRowView)container.DataItem)["DataDateTime"].ToString()) == Convert.ToDateTime("1900-01-02")
                    || Convert.ToDateTime(((DataRowView)container.DataItem)["DataDateTime"].ToString()) == Convert.ToDateTime("1900-01-03"))
                    l.Text = Convert.ToInt32(((DataRowView)container.DataItem)[colname]).ToString();
                else
                {
                    if (((DataRowView)container.DataItem)[colname + "_AuditFlag"].ToString().Equals(""))//无审核标记显示下位标记，有审核标记显示审核标记
                    {
                        l.Text = (((DataRowView)container.DataItem)[colname] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(((DataRowView)container.DataItem)[colname].ToString()), DecimalNum).ToString() : "")
                            + "      " + ((DataRowView)container.DataItem)[colname + "_dataFlag"].ToString();
                    }
                    else
                    {
                        if (((DataRowView)container.DataItem)[colname + "_AuditFlag"].ToString().Contains("RM")) l.Text += (((DataRowView)container.DataItem)[colname] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(((DataRowView)container.DataItem)[colname].ToString()), DecimalNum).ToString() : "")
                            + "      " + "RM";
                        else if (((DataRowView)container.DataItem)[colname + "_AuditFlag"].ToString().Contains("QC")) l.Text += (((DataRowView)container.DataItem)[colname] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(((DataRowView)container.DataItem)[colname].ToString()), DecimalNum).ToString() : "")
                        + "      " + "QC";
                        else if (((DataRowView)container.DataItem)[colname + "_AuditFlag"].ToString().Contains("PF")) l.Text += (((DataRowView)container.DataItem)[colname] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(((DataRowView)container.DataItem)[colname].ToString()), DecimalNum).ToString() : "")
                        + "      " + "PF";
                        else
                        {
                            string status = ((DataRowView)container.DataItem)[colname + "_AuditFlag"].ToString().Replace("N", "").Replace("d", "").Replace("MF", "").Replace(",", "|");
                            l.Text += (((DataRowView)container.DataItem)[colname] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(((DataRowView)container.DataItem)[colname].ToString()), DecimalNum).ToString() : "")
                               + "      " +status.Trim('|');
                            //l.Text += (((DataRowView)container.DataItem)[colname] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(((DataRowView)container.DataItem)[colname].ToString()), DecimalNum).ToString() : "")
                            //    + "      " + (!status.Equals("") && status.Substring(0, 1).Equals("|") ? status.Substring(1, status.Length - 1) : "");
                        }
                    }
                }
            }
            catch
            {
                l.Text = "";
            }

        }

        public void RadNumericTextBox_Binding(object sender, EventArgs e)
        {
            //RadNumericTextBox text = (RadNumericTextBox)sender;
            //GridDataItem container = (GridDataItem)text.NamingContainer;
            //object aa = ((DataRowView)container.DataItem)["DataDateTime"].ToString();
        }
    }
    #endregion
}