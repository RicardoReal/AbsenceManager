using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using AbsenceManager.Security;
using static AbsenceManager.MarcacaoFerias;

namespace AbsenceManager
{
    public partial class MarcacaoFerias : System.Web.UI.Page
    {
        public class PresencaFuncionario
        {
            public PresencaFuncionario() { }

            public FuncionarioHorasFuncionario FHF { get; set; }
            public TipoHora TH { get; set; }
        }

        private User user;
        private decimal _Compensacoes;

        private readonly Color CorFeriado = Color.RoyalBlue;
        private readonly Color CorFeriasPendentes = Color.MediumPurple;
        private readonly Color CorFeriasAprovadas = Color.ForestGreen;
        private readonly Color CorFeriasNaoAprovadas = Color.LightCoral;

        private readonly Color CorMenos = Color.Tomato;
        private readonly Color CorMais = Color.GreenYellow;
        private readonly Color CorPresenca = Color.DeepSkyBlue;
        private readonly Color CorAusencia = Color.Red;

        private List<Feriado> feriados;
        private List<FeriasFuncionario> feriasAprovadas;
        private List<FeriasFuncionario> feriasNaoAprovadas;
        private List<FeriasFuncionario> feriasPendentes;
        private List<PresencaFuncionario> presencas;
        private List<AusenciaFuncionario> ausencias;

        public string Compensacoes
        {
            get
            {
                if (_Compensacoes == 0) return "Não existem compensações necessárias.";
                return _Compensacoes > 0
                    ? _Compensacoes + " hora(s) a mais."
                    : Math.Abs(_Compensacoes) + " hora(s) a compensar.";
            }
        }

        public string FeriasPendentes
        {
            get
            {
                return feriasPendentes.Count().ToString();
            }
        }

        public string FeriasAprovadas
        {
            get
            {
                return feriasAprovadas.Count().ToString();
            }
        }

        public string FeriasNaoAprovadas
        {
            get
            {
                return feriasNaoAprovadas.Count().ToString();
            }
        }

        public string FeriasPorMarcar
        {
            get
            {
                int x = user.NrDiasFerias - int.Parse(FeriasAprovadas) - int.Parse(FeriasNaoAprovadas) - SelectedDates.Count() - int.Parse(FeriasPendentes);
                return x.ToString();
            }
        }

        public string FeriasMarcadas
        {
            get
            {
                int x = int.Parse(FeriasAprovadas) + int.Parse(FeriasNaoAprovadas) + SelectedDates.Count() + int.Parse(FeriasPendentes);
                return x.ToString();
            }
        }

