using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Expressions;
using AbsenceManager.Security;
using System.Web.Security;
using System.Collections.Generic;

namespace AbsenceManager
{
    public partial class List : System.Web.UI.Page
    {
        protected MetaTable table;

        private bool _hasOnScreenPermission;

        protected bool HasOnScreenPermission
        {
            get { return _hasOnScreenPermission; }
        }

        private bool _hasTrackChangesPermission;

        protected bool HasTrackChangesPermission
        {
            get { return _hasTrackChangesPermission; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            table = DynamicDataRouteHandler.GetRequestMetaTable(Context);
            var defaultValues = Page.GetFilterValuesFromSession(table, table.GetColumnValuesFromRoute(Context));
            GridView1.SetMetaTable(table, defaultValues);
            GridDataSource.EntityTypeFilter = table.EntityType.Name;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = table.DisplayName;
            GridDataSource.Include = table.ForeignKeyColumnsNames;

            // Disable various options if the table is readonly
            if (table.IsReadOnly)
            {
                GridView1.Columns[0].Visible = false;
                InsertHyperLink.Visible = false;
                GridView1.EnablePersistedSelection = false;
            }

            AMRoleProvider gprp = (AMRoleProvider)System.Web.Security.Roles.Provider;
            String userName = Membership.GetUser().UserName;

            // Page Read Only
            ScreenPermissions sp = gprp.GetPermissionsForScreenInUser(table.Name, userName);

            // Track Changes Permission
            _hasTrackChangesPermission = gprp.UserCanTrackChanges(table.Name, userName) && pageHasTrackChanges();

            //if (sp == ScreenPermissions.Write)
            _hasOnScreenPermission = sp == ScreenPermissions.Write;
            //else
            //{
            //    _hasInlinePermission = sp == ScreenPermissions.InlineWrite;
            //    _hasOnScreenPermission = sp == ScreenPermissions.OnScreenWrite;
            //}

            if (!PermissionsManager.IsInsertAvailable(table))
                InsertHyperLink.Visible = false;

            // Configura os campos a pesquisar no search com base no web.config
            SearchExpression se = (SearchExpression)GridQueryExtender.Expressions[1];
            if (!SearchHelpers.ConfigureSearch(table.EntityType.Name, ref se))
            {
                this.HideSearchPanel();
            }
            else
            {
                // Expand filters
                string tooltipText = "Expand filters";
                FilterArrow.Alt = tooltipText;
                FilterArrow.Attributes.Add("alt", tooltipText);
                FilterArrow.Attributes.Add("title", tooltipText);

                if (TextBoxSearch.Text == SearchHelpers.ConfigureTooltipText(table.EntityType.Name))
                {
                    TextBoxSearch.Text = "";
                }
            }

            if (!IsPostBack)
            {
                // Ordenação da lista
                var displayColumn = table.Attributes.GetAttribute<DisplayColumnAttribute>();
                if (displayColumn != null && displayColumn.SortColumn != null)
                {
                    GridView1.Sort(displayColumn.SortColumn,
                        displayColumn.SortDescending ? SortDirection.Descending : SortDirection.Ascending);
                }
            }
        }

        protected void Label_PreRender(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            DynamicFilter dynamicFilter = (DynamicFilter)label.FindControl("DynamicFilter");
            QueryableFilterUserControl fuc = dynamicFilter.FilterTemplate as QueryableFilterUserControl;
            if (fuc != null && fuc.FilterControl != null)
            {
                label.AssociatedControlID = fuc.FilterControl.GetUniqueIDRelativeTo(label);
            }
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary(GridView1.GetDefaultValues());
            InsertHyperLink.NavigateUrl = table.GetActionPath(PageAction.Insert, routeValues);

            // Watermark
            if (String.IsNullOrEmpty(TextBoxSearch.Text))
            {
                String watermarkText = SearchHelpers.ConfigureTooltipText(table.EntityType.Name);
                TextBoxSearch.Text = watermarkText;
                TextBoxSearch.Attributes.Add("alt", watermarkText);
                TextBoxSearch.Attributes.Add("title", watermarkText);
            }

            base.OnPreRenderComplete(e);
        }

        protected void GridView1_ItemCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == DataControlCommands.EditCommandName)
            //{
            //    _mode = MODE.EDIT;
            //}
            //else
            //{
            //    using (dbContext ent = new dbContext())
            //    {
                    int index;
                    long id = 0;
                    if (!String.IsNullOrEmpty((string)e.CommandArgument))
                    {
                        if (Int32.TryParse((string)e.CommandArgument, out index))
                            id = (long)GridView1.DataKeys[index].Value;
                    }
                    if (e.CommandName == "ShowTrackChanges")
                    {
                        //string tableName = CorrectTableName(table.Name);
                        this.Page.Response.Redirect("~/Track" + table.Name + "/List.aspx?ID=" + id, false);
                    }
            //    }
            //}
        }

        protected void DynamicFilter_FilterChanged(object sender, EventArgs e)
        {
            GridView1.PageIndex = 0;
            string pageSession = "pager_" + Page.AppRelativeVirtualPath.Split('/')[3]; // Entidade
            Session[pageSession] = 0;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GridView1.PageIndex = 0;
            string pageSession = "pager_" + Page.AppRelativeVirtualPath.Split('/')[3]; // Entidade
            Session[pageSession] = 0;

            GridView1.DataBind();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            GridView1.PageIndex = 0;
            string pageSession = "pager_" + Page.AppRelativeVirtualPath.Split('/')[3]; // Entidade
            Session[pageSession] = 0;

            TextBoxSearch.Text = String.Empty;
            Page.ClearTableFilters(table);
            Response.Redirect(table.ListActionPath);
        }

        private void HideSearchPanel()
        {
            SearchContainer.Visible = false;

            TextBoxSearch.Enabled = false;
            btnSearch.Enabled = false;
            btnReset.Enabled = false;
        }

        protected void GridView1_OnRowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            if (e.Exception == null || e.ExceptionHandled)
            {
                Response.Redirect(table.ListActionPath);
            }
            else if (e.Exception != null)
            {
                customValida.ErrorMessage = e.Exception.Message;
                customValida.IsValid = false;
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;
            }
        }

        private bool pageHasTrackChanges()
        {
            string path = this.Context.Request.AppRelativeCurrentExecutionFilePath;
            return path.Contains("Role")
                || path.Contains("HorasFuncionario")
                || path.Contains("FeriasFuncionario")
                || path.Contains("AusenciaFuncionarios");

        }

        protected void GridDataSource_Selecting(object sender, EntityDataSourceSelectingEventArgs e)
        {
            string s = e.SelectArguments.SortExpression;
            if (s.StartsWith("it."))
                e.SelectArguments.SortExpression = s.Substring(3);
        }

        protected void GridView1_OnRowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception == null || e.ExceptionHandled)
            {
                Response.Redirect(table.ListActionPath);
            }
            else if (e.Exception != null)
            {
                customValida.ErrorMessage = e.Exception.Message;
                customValida.IsValid = false;
                e.ExceptionHandled = true;
            }
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                ////////////////////////////////////////////////////////////////
                ///Força a renderização da coluna TrackChanges na ultima coluna da GridView
                ////////////////////////////////////////////////////////////////
                AMRoleProvider gprp = (AMRoleProvider)System.Web.Security.Roles.Provider;
                String userName = Membership.GetUser().UserName;
                GridViewRow row = e.Row;
                List<TableCell> columns = new List<TableCell>();
                _hasTrackChangesPermission = gprp.UserCanTrackChanges(table.Name, userName) && pageHasTrackChanges();

                foreach (DataControlField column in GridView1.Columns)
                {
                    if (column.HeaderText == "Track Changes")
                    {
                        TableCell cell = row.Cells[1];

                        row.Cells.Remove(cell);
                        if (_hasTrackChangesPermission)
                            columns.Add(cell);
                    }
                }

                row.Cells.AddRange(columns.ToArray());
                ////////////////////////////////////////////////////////////////
            }
        }
    }
}
