namespace SmartEP.Service.Core.Control
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Telerik.Web.UI;

    public class Binding
    {
        #region Constructors

        public Binding()
        {
        }

        #endregion Constructors

        #region Nested Types

        public class BindingTreeView
        {
            #region Fields

            private object dataSource;
            private string parentId;
            private string textField;
            private RadTreeView treeview;
            private string valueField;

            #endregion Fields

            #region Constructors

            public BindingTreeView()
            {
                this.treeview = TreeView;
                this.dataSource = DataSource;
                this.parentId = ParentId;
                this.textField = TextField;
                this.valueField = ValueField;
            }

            #endregion Constructors

            #region Properties

            public object DataSource
            {
                get
                {
                    return dataSource;
                }
                set
                {
                    dataSource = value;
                }
            }

            public string ParentId
            {
                get
                {
                    return parentId;
                }
                set
                {
                    parentId = value;
                }
            }

            public string TextField
            {
                get
                {
                    return textField;
                }
                set
                {
                    textField = value;
                }
            }

            public RadTreeView TreeView
            {
                get
                {
                    return treeview;
                }
                set
                {
                    treeview = value;
                }
            }

            public string ValueField
            {
                get
                {
                    return valueField;
                }
                set
                {
                    valueField = value;
                }
            }

            #endregion Properties

            #region Methods

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <typeparam name="TSource"></typeparam>
            /// <param name="ctrl"></param>
            /// <param name="dataSource"></param>
            public static void BindDataControl<T, TSource>(T ctrl, TSource dataSource)
                where T : System.Web.UI.WebControls.BaseDataList
                where TSource : System.Collections.IList
            {
                ctrl.DataSourceID = null;
                ctrl.DataSource = dataSource;
                ctrl.DataBind();
            }

            #endregion Methods

            #region Other

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <typeparam name="TSource"></typeparam>
            /// <param name="ctrl"></param>
            /// <param name="dataSource"></param>
            //public void BindTreeControl<T, TSource>(T ctrl, TSource dataSource)
            //    where T : HierarchicalControlItemContainer,System.Web.UI.WebControls.BaseDataBoundControl
            //    where TSource : System.Collections.IList
            //{
            //    ctrl.DataSource = dataSource;
            //    ctrl.DataFieldParentID = parentId;
            //    ctrl.DataFieldID = valueField;
            //    ctrl.DataTextField = textField;
            //    ctrl.DataValueField = valueField;
            //    ctrl.DataBind();
            //}

            #endregion Other
        }

        #endregion Nested Types
    }
}