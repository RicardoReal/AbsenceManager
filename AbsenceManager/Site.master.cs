using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using System;
using System.Web;
using System.Web.Security;
using AbsenceManager.Security;
using System.Linq;

namespace AbsenceManager
{
    public partial class Site : System.Web.UI.MasterPage
    {
        string menuHTML = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    GenerateMenu();

                    int ct = ent.FeriasFuncionarios.Where(x => x.Pendente == true).Count();

                    if (PermissionsManager.UserIsAdmin())
                    {
                        avisoslink.Visible = ct > 0;
                    }
                    else avisoslink.Visible = false;
                }
            }

            string format = "yyyy-MM-dd HH:mm";
            LabelReleaseDate.Text = ((DateTime)Context.Application.Get("BuildDate")).ToString(format);

            // Nome do utilizador
            AMMembershipProvider gomp = (AMMembershipProvider)System.Web.Security.Membership.Provider;
            string name = gomp.GetName(Page.User.Identity.Name);

            if (Session["Name"] == null)
            {
                if (this.Session["Name"] != null)
                    this.Session["Name"] = name;
                else
                    this.Session.Add("Name", name);
            }

            LoginName.InnerHtml = Session["Name"].ToString();
        }

        #region Menu c/ dropdown na settings
        private void GenerateMenu()
        {
            if (this.Session["MenuHTML"] == null)
            {
                menuHTML += "<ul>";

                ProcessNode(SiteMap.RootNode);

                menuHTML += "</ul>";

                this.Session.Add("MenuHTML", menuHTML);
            }

            menubar.InnerHtml = this.Session["MenuHTML"].ToString();
        }

        private void ProcessNode(SiteMapNode smn)
        {
            if ((DisplayMenuElement(smn) && smn.ParentNode != null) || smn.ParentNode == null)
            {
                if (!smn.Url.Contains("Edit.aspx") && !smn.Url.Contains("Details.aspx"))
                {
                    // Gera nó sem childs
                    if (!smn.HasChildNodes)
                    {
                        menuHTML += "<li><a id=\"" + smn.Title + "\" href=\"" + smn.Url + "\"> " + smn.Description + "</a></li>";
                    }
                    else
                    {
                        // Gera nó com childs sem ser o root
                        if (smn.Title != "Default")
                        {
                            if (DisplayMenuElement(smn))
                            {
                                menuHTML += "<li><a id=\"" + smn.Title + "\" href=\"#\"> " + smn.Description + "</a><ul>";

                                foreach (SiteMapNode smnChild in smn.ChildNodes)
                                {
                                    ProcessNode(smnChild);
                                }

                                menuHTML += "</ul></li>";
                            }
                        }
                        else
                        {
                            //Gera nó root
                            foreach (SiteMapNode smnChild in smn.ChildNodes)
                            {
                                ProcessNode(smnChild);
                            }
                        }
                    }
                }
            }
        }

        // Determina se o user tem acesso à página
        private bool DisplayMenuElement(SiteMapNode smn)
        {
            AMRoleProvider gprp = (AMRoleProvider)Roles.Provider;
            //string table = smn.Url == ""
            //    ? smn.Title
            //    : smn.Url.Split('/')[1];

            string table = smn.Title;

            ScreenPermissions sp = gprp.GetPermissionsForScreenInUser(table, Membership.GetUser().UserName);

            if (sp >= ScreenPermissions.Read)
            {
                return true;
            }
            else if (smn.HasChildNodes && smn.Title != "Default")
            {
                foreach (SiteMapNode _Node in smn.ChildNodes) if (DisplayMenuElement(_Node)) return true;
            }

            return false;
        }
        #endregion


        protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {
            Session.Abandon();
        }

        protected void Continue_Click(object sender, EventArgs e)
        {
            ModalPopupExtenderPass.Hide();
            Response.Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtenderPass.Hide();
        }

    }
}
