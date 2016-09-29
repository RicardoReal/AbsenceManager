using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Expressions;
using AbsenceManager.Security;

namespace AbsenceManager
{
    public partial class Insert : System.Web.UI.Page
    {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e)
        {
            table = DynamicDataRouteHandler.GetRequestMetaTable(Context);
            FormView1.SetMetaTable(table, table.GetColumnValuesFromRoute(Context));
            DetailsDataSource.EntityTypeFilter = table.EntityType.Name;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = table.DisplayName;
            if (!IsPostBack)
            {
                ViewState["PreviousPageURL"] = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();
            }
        }

        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == DataControlCommands.CancelCommandName)
            {
                Response.Redirect(table.ListActionPath);
            }
        }

        protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception == null || e.ExceptionHandled)
            {
                if (PermissionsManager.UserIsAdmin()) Response.Redirect(table.ListActionPath);
                else
                {
                    string url = ViewState["PreviousPageURL"].ToString();
                    if (url == "" || url.Contains("ReturnUrl=")) url = "~/Default.aspx";
                    Response.Redirect(url);
                }
            }
            else
            {
                if (e.Exception.GetType() == typeof(ValidationException))
                {
                    customValida.ErrorMessage = e.Exception.Message;
                    customValida.IsValid = false;
                    e.ExceptionHandled = true;
                    e.KeepInInsertMode = true;
                }
            }
        }

    }
}
