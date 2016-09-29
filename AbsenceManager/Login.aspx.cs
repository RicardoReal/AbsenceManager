using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AbsenceManager.Security;

namespace AbsenceManager
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            ((TextBox)LoginUser.FindControl("UserName")).Focus();
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            AMMembershipProvider gomp = (AMMembershipProvider)System.Web.Security.Membership.Provider;
            string name = gomp.GetName(
                ((TextBox)LoginUser.FindControl("UserName")).Text
                );

            if (this.Session["Name"] != null)
                this.Session["Name"] = name;
            else
                this.Session.Add("Name", name);


            using (AM_Entities ent = new AM_Entities())
            {
                Role role = (from u in ent.Users
                             join r in ent.Roles on u.RoleID equals r.ID
                             where u.Nome == name
                             select r).FirstOrDefault();

                if (role.EmpresaID != null)
                {
                    this.Session.Add("EmpresaID", role.EmpresaID);
                }
            }

        }
    }
}