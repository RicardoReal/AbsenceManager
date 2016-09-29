using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using System.Data;
using AbsenceManager.Security;

namespace AbsenceManager
{
    public partial class MapaPresencas : System.Web.UI.Page
    {
        private User user;
        private decimal _Compensacoes;
        private readonly Color CorFeriado = Color.RoyalBlue;
        private readonly Color CorMenos = Color.Tomato;
        private readonly Color CorMais = Color.LimeGreen;
        private readonly Color CorNormal = Color.DeepSkyBlue;
        private readonly Color CorHorasExtra = Color.Gold;
        private readonly Color CorAusencia = Color.Red;
        private readonly Color CorFerias = Color.ForestGreen;

        private static List<Feriado> feriados;
        private static List<FuncionarioHorasFuncionario> horasMes;
        private static List<FuncionarioHorasFuncionario> horasExtraMes;
        private static List<AusenciaFuncionario> ausencias;
        private static List<FeriasFuncionario> ferias;

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

        public string NomeFuncionario
        {
            get { return user.Nome; }
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
            if (!PermissionsManager.HasPageAcess("Mapa Presenças")) Context.Response.Redirect("~/NoPermission.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            feriados = new List<Feriado>();

            using (AM_Entities ent = new AM_Entities())
            {
                if (Request.QueryString.Count == 0)
                    user = GeneralHelpers.GetCurrentUser(ent);
                else
                {
                    long userID = long.Parse(Request.QueryString.GetValues(0)[0]);
                    user = (from u in ent.Users where u.ID == userID select u).FirstOrDefault();
                }

                Title = "Mapa de Presenças";

                _Compensacoes = user.Compensacoes;

                foreach (Feriado feriado in ent.Feriadoes)
                    feriados.Add(feriado);

                if (!IsPostBack)
                {
                    MonthCalendar.VisibleDate = new DateTime(MonthCalendar.TodaysDate.Year, MonthCalendar.TodaysDate.Month, 1);
                    DateTime startDate = MonthCalendar.VisibleDate.AddMonths(-1);
                    DateTime endDate = MonthCalendar.VisibleDate.AddMonths(1);


                    horasMes = new List<FuncionarioHorasFuncionario>();
                    horasMes = (from hf in ent.FuncionarioHorasFuncionarios
                                where
                                    hf.UserID == user.ID
                                    && hf.Data.Year == MonthCalendar.TodaysDate.Year
                                    && (hf.Data >= startDate
                                        || hf.Data <= endDate)
                                select hf).ToList();

                    horasExtraMes = new List<FuncionarioHorasFuncionario>();
                    horasExtraMes = (from hf in ent.FuncionarioHorasFuncionarios
                                     join th in ent.TipoHoras on hf.TipoHorasID equals th.ID
                                     where
                                        th.HorasExtra == true
                                        && hf.UserID == user.ID
                                         && (hf.Data >= startDate
                                        || hf.Data <= endDate)
                                     select hf).ToList();

                    ausencias = new List<AusenciaFuncionario>();
                    ausencias = (from au in ent.AusenciaFuncionarios
                                 where
                                    au.UserID == user.ID
                                    && (au.DataAusencia >= startDate
                                        || au.DataAusencia <= endDate)
                                 select au).ToList();

                    ferias = new List<FeriasFuncionario>();
                    ferias = (from ff in ent.FeriasFuncionarios
                              where
                                    ff.UserID == user.ID
                                    && (ff.DataFerias >= startDate
                                        || ff.DataFerias <= endDate)
                              select ff).ToList();

                    // Tipo de Presença
                    TipoMarcacaoList.DataSource = CreateTipoMarcacaoDataSource(ent);
                    TipoMarcacaoList.DataTextField = "TipoMarcTextField";
                    TipoMarcacaoList.DataValueField = "TipoMarcValueField";

                    TipoMarcacaoList.DataBind();

                    TipoMarcacaoList.SelectedIndex = 0;
                }
            }
        }

        private ICollection CreateTipoMarcacaoDataSource(AM_Entities ent)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("TipoMarcTextField", typeof(string)));
            dt.Columns.Add(new DataColumn("TipoMarcValueField", typeof(long)));

