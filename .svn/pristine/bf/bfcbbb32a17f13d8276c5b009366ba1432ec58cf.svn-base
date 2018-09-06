using SmartEP.Core.Generic;
using SmartEP.Service.DataAnalyze.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class SuperStationInterface : SmartEP.WebUI.Common.BasePage
    {
        SuperStationInterfaceService SSInterfaceService = Singleton<SuperStationInterfaceService>.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        protected void Modify_Click(object sender, EventArgs e)
        {
            string password = newPwd.Text;
            SSInterfaceService.ModifyPassword(password);
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {

        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            string password = System.Configuration.ConfigurationManager.AppSettings["SSPassword"];
            SSInterfaceService.ModifyPassword(password);
        }

        //protected void btn2_Click(object sender, EventArgs e)
        //{
        //    var idCell = gridInstrument.SelectedItems["OperateStatus"];
        //    RadButton btn = idCell.FindControl("btn1") as RadButton;
        //    RadButton btn1 = gridInstrument.MasterTableView.FindControl("btn1") as RadButton;
        //    RadButton btn2 = gridInstrument.MasterTableView.FindControl("btn2") as RadButton;
        //    if (btn2.Text == "0")
        //    {
        //        btn2.Text = "1";
        //        btn1.Text = "1";
        //        btn2.Image.ImageUrl = "../../../Img/TelerikImg/1.png";
        //        btn1.Image.ImageUrl = "../../../Img/TelerikImg/off.png";
        //    }
        //    else
        //    {
        //        btn2.Text = "0";
        //        btn1.Text = "0";
        //        btn2.Image.ImageUrl = "../../../Img/TelerikImg/2.png";
        //        btn1.Image.ImageUrl = "../../../Img/TelerikImg/on.png";
        //    }
        //}

        protected void gridInstrument_ItemCommand(object sender, GridCommandEventArgs e)
        {
            var currentItem = e.Item as GridDataItem;
            var id = currentItem["InterfaceName"].UniqueID;
            var interfaceName = currentItem["InterfaceName"].Text;
            //RadButton btn1 = currentItem["CommunicateStatus"].FindControl("btn1") as RadButton;
            GridTableCell pointCell = (GridTableCell)currentItem["CommunicateStatus"];
            RadButton btn2 = currentItem["OperateStatus"].FindControl("btn2") as RadButton;
            if (btn2.Text == "0")
            {
                btn2.Text = "1";
                SSInterfaceService.ModifyStatus(1, int.Parse(btn2.Text), interfaceName);
                btn2.Image.ImageUrl = "imagesPic/1.png";
                pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/Images/icons/Red.png\" />";
            }
            else
            {
                btn2.Text = "0";
                SSInterfaceService.ModifyStatus(0, int.Parse(btn2.Text), interfaceName);
                btn2.Image.ImageUrl = "imagesPic/2.png";
                pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/Images/icons/Green.png\" />";
            }
        }

        protected void gridInstrument_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;

                if (item["CommunicateStatus"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["CommunicateStatus"];

                    if (pointCell.Text == "0")
                    {
                        //pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/icons/Green.PNG\" />";
                        pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/Images/icons/Green.png\" />";
                        //pointCell.ToolTip = "通信启动";
                    }
                    else if (pointCell.Text == "1")
                    {
                        //pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/icons/red.PNG\" />";
                        pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/Images/icons/Red.png\" />";
                        //pointCell.ToolTip = "通信关闭";
                    }
                    else
                    {
                        pointCell.Text = "--";

                    }
                }
            }
        }

        protected void gridInstrument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            gridInstrument.DataSource = SSInterfaceService.getSSInterface();
            //gridInstrument.DataBind();
        }
    }
}