using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.RemoteControl
{
    public partial class AppointmentToolTip : System.Web.UI.UserControl
    {
        private Appointment apt;

        public Appointment TargetAppointment
        {
            get
            {
                return apt;
            }

            set
            {
                apt = value;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            try
            {
                //StartingOn.Text = apt.Owner.UtcToDisplay(apt.Start).ToString();
                pointName.Text = apt.Subject;
                FullText.Text = apt.Description.Replace(",", "\r\n");
                EndOn.Text = Convert.ToDateTime(apt.Owner.UtcToDisplay(apt.End).ToString()).ToString("yyyy年MM月dd日");
            }
            catch
            {
            }
            ;
        }
    }
}