            dt.Rows.Add(CreateRow("Presença", -1, dt));
            dt.Rows.Add(CreateRow("Férias", -2, dt));

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

        protected void Calendar_PreRender(object sender, EventArgs e)
        {
            Calendar c = sender as Calendar;
            c.SelectedDates.Clear();

            foreach (DateTime dt in SelectedDates.Where(x => x.Month == MonthCalendar.TodaysDate.Month))
                c.SelectedDates.Add(dt);
        }

        protected void DayRender(object sender, DayRenderEventArgs e)
        {
            long userID = long.Parse(Request.QueryString.GetValues(0)[0]);

            Feriado feriado = feriados.Where(f => f.FeriadoData == e.Day.Date.Date).FirstOrDefault();
            FuncionarioHorasFuncionario horaFunc = horasMes.Where(f => f.Data == e.Day.Date).FirstOrDefault();
            FuncionarioHorasFuncionario horaExtraFunc = horasExtraMes.Where(f => f.Data == e.Day.Date.Date).FirstOrDefault();
            AusenciaFuncionario ausencia = ausencias.Where(a => a.DataAusencia == e.Day.Date.Date).FirstOrDefault();
            FeriasFuncionario feria = ferias.Where(a => a.DataFerias == e.Day.Date.Date).FirstOrDefault();

            if (feriado != null)
            {
                e.Cell.ToolTip = feriado.Descricao;
                e.Cell.BackColor = CorFeriado;
            }
            else if (horaFunc != null)
            {
                e.Cell.ToolTip = horaFunc.NrHoras.ToString() + " Horas Trabalhadas";
                if (horaFunc.NrHoras == 8) e.Cell.BackColor = CorNormal;
                else if (horaFunc.NrHoras < 8) e.Cell.BackColor = CorMenos;
                else e.Cell.BackColor = CorMais;

            }
            else if (ausencia != null)
            {
                string descTipoAus;
                using (AM_Entities ent = new AM_Entities())
                {
                    descTipoAus = ent.TipoAusencias.Where(x => x.ID == ausencia.TipoAusenciaID).FirstOrDefault().Descricao;
                }
                e.Cell.ToolTip = descTipoAus;
                e.Cell.BackColor = CorAusencia;
            }
            else if (feria != null)
            {
                e.Cell.ToolTip = "Férias";
                e.Cell.BackColor = CorFerias;
            }
            

            if (horaExtraFunc != null)
            {
                e.Cell.ToolTip += Environment.NewLine + horaExtraFunc.NrHoras.ToString() + " Horas Extra Trabalhadas";
                e.Cell.BackColor = CorHorasExtra;
            }

            e.Day.IsSelectable = false;
        }

        protected void MonthCalendar_MonthChanged(object sender, MonthChangedEventArgs e)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                DateTime startDate = MonthCalendar.VisibleDate.AddMonths(-1);
                DateTime endDate = MonthCalendar.VisibleDate.AddMonths(1);


                horasMes = new List<FuncionarioHorasFuncionario>();
                horasMes = (from hf in ent.FuncionarioHorasFuncionarios
                            where
                                hf.UserID == user.ID
                                && hf.Data.Year == MonthCalendar.TodaysDate.Year
                                && (hf.Data >= startDate
                                    || hf.Data <= endDate)
                            select hf).ToList();

                horasExtraMes = new List<FuncionarioHorasFuncionario>();
                horasExtraMes = (from hf in ent.FuncionarioHorasFuncionarios
                                 join th in ent.TipoHoras on hf.TipoHorasID equals th.ID
                                 where
                                    th.HorasExtra == true
                                    && hf.UserID == user.ID
                                     && (hf.Data >= startDate
                                    || hf.Data <= endDate)
                                 select hf).ToList();

