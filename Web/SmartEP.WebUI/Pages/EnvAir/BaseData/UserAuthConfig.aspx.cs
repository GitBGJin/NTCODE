using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Frame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.BaseData
{
    public partial class UserAuthConfig : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitControl();
            }
        }

        private void InitControl()
        {

        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grid.Rebind();
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //每页显示数据个数            
            //int pageSize = grid.PageSize;
            //当前页的序号
            //int currentPageIndex = grid.CurrentPageIndex + 1;
            //查询记录的开始序号
            //int startRecordIndex = pageSize * currentPageIndex;

            //int recordTotal = 0;


            UserService userService = new UserService();
            IList<User> users = userService.GetAllUser().Where(x => x.CName.Contains(txtUserName.Text)).ToList<User>();
            grid.DataSource = users;
            //数据分页的页数
            //grid.VirtualItemCount = pointList.Count();
        }        
    }
}