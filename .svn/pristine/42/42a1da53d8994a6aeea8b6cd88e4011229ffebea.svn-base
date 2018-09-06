using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using SmartEP.Utilities.AdoData;
using System.ComponentModel;

namespace SmartEP.WebUI.Pages.EnvAir.RemoteControl
{
    public partial class ProtocolCommandTree : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            if (!IsPostBack)
            {
                BindTree();
            }
        }

        protected void BindTree()
        {
            string Sql = "SELECT  distinct commandType from V_Command_Mode_Type where commandMode='请求命令' and commandType!='反控命令'";
            string SqlAll = "SELECT commandType, commandNumber,ISNULL(LTRIM(STR(commandNumber)), '') + ISNULL(commandName, '') AS commandName from V_Command_Mode_Type  where commandMode='请求命令' and  isUseOrNot=1 and commandType!='反控命令'  ORDER BY commandNumber";
            DataView dvParetnNodes = new DatabaseHelper().ExecuteDataView(Sql, "AMS_BaseDataConnection");
            DataView dvAll = new DatabaseHelper().ExecuteDataView(SqlAll, "AMS_BaseDataConnection");

            string strParentNode="";
            List<SiteDataItem> siteData = new List<SiteDataItem>();

            for (var i = 0; i < dvParetnNodes.Count; i++)
            {
                strParentNode = dvParetnNodes[i]["commandType"].ToString();
                //根节点
                siteData.Add(new SiteDataItem(i+1, 0, strParentNode));

                dvAll.RowFilter = "commandType='" + strParentNode + "'";
                for (var j = 0; j < dvAll.Count; j++)
                {
                    siteData.Add(new SiteDataItem(GetValue<int>(dvAll[j]["commandNumber"], 0), i+1, dvAll[j]["commandName"].ToString()));
                }
            }

            RadTreeView1.DataTextField = "Text";
            RadTreeView1.DataValueField = "ID";
            RadTreeView1.DataFieldID = "ID";
            RadTreeView1.DataFieldParentID = "ParentID";
            RadTreeView1.DataSource = siteData;
            RadTreeView1.DataBind();
            RadTreeView1.ExpandAllNodes();
        }

        private string GetStringValue(object source, string defaultValue)
        {
            return (source != null) ? source.ToString() : defaultValue;
        }

        private TValue Parse<TValue>(object source, TValue defaultValue)
        {
            string value = this.GetStringValue(source, null);

            TValue result = defaultValue;
            if (!string.IsNullOrEmpty(value))
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(TValue));
                try
                {
                    result = (TValue)typeConverter.ConvertFrom(value);
                }
                catch { }
            }
            return result;
        }

        private TValue GetValue<TValue>(object source, TValue defaultValue)
        {
            if (source == null)
            {
                return defaultValue;
            }
            string valueToConvert = source.ToString();
            return this.Parse(valueToConvert, defaultValue);
        }

        internal class SiteDataItem
        {
            private string _text;
            private int _id;
            private int _parentId;

            public string Text
            {
                get { return _text; }
                set { _text = value; }
            }


            public int ID
            {
                get { return _id; }
                set { _id = value; }
            }

            public int ParentID
            {
                get { return _parentId; }
                set { _parentId = value; }
            }

            public SiteDataItem(int id, int parentId, string text)
            {
                _id = id;
                _parentId = parentId;
                _text = text;
            }
        }

    }
}