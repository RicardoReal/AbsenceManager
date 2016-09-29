using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using AbsenceManager.Security;

namespace AbsenceManager
{
    public partial class MapaGeralFerias : System.Web.UI.Page
    {
        private readonly Color CorFeriado = Color.RoyalBlue;
        private readonly Color CorFeriasAprovadas = Color.ForestGreen;
        private readonly Color CorFeriasPendentes = Color.Gold;

        private List<Feriado> feriados;
        private List<FeriasFuncionario> feriasAprovadas;
        private List<FeriasFuncionario> feriasPendentes;

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
                return feriasPendentes.Count().ToString();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!PermissionsManager.HasPageAcess("Mapa Geral Férias")) Context.Response.Redirect("~/NoPermission.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Mapa Geral Férias";

            feriados = new List<Feriado>();

            using (AM_Entities ent = new AM_Entities())
            {
                foreach (Feriado feriado in ent.Feriadoes)
                    feriados.Add(feriado);

                feriasAprovadas = ent.FeriasFuncionarios.Where(f => f.Aprovado == true && f.Pendente == false && (f.Ano == DateTime.Now.Year || f.Ano == DateTime.Now.Year + 1)).ToList();

                feriasPendentes = ent.FeriasFuncionarios.Where(f => f.Pendente == true && (f.Ano == DateTime.Now.Year || f.Ano == DateTime.Now.Year + 1)).ToList();
            }

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
            }
        }

        protected void DayRender(object sender, DayRenderEventArgs e)
        {
            Feriado feriado = feriados.Where(f => f.FeriadoData == e.Day.Date).FirstOrDefault();
            var diaFeriasAprovadas = feriasAprovadas.Where(x => x.DataFerias == e.Day.Date);
            var diaFeriasPendentes = feriasPendentes.Where(x => x.DataFerias == e.Day.Date);

            if (feriado != null)
            {
                e.Cell.ToolTip = feriado.Descricao;
                e.Cell.BackColor = CorFeriado;
            }
            else if (diaFeriasAprovadas.Count() != 0 || diaFeriasPendentes.Count() != 0)
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    string name = string.Empty;

                    foreach (FeriasFuncionario feriasf in diaFeriasAprovadas)
                    {
                        name = ent.Users.Where(u => u.ID == feriasf.UserID).FirstOrDefault().Nome;
                        e.Cell.ToolTip += name + "\n";
                        e.Cell.BackColor = CorFeriasAprovadas;
                    }

                    foreach (FeriasFuncionario feriasf in diaFeriasPendentes)
                    {
                        name = ent.Users.Where(u => u.ID == feriasf.UserID).FirstOrDefault().Nome;
                        e.Cell.ToolTip += name + " - Por Aprovar\n";
                        e.Cell.BackColor = CorFeriasPendentes;
                    }
                }
            }
            else if (e.Day.IsWeekend)
                e.Day.IsSelectable = false;
        }
    }
}