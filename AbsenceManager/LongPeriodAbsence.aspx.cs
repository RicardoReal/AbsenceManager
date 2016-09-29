using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Expressions;
using AbsenceManager.Security;
using System.Linq;
using System.Collections;
using System.Data;
using System.Collections.Generic;

namespace AbsenceManager
{
    public partial class LongPeriodAbsence : System.Web.UI.Page
    {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e)
        {
            table = DynamicDataRouteHandler.GetRequestMetaTable(Context);
            //FormView1.SetMetaTable(table, table.GetColumnValuesFromRoute(Context));
            //DetailsDataSource.EntityTypeFilter = table.EntityType.Name;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Período de Ausências Prolongada";
            if (!IsPostBack)
            {
                ViewState["PreviousPageURL"] = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();

                using(AM_Entities ent = new AM_Entities())
                {
                    DropDownFuncionarios.DataSource = CreateUsersDataSource(ent); ;
                    DropDownFuncionarios.DataTextField = "UsersTextField";
                    DropDownFuncionarios.DataValueField = "UsersValueField";
                    DropDownFuncionarios.DataBind();
                    DropDownFuncionarios.SelectedIndex = 0;

                    DropDownTipoAusência.DataSource = CreateTipoAusenciaDataSource(ent);
                    DropDownTipoAusência.DataTextField = "TipoAusTextField";
                    DropDownTipoAusência.DataValueField = "TipoAusValueField";
                    DropDownTipoAusência.DataBind();
                    DropDownTipoAusência.SelectedIndex = 0;
                }
            }
        }

        private ICollection CreateUsersDataSource(AM_Entities ent)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("UsersTextField", typeof(string)));
            dt.Columns.Add(new DataColumn("UsersValueField", typeof(long)));

            foreach (User u in ent.Users.Where(x => x.Activo == true).OrderBy(x => x.Nome).ToList())
                dt.Rows.Add(CreateRow(u.Nome, u.ID, dt));

            DataView dv = new DataView(dt);
            return dv;
        }

        private ICollection CreateTipoAusenciaDataSource(AM_Entities ent)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("TipoAusTextField", typeof(string)));
            dt.Columns.Add(new DataColumn("TipoAusValueField", typeof(long)));

            dt.Rows.Add(CreateRow("Férias", -1, dt));

            foreach (TipoAusencia ta in ent.TipoAusencias.ToList())
                dt.Rows.Add(CreateRow(ta.Descricao, ta.ID, dt));

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

        protected void Insert_OnClick(object sender, EventArgs e)
        {
            try
            {
                DateTime init, end;
                long userID = long.Parse(DropDownFuncionarios.SelectedValue);
                long tipoAusência = long.Parse(DropDownTipoAusência.SelectedValue);

                if (!DateTime.TryParse(TextBoxDataInicio.Text, out init))
                {
                    GeneralHelpers.ShowJavascriptAlert("Data de início inválida.");
                    return;
                }
                if (!DateTime.TryParse(TextBoxDataFim.Text, out end))
                {
                    GeneralHelpers.ShowJavascriptAlert("Data de fim inválida.");
                    return;
                }
                if (init > end)
                {
                    GeneralHelpers.ShowJavascriptAlert("Data de início posterior a data de fim.");
                    return;
                }

                using (AM_Entities ent = new AM_Entities())
                {
                    List<Feriado> feriados = ent.Feriadoes.ToList();
                    while (init <= end)
                    {
                        if (init.DayOfWeek != DayOfWeek.Saturday && init.DayOfWeek != DayOfWeek.Sunday
                            && feriados.Where(f => f.FeriadoData == init).Count() == 0)
                        {
                            if (tipoAusência == -1)
                            {
                                FeriasFuncionario ff = new FeriasFuncionario();
                                ff.DataFerias = init;
                                ff.Aprovado = true;
                                ff.Pendente = false;
                                ff.NrHoras = 8;
                                ff.UserID = userID;
                                ent.FeriasFuncionarios.AddObject(ff);
                            }
                            else
                            {
                                AusenciaFuncionario af = new AusenciaFuncionario();
                                af.DataAusencia = init;
                                af.NrHoras = 8;
                                af.TipoAusenciaID = tipoAusência;
                                af.UserID = userID;
                                ent.AusenciaFuncionarios.AddObject(af);
                            }
                        }
                        init = init.AddDays(1);
                    }
                    ent.SaveChanges();
                    GeneralHelpers.ShowJavascriptAlert("Ausência Gravada com Sucesso!");

                    string url = ViewState["PreviousPageURL"].ToString();
                    if (url == "" || url.Contains("ReturnUrl=")) url = "~/Default.aspx";
                    Response.Redirect(url);
                }
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "AusenciasProlongadas");
            }
        }

        protected void Cancel_OnClick(object sender, EventArgs e)
        {
            string url = ViewState["PreviousPageURL"].ToString();
            if (url == "" || url.Contains("ReturnUrl=")) url = "~/Default.aspx";
            Response.Redirect(url);
        }

    }
}