                ausencias = new List<AusenciaFuncionario>();
                ausencias = (from au in ent.AusenciaFuncionarios
                             where
                                au.UserID == user.ID
                                && (au.DataAusencia >= startDate
                                    || au.DataAusencia <= endDate)
                             select au).ToList();

                ferias = new List<FeriasFuncionario>();
                ferias = (from ff in ent.FeriasFuncionarios
                          where
                                ff.UserID == user.ID
                                && (ff.DataFerias >= startDate
                                    || ff.DataFerias <= endDate)
                          select ff).ToList();
            }
        }

        protected void Button_OnClick(object sender, EventArgs e)
        {
            DateTime dt;
            if (DateTime.TryParse(TextBoxDate.Text, out dt))
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    long id = long.Parse(TipoMarcacaoList.SelectedValue);
                    if (id == -1)
                    {
                        FuncionarioHorasFuncionario hf = new FuncionarioHorasFuncionario();
                        hf.Data = dt.Date;
                        hf.UserID = user.ID;
                        hf.NrHoras = 8;
                        hf.HorasID = ent.HorasFuncionarios.Where(x => x.Data == dt.Date).FirstOrDefault().ID;
                        hf.TipoHorasID = ent.TipoHoras.Where(x => x.Descricao.Contains("Norma")).FirstOrDefault().ID;
                        ent.FuncionarioHorasFuncionarios.AddObject(hf);

                        // REMOVER FERIAS
                        var ferias = ent.FeriasFuncionarios.Where(f => f.UserID == user.ID && f.DataFerias == dt.Date);
                        foreach (var diaferias in ferias) ent.FeriasFuncionarios.DeleteObject(diaferias);

                        // REMOVER AUSENCIA
                        var auss = ent.AusenciaFuncionarios.Where(a => a.UserID == user.ID && a.DataAusencia == dt.Date);
                        foreach (var aus in auss) ent.AusenciaFuncionarios.DeleteObject(aus);
                    }
                    else if (id == -2)
                    {
                        FeriasFuncionario ff = new FeriasFuncionario();
                        ff.UserID = user.ID;
                        ff.DataFerias = dt.Date;
                        ff.NrHoras = 8;
                        ff.Aprovado = true;
                        ff.Pendente = false;
                        ent.FeriasFuncionarios.AddObject(ff);

                        // REMOVER PRESENCA
                        var presencas = ent.FuncionarioHorasFuncionarios.Where(hf => hf.UserID == user.ID && hf.Data == dt.Date);
                        foreach (var presenca in presencas) ent.FuncionarioHorasFuncionarios.DeleteObject(presenca);

                        // REMOVER AUSENCIA
                        var auss = ent.AusenciaFuncionarios.Where(a => a.UserID == user.ID && a.DataAusencia == dt.Date);
                        foreach (var aus in auss) ent.AusenciaFuncionarios.DeleteObject(aus);
                    }
                    else if (id != 0)
                    {
                        AusenciaFuncionario af = new AusenciaFuncionario();
                        af.TipoAusenciaID = id;
                        af.UserID = user.ID;
                        af.NrHoras = 8;
                        af.DataAusencia = dt.Date;
                        ent.AusenciaFuncionarios.AddObject(af);

                        // REMOVER PRESENCA
                        var presencas = ent.FuncionarioHorasFuncionarios.Where(hf => hf.UserID == user.ID && hf.Data == dt.Date);
                        foreach (var presenca in presencas) ent.FuncionarioHorasFuncionarios.DeleteObject(presenca);

                        // REMOVER FERIAS
                        var ferias = ent.FeriasFuncionarios.Where(f => f.UserID == user.ID && f.DataFerias == dt.Date);
                        foreach (var diaferias in ferias) ent.FeriasFuncionarios.DeleteObject(diaferias);
                    }
                    ent.SaveChanges();
                }
            }
            Response.Redirect(Request.RawUrl);
        }
    }


}