        public List<DateTime> SelectedDates
        {
            get
            {
                if (ViewState["Dates"] == null) ViewState["Dates"] = new List<DateTime>();
                return (List<DateTime>)ViewState["Dates"];
            }
            set
            {
                ViewState["Dates"] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!PermissionsManager.HasPageAcess("Marcação Férias")) Context.Response.Redirect("~/NoPermission.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Mapa Presenças";

            feriados = new List<Feriado>();
            feriasNaoAprovadas = new List<FeriasFuncionario>();
            feriasPendentes = new List<FeriasFuncionario>();

            using (AM_Entities ent = new AM_Entities())
            {
                user = GeneralHelpers.GetCurrentUser(ent);

                _Compensacoes = user.Compensacoes;

                foreach (Feriado feriado in ent.Feriadoes)
                    feriados.Add(feriado);

                feriasAprovadas = ent.FeriasFuncionarios.Where(f => f.UserID == user.ID).ToList();

                feriasPendentes = feriasAprovadas.Where(f => f.Pendente == true).ToList();

                feriasNaoAprovadas = feriasAprovadas.Where(f => f.Aprovado == false && f.Pendente == false).ToList();

                feriasAprovadas = feriasAprovadas.Where(f => f.Aprovado == true && f.Pendente == false).ToList();

                presencas = (from fhf in ent.FuncionarioHorasFuncionarios
                             join th in ent.TipoHoras on fhf.TipoHorasID equals th.ID
                             where fhf.UserID == user.ID
                             select new PresencaFuncionario { FHF = fhf, TH = th }
                             ).ToList();

                ausencias = ent.AusenciaFuncionarios.Where(a => a.UserID == user.ID).ToList();


                if (!IsPostBack)
                {
                    JanCalendar.VisibleDate = new DateTime(JanCalendar.TodaysDate.Year, 1, 1);
                    FebCalendar.VisibleDate = new DateTime(FebCalendar.TodaysDate.Year, 2, 1);
                    MarCalendar.VisibleDate = new DateTime(MarCalendar.TodaysDate.Year, 3, 1);
                    AprCalendar.VisibleDate = new DateTime(AprCalendar.TodaysDate.Year, 4, 1);

                    MayCalendar.VisibleDate = new DateTime(JanCalendar.TodaysDate.Year, 5, 1);
                    JunCalendar.VisibleDate = new DateTime(FebCalendar.TodaysDate.Year, 6, 1);
                    JulCalendar.VisibleDate = new DateTime(MarCalendar.TodaysDate.Year, 7, 1);
                    AugCalendar.VisibleDate = new DateTime(AprCalendar.TodaysDate.Year, 8, 1);

                    SepCalendar.VisibleDate = new DateTime(JanCalendar.TodaysDate.Year, 9, 1);
                    OctCalendar.VisibleDate = new DateTime(FebCalendar.TodaysDate.Year, 10, 1);
                    NovCalendar.VisibleDate = new DateTime(MarCalendar.TodaysDate.Year, 11, 1);
                    DezCalendar.VisibleDate = new DateTime(AprCalendar.TodaysDate.Year, 12, 1);

                    Jan_Calendar.VisibleDate = new DateTime(JanCalendar.TodaysDate.Year + 1, 1, 1);
                    Feb_Calendar.VisibleDate = new DateTime(FebCalendar.TodaysDate.Year + 1, 2, 1);
                    Mar_Calendar.VisibleDate = new DateTime(MarCalendar.TodaysDate.Year + 1, 3, 1);
                    Apr_Calendar.VisibleDate = new DateTime(AprCalendar.TodaysDate.Year + 1, 4, 1);


                    List<object> tipoAusencias = (from ta in ent.TipoAusencias select new { ta.ID, ta.Descricao }).ToList<object>();
                    tipoAusencias.Add(new { ID = "-1", Descricao = "Férias" });

                    TipoAusenciasDDL.DataSource = tipoAusencias;
                    //TipoAusenciasDDL.DataSource = from ta in ent.TipoAusencias select new { ta.ID, ta.Descricao };
                    TipoAusenciasDDL.DataValueField = "ID";
                    TipoAusenciasDDL.DataTextField = "Descricao";
                    TipoAusenciasDDL.DataBind();
                }
            }
        }

        protected void Calendar_PreRender(object sender, EventArgs e)
        {
            Calendar c = sender as Calendar;
            c.SelectedDates.Clear();
            int month;
            if (c.ID.StartsWith("Jan")) month = 1;
            else if (c.ID.StartsWith("Feb")) month = 2;
            else if (c.ID.StartsWith("Mar")) month = 3;
            else if (c.ID.StartsWith("Apr")) month = 4;
            else if (c.ID.StartsWith("May")) month = 5;
            else if (c.ID.StartsWith("Jun")) month = 6;
            else if (c.ID.StartsWith("Jul")) month = 7;
            else if (c.ID.StartsWith("Aug")) month = 8;
            else if (c.ID.StartsWith("Sep")) month = 9;
            else if (c.ID.StartsWith("Oct")) month = 10;
            else if (c.ID.StartsWith("Nov")) month = 11;
            else month = 12;

            foreach (DateTime dt in SelectedDates.Where(x => x.Month == month))
                c.SelectedDates.Add(dt);
        }

        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            //Check if selected Date is in the saved list 
            // Remove the Selected Date from the saved list 
            Calendar c = sender as Calendar;

            List<DateTime> feriasNaprov = (from f in feriasNaoAprovadas select f.DataFerias).ToList();
            List<DateTime> feriasPend = (from f in feriasPendentes select f.DataFerias).ToList();
            if (feriasNaprov.Contains(c.SelectedDate))
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    FeriasFuncionario ff = (from f in ent.FeriasFuncionarios where f.DataFerias == c.SelectedDate select f).FirstOrDefault();
                    ent.FeriasFuncionarios.DeleteObject(ff);
                    feriasNaoAprovadas.Remove(feriasNaoAprovadas.Where(d => d.DataFerias == c.SelectedDate).FirstOrDefault());
                    ent.SaveChanges();
                    Response.Redirect(Request.RawUrl);
                }
            }
            else if (feriasPend.Contains(c.SelectedDate))
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    FeriasFuncionario ff = (from f in ent.FeriasFuncionarios where f.DataFerias == c.SelectedDate select f).FirstOrDefault();
                    ent.FeriasFuncionarios.DeleteObject(ff);
                    feriasPendentes.Remove(feriasPendentes.Where(d => d.DataFerias == c.SelectedDate).FirstOrDefault());
                    ent.SaveChanges();
                    Response.Redirect(Request.RawUrl);
                }
            }
            else if (SelectedDates.Contains(c.SelectedDate))
                SelectedDates.Remove(c.SelectedDate);
            else
                SelectedDates.Add(c.SelectedDate);

            ViewState["Dates"] = SelectedDates;
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                User u = GeneralHelpers.GetCurrentUser(ent);

                if (TipoAusenciasDDL.SelectedValue == "-1")
                {
                    
                    List<DateTime> existingVacations = (from ev in ent.FeriasFuncionarios
                                                        where ev.UserID == u.ID
                                                        select ev.DataFerias).ToList();

                    foreach (DateTime dt in SelectedDates.Except(existingVacations))
                    {
                        FeriasFuncionario ff = new FeriasFuncionario();
                        ff.Ano = dt.Year;
                        ff.Mes = dt.Month;
                        ff.Dia = dt.Day;
                        ff.DataFerias = dt;
                        ff.NrHoras = 8;
                        ff.UserID = u.ID;
                        ff.Pendente = true;

                        ent.FeriasFuncionarios.AddObject(ff);
                    }

                    foreach (DateTime dt in existingVacations.Except(SelectedDates))
                    {
                        FeriasFuncionario deleteDt = (from dtm in ent.FeriasFuncionarios
                                                      where dtm.DataFerias == dt
                                                      && dtm.UserID == u.ID
                                                      select dtm).FirstOrDefault();

                        ent.FeriasFuncionarios.Detach(deleteDt);
                    }
                    
                }
                else
                {
                    foreach (DateTime dt in SelectedDates)
                    {
                        AusenciaFuncionario af = new AusenciaFuncionario();
                        af.Ano = dt.Year;
                        af.DataAusencia = dt;
                        af.Dia = dt.Day;
                        af.Mes = dt.Month;
                        af.NrHoras = 8;
                        af.TipoAusenciaID = Convert.ToInt64(TipoAusenciasDDL.SelectedValue);
                        af.UserID = u.ID;

                        ent.AusenciaFuncionarios.AddObject(af);
                    }
                }
                ent.SaveChanges();
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void DayRender(object sender, DayRenderEventArgs e)
        {
            Feriado feriado = feriados.Where(f => f.FeriadoData == e.Day.Date).FirstOrDefault();
            FeriasFuncionario diaFeriasPendentes = feriasPendentes.Where(x => x.DataFerias == e.Day.Date).FirstOrDefault();
            FeriasFuncionario diaFeriasAprovadas = feriasAprovadas.Where(x => x.DataFerias == e.Day.Date).FirstOrDefault();
            FeriasFuncionario diaFeriasNaoAprovadas = feriasNaoAprovadas.Where(x => x.DataFerias == e.Day.Date).FirstOrDefault();
            List<PresencaFuncionario> presenca = presencas.Where(x => x.FHF.Data == e.Day.Date).ToList();
            AusenciaFuncionario ausencia = ausencias.Where(x => x.DataAusencia == e.Day.Date).FirstOrDefault();

            DateTime? selected = SelectedDates.Where(x => x.Year == e.Day.Date.Year && x.Month == e.Day.Date.Month && x.Day == e.Day.Date.Day).FirstOrDefault();

            if (presenca.Count != 0)
            {
                decimal sumHoras = 0;
                foreach (PresencaFuncionario pf in presenca)
                {
                    e.Cell.ToolTip += pf.FHF.NrHoras.ToString() + " Horas " + pf.TH.Descricao + Environment.NewLine;
                    sumHoras += pf.FHF.NrHoras;
                }

                if (sumHoras == 8) e.Cell.BackColor = CorPresenca;
                else if (sumHoras < 8) e.Cell.BackColor = CorMenos;
                else e.Cell.BackColor = CorMais;
                e.Day.IsSelectable = false;
            }
            else if (ausencia != null)
            {
                e.Cell.ToolTip = ausencia.NrHoras.ToString() + " Horas Ausente";
                e.Cell.BackColor = CorAusencia;
                e.Day.IsSelectable = false;
            }
            else if (feriado != null)
            {
                e.Cell.ToolTip = feriado.Descricao;
                e.Cell.BackColor = CorFeriado;
                e.Day.IsSelectable = false;
            }
            else if (diaFeriasPendentes != null)
            {
                e.Cell.ToolTip = "Pendente";
                e.Cell.BackColor = CorFeriasPendentes;
                e.Day.IsSelectable = true;
            }
            else if (diaFeriasAprovadas != null)
            {
                e.Cell.ToolTip = "Aprovado";
                e.Cell.BackColor = CorFeriasAprovadas;
                e.Day.IsSelectable = false;
            }
            else if (diaFeriasNaoAprovadas != null)
            {
                e.Cell.ToolTip = "Não Aprovado";
                e.Cell.BackColor = CorFeriasNaoAprovadas;
                e.Day.IsSelectable = true;
            }
            else if (selected != new DateTime(1, 1, 1))
            {
                e.Cell.ToolTip = "";
                e.Cell.BackColor = Color.Black; e.Day.IsSelectable = true;
            }
            else if (e.Day.IsWeekend)
                e.Day.IsSelectable = false;

            if (int.Parse(FeriasPorMarcar) == 0 && !(selected != new DateTime(1, 1, 1)) && diaFeriasNaoAprovadas == null && diaFeriasPendentes == null) e.Day.IsSelectable = false;
        }
    }
}