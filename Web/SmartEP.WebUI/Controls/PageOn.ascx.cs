using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using CustomControl;
using System.Web.UI.WebControls;

namespace CustomControl 
{
    [DefaultProperty("PageSize")]
    [ToolboxData("<{0}:PageOn runat=server Width=100%></{0}:PageOn>")]
    public partial class PageOn : CompositeControl 
    {
        Label lblMessage;
        LinkButton btnFirst;
        LinkButton btnPrev;
        LinkButton btnNext;
        LinkButton btnLast;
        TextBox txtGoPage;
        Button btnGo; 
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #region 定义一个委托
        public delegate void PageOnEventHandler(object sender, EventArgs args);
        #endregion
        #region 定义基于该委托的事件
        public event PageOnEventHandler RecPageChanged;
        #endregion
        #region 重写冒泡事件，并根据参数特征，捕获需要处理的事件，使其调用需要的方法
        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            bool handled = false;
            CommandEventArgs cea = args as CommandEventArgs;
            if (cea == null)
            {
                return handled;
            }
            switch (cea.CommandName)
            {
                case "first":
                    handled = true;
                    CurPage = 1;
                    break;
                case "prev":
                    handled = true;
                    if (CurPage > 1)
                    {
                        CurPage--;
                    }
                    else
                    {
                        CurPage = 1;
                    }
                    break;
                case "next":
                    handled = true;
                    if (CurPage < PageCount)
                    {
                        CurPage++;
                    }
                    else
                    {
                        CurPage = PageCount;
                    }
                    break;
                case "last":
                    handled = true;
                    CurPage = PageCount;
                    break;
                case "go":
                    string strGo = txtGoPage.Text.Trim();
                    int iGo;
                    if (!string.IsNullOrEmpty(strGo) && int.TryParse(strGo, out iGo))
                    {
                        handled = true;
                        CurPage = iGo;
                    }
                    break;
            }
            if (handled)
            {
                if (this.RecPageChanged != null)
                {
                    RecPageChanged(this, args);
                    this.RecreateChildControls();
                }
                return handled;
            }
            else
            {
                return base.OnBubbleEvent(source, args);
            }
        }
        #endregion
        #region 定义分页控件需要用到的Proptery
        public int RowCount
        {
            get
            {
                if (ViewState["m_rowCount"] == null || int.Parse(ViewState["m_rowCount"].ToString()) < 0)
                {
                    ViewState["m_rowCount"] = 0;
                }
                return int.Parse(ViewState["m_rowCount"].ToString());
            }
            set
            {
                if (value < 0)
                {
                    ViewState["m_rowCount"] = 0;
                }
                else
                {
                    ViewState["m_rowCount"] = value;
                }
                this.RecreateChildControls();
            }
        }
        public int CurPage
        {
            get
            {
                if (ViewState["m_curPage"] == null || int.Parse(ViewState["m_curPage"].ToString()) < 1)
                {
                    ViewState["m_curPage"] = 1;
                }
                return int.Parse(ViewState["m_curPage"].ToString());
            }
            set
            {
                if (value < 1)
                {
                    ViewState["m_curPage"] = 1;
                }
                else if (value > PageCount)
                {
                    ViewState["m_curPage"] = PageCount;
                }
                else
                {
                    ViewState["m_curPage"] = value;
                }
            }
        }
        public int PageCount
        {
            get
            {
                return RowCount / PageSize + 1;
            }
        }
        public int PageSize
        {
            get
            {
                if (ViewState["m_pageSize"] == null || int.Parse(ViewState["m_pageSize"].ToString()) < 1)
                {
                    ViewState["m_pageSize"] = 15;
                }
                return int.Parse(ViewState["m_pageSize"].ToString());
            }
            set
            {
                if (value > 0)
                {
                    ViewState["m_pageSize"] = value;
                    this.RecreateChildControls();
                }
            }
        }
        #endregion
        #region 生成自定义控件的子空间
        protected override void CreateChildControls()
        {
            Controls.Clear();
            lblMessage = new Label();
            lblMessage.Text = "当前第" + CurPage + "页 共" + PageCount + "页  共" + RowCount + "条记录";
            lblMessage.ID = "lblMessage";
            Controls.Add(lblMessage);
            btnFirst = new LinkButton();
            btnFirst.Text = "首页";
            btnFirst.CommandName = "first";
            btnFirst.ID = "btnFirst";
            if (CurPage <= 1)
            {
                btnFirst.Enabled = false;
            }
            Controls.Add(btnFirst);
            btnPrev = new LinkButton();
            btnPrev.Text = "上一页";
            btnPrev.CommandName = "prev";
            btnPrev.ID = "btnPrev";
            if (CurPage <= 1)
            {
                btnPrev.Enabled = false;
            }
            Controls.Add(btnPrev);
            btnNext = new LinkButton();
            btnNext.Text = "下一页";
            btnNext.CommandName = "next";
            btnNext.ID = "btnNext";
            if (CurPage >= PageCount)
            {
                btnNext.Enabled = false;
            }
            Controls.Add(btnNext);
            btnLast = new LinkButton();
            btnLast.Text = "末页";
            btnLast.CommandName = "last";
            btnLast.ID = "btnLast";
            if (CurPage >= PageCount)
            {
                btnLast.Enabled = false;
            }
            Controls.Add(btnLast);
            txtGoPage = new TextBox();
            txtGoPage.TabIndex = 1;
            txtGoPage.ID = "txtGoPage";
            txtGoPage.Attributes.Add("onkeyup", @"this.value=this.value.replace(/\D/g,'')");
            txtGoPage.Attributes.Add("onafterpaste", @"this.value=this.value.replace(/\D/g,'')");
            Controls.Add(txtGoPage);
            btnGo = new Button();
            btnGo.TabIndex = 2;
            btnGo.CommandName = "go";
            btnGo.Text = "GO";
            btnGo.ID = "btnGO";
            Controls.Add(btnGo);
            Debug.WriteLine("ffffffffffffffffffffffffffffffffffffffffffffffffff");
            base.CreateChildControls();
        }
        #endregion
        #region 自定义布局
        protected override void RenderContents(HtmlTextWriter output)
        {
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
            output.AddStyleAttribute("text-align", "left");
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            output.Write("  ");
            lblMessage.RenderControl(output);
            output.RenderEndTag();
            output.AddStyleAttribute("text-align", "right");
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            btnFirst.RenderControl(output);
            output.Write("  ");
            btnPrev.RenderControl(output);
            output.Write("  ");
            btnNext.RenderControl(output);
            output.Write("  ");
            btnLast.RenderControl(output);
            output.Write("到");
            output.AddStyleAttribute(HtmlTextWriterStyle.Width, "30px");
            txtGoPage.RenderControl(output);
            output.Write("页");
            btnGo.RenderControl(output);
            output.Write("  ");
            output.RenderEndTag();
            output.RenderEndTag();
        }
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Table;
            }
        }
        #endregion
    }
}