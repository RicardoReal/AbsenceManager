using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AbsenceManager
{
    public partial class SiteNoMenuLogin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string format = "yyyy-MM-dd HH:mm";
            LabelReleaseDate.Text = ((DateTime)Context.Application.Get("BuildDate")).ToString(format);

            CurrentYear.Text = DateTime.Now.Year.ToString();
        }
    }
}