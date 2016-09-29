using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Expressions;
using AbsenceManager.Security;
using System.Web.Security;
using System.Text;
using System.Collections;
using System.Data;
using System.Collections.Generic;

namespace AbsenceManager.DynamicData.CustomPages.Users
{
    public partial class List : System.Web.UI.Page
    {
        protected MetaTable table;

        private bool _hasOnScreenPermission;
        private bool _hasTrackChangesPermission;

        protected bool HasTrackChangesPermission
        {
            get { return _hasTrackChangesPermission; }
        }

        protected bool HasOnScreenPermission
        {
            get { return _hasOnScreenPermission; }
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

            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
                sm.RegisterPostBackControl(lnkExport);

            AMRoleProvider gprp = (AMRoleProvider)System.Web.Security.Roles.Provider;
            String userName = Membership.GetUser().UserName;

            if (this.Session["EmpresaId"] != null)
            {
                FilterRepeaterContainer.Visible = false;
                toggleFilters.Visible = false;
            }

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
            {
                InsertHyperLink.Visible = false;
                ButtonContainer.Visible = false;
                _hasOnScreenPermission = false;
            }

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

                // Criação de lista de meses
                MonthList.DataSource = CreateMonthDataSource();
                MonthList.DataTextField = "MonthTextField";
                MonthList.DataValueField = "MonthValueField";

                MonthList.DataBind();

                MonthList.SelectedIndex = DateTime.Now.Month - 1;


                //Criação de lista de Departamentos
                DeptList.DataSource = CreateDeptDataSource();
                DeptList.DataTextField = "DeptTextField";
                DeptList.DataValueField = "DeptValueField";

                DeptList.DataBind();

                DeptList.SelectedIndex = 0;
            }
        }

        private ICollection CreateMonthDataSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("MonthTextField", typeof(string)));
            dt.Columns.Add(new DataColumn("monthValueField", typeof(long)));

            dt.Rows.Add(CreateRow("Janeiro", 1, dt));
            dt.Rows.Add(CreateRow("Fevereiro", 2, dt));
            dt.Rows.Add(CreateRow("Março", 3, dt));
            dt.Rows.Add(CreateRow("Abril", 4, dt));
            dt.Rows.Add(CreateRow("Maio", 5, dt));
            dt.Rows.Add(CreateRow("Junho", 6, dt));
            dt.Rows.Add(CreateRow("Julho", 7, dt));
            dt.Rows.Add(CreateRow("Agosto", 8, dt));
            dt.Rows.Add(CreateRow("Setembro", 9, dt));
            dt.Rows.Add(CreateRow("Outubro", 10, dt));
            dt.Rows.Add(CreateRow("Novembro", 11, dt));
            dt.Rows.Add(CreateRow("Dezembro", 12, dt));

            DataView dv = new DataView(dt);
            return dv;
        }

        private ICollection CreateDeptDataSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("DeptTextField", typeof(string)));
            dt.Columns.Add(new DataColumn("DeptValueField", typeof(long)));

            dt.Rows.Add(CreateRow("Todos", 0, dt));

            using (AM_Entities ent = new AM_Entities())
            {
                foreach (Departamento dept in ent.Departamentoes.ToList())
                    dt.Rows.Add(CreateRow(dept.Nome, dept.ID, dt));
            }

            DataView dv = new DataView(dt);
            return dv;
        }

        private DataRow CreateRow(String Text, long Value, DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr[0] = Text;
            dr[1] = Value;

            return dr;
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
            return true;
        }

        protected void GridDataSource_Selecting(object sender, EntityDataSourceSelectingEventArgs e)
        {
            string s = e.SelectArguments.SortExpression;
            if (s.StartsWith("it."))
                e.SelectArguments.SortExpression = s.Substring(3);
        }

        protected void GridView1_ItemCommand(object sender, GridViewCommandEventArgs e)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                int index;
                long id = 0;
                if (!String.IsNullOrEmpty((string)e.CommandArgument))
                {
                    if (Int32.TryParse((string)e.CommandArgument, out index))
                        id = (long)GridView1.DataKeys[index].Value;
                }
                if (e.CommandName == "ShowMapaPresencas")
                {
                    this.Page.Response.Redirect("~/MapaPresencas.aspx?UserID=" + id, false);
                }
                else if (e.CommandName == "ShowTrackChanges")
                {
                    //string tableName = CorrectTableName(table.Name);
                    this.Page.Response.Redirect("~/Track" + table.Name + "/List.aspx?ID=" + id, false);
                }
                else if (e.CommandName == "ShowAprovacaoFerias")
                {
                    if (PermissionsManager.UserIsAdmin())
                    {
                        this.Page.Response.Redirect("~/AprovacaoFerias.aspx?UserID=" + id, false);
                        return;
                    }

                    User user = GeneralHelpers.GetCurrentUser(ent);
                    long? responsavelID = (from u in ent.Users
                                           join d in ent.Departamentoes on u.DepartamentoID equals d.ID
                                           where u.ID == id
                                           select d.EncarregadoID).FirstOrDefault();

                    if (responsavelID == null || responsavelID == user.ID)
                    {
                        this.Page.Response.Redirect("~/AprovacaoFerias.aspx?UserID=" + id, false);
                        return;
                    }
                    else GeneralHelpers.ShowJavascriptAlert("Sem permissões para aprovar férias deste funcionário.");
                }
            }
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            StringBuilder output = new StringBuilder();
            // Headers
            output.AppendLine("Nr.Funcionário;Nome;Empresa;Departamento;21;22;23;24;25;26;27;28;29;30;31;1;2;3;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;Total de Horas;Sub Almoço");

            using (AM_Entities ent = new AM_Entities())
            {
                // Desde o dia 21 mês anterior até 20 do mês corrente
                int month = int.Parse(MonthList.SelectedValue);
                int deptID = int.Parse(DeptList.SelectedValue);

                DateTime startDate = new DateTime(month - 1 <= 0 ? DateTime.Now.Year - 1 : DateTime.Now.Year, month - 1 <= 0 ? 12 : month - 1, 21);
                DateTime endDate = new DateTime(DateTime.Now.Year, month, 20);

                // Obter os ids dos funcionários que trabalharam este mês
                var userIds = (from funchoras in ent.FuncionarioHorasFuncionarios
                               join horas in ent.HorasFuncionarios on funchoras.HorasID equals horas.ID
                               where funchoras.Data >= startDate && funchoras.Data <= endDate
                               && (deptID == 0 || horas.DepartamentoID == deptID)
                               group funchoras by funchoras.UserID into ids
                               select ids.Key).ToList();


                foreach (var id in userIds)
                {
                    // Obter lista de horas para cada funcionário
                    var horasList = (from funchoras in ent.FuncionarioHorasFuncionarios
                                     join horas in ent.HorasFuncionarios on funchoras.HorasID equals horas.ID
                                     join user in ent.Users on funchoras.UserID equals user.ID
                                     join empresa in ent.Empresas on user.EmpresaID equals empresa.ID
                                     join departamento in ent.Departamentoes on horas.DepartamentoID equals departamento.ID
                                     where user.ID == id
                                     && funchoras.Data >= startDate && funchoras.Data <= endDate
                                     select new RegHoras
                                     {
                                         NrFunc = user.NrFuncionario,
                                         Nome = user.Nome,
                                         NomeEmpresa = empresa.Nome,
                                         Dept = departamento.Nome,
                                         Dia = funchoras.Data.Day,
                                         NrHoras = funchoras.NrHoras
                                     }).ToList();

                    string deptName = deptID == 0 ? "" : ent.Departamentoes.Where(d => d.ID == deptID).FirstOrDefault().Nome;
                    deptName = deptName == "" ? horasList.First().Dept : deptName;
                    string funcInfo = horasList.First().NrFunc + ";" + horasList.First().Nome + ";" + horasList.First().NomeEmpresa + ";" + deptName + ";";
                    string sHoras = "";

                    RegHoras tmp;
                    decimal totalHoras = 0, subAlmoco = 0;

                    // Processa horas de dias 21 a 31 do mês passado
                    for (int i = 21; i <= 31; i++)
                    {
                        tmp = horasList.Where(x => x.Dia == i).FirstOrDefault();

                        sHoras += tmp == null ? " " : tmp.NrHoras.ToString();
                        sHoras += ";";

                        totalHoras += tmp == null ? 0 : tmp.NrHoras;
                        subAlmoco += tmp == null ? 0 : tmp.NrHoras > 4 ? 1 : 0;
                    }

                    // Processa horas de dias 1 a 20 do mês corrente
                    for (int i = 1; i <= 20; i++)
                    {
                        tmp = horasList.Where(x => x.Dia == i).FirstOrDefault();

                        sHoras += tmp == null ? " " : tmp.NrHoras.ToString();
                        sHoras += ";";

                        totalHoras += tmp == null ? 0 : tmp.NrHoras;
                        subAlmoco += tmp == null ? 0 : tmp.NrHoras > 4 ? 1 : 0;
                    }

                    output.AppendLine(funcInfo + sHoras + totalHoras + ";" + subAlmoco);
                }
            }

            string attachment = "attachment; filename=" + "ListagemFuncionários.csv";
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "text/csv";
            Response.AddHeader("Pragma", "public");

            Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");

            Response.Write(output);
            Response.End();
        }

        private class RegHoras
        {
            public string NrFunc { get; set; }
            public string Nome { get; set; }
            public string NomeEmpresa { get; set; }
            public string Dept { get; set; }
            public int Dia { get; set; }
            public decimal NrHoras { get; set; }
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
                        TableCell cell = row.Cells[2];

                        row.Cells.Remove(cell);
                        if (_hasTrackChangesPermission)
                            columns.Add(cell);
                    }
                }

                row.Cells.AddRange(columns.ToArray());
                ////////////////////////////////////////////////////////////////
            }
        }

        protected void FilterByEmpresa(object sender, CustomExpressionEventArgs e)
        {
            if (this.Session["EmpresaId"] != null)
            {
                long EmpresaID = (long)this.Session["EmpresaId"];

                e.Query = from u in e.Query.Cast<User>()
                          where u.EmpresaID == EmpresaID
                          select u;
            }
        }
    }
}
