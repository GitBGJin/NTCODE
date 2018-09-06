namespace SmartEP.WebControl
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;

    /// <summary>
    /// ServerControl1 的摘要说明
    /// </summary>
    public class ServerControl1 : ScriptControl
    {
        #region Constructors

        public ServerControl1()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #endregion Constructors

        #region Methods

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("SmartEP.WebControl.ClientControl1", this.ClientID);
            yield return descriptor;
        }

        // 生成脚本引用
        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            yield return new ScriptReference("SmartEP.WebControl.ClientControl1.js", this.GetType().Assembly.FullName);
        }

        #endregion Methods
    }
}