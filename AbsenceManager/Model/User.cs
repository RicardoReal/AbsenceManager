using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Objects.DataClasses;

namespace AbsenceManager
{
    [MetadataType(typeof(UserMetadata))]
    [DisplayColumn("Nome", "Nome", false)]
    public partial class User : EntityObject
    {
        public override string ToString()
        {

            return string.Format("{0} ({1})", this.NrFuncionario, this.Nome);
        }

        public string NomeEmpresa
        {
            get
            {
                return this.Empresa.Nome;
            }
        }
    }

    [DisplayName("Funcionários")]
    public class UserMetadata
    {
        [Display(Name = "Aprovar Férias", Order = 1)]
        public object FeriasFuncionarios { get; set; }

        [Display(Name = "Nr. Funcionário", Order = 2)]
        public object NrFuncionario { get; set; }

        [Display(Name = "Nome", Order = 3)]
        public object Nome { get; set; }

        [Display(Name = "Username", Order = 4)]
        public object UserName { get; set; }

        [Display(Name = "Password", Order = 5)]
        [UIHint("Password")]
        [HideColumnIn(PageTemplate.List)]
        public object Password { get; set; }

        [Display(Name = "Role", Order = 6)]
        public object Role { get; set; }

        [Display(Name = "Departamento", Order = 7)]
        public object Departamento { get; set; }

        [Display(Name = "Empresa", Order = 8)]
        public object Empresa { get; set; }

        [Display(Name = "Custo Hora", Order = 9)]
        [HideColumnIn(PageTemplate.Insert | PageTemplate.List)]
        public object CustoHora { get; set; }

        [Display(Name = "Activo", Order = 10)]
        public object Activo { get; set; }

        [Display(Name = "Is Administrator", Order = 11)]
        [HideColumnIn(PageTemplate.List)]
        public object IsAdmin { get; set; }

        [Display(Name = "Dias de Férias", Order = 12)]
        public object NrDiasFerias { get; set; }

        [Display(Name = "Compensações", Order = 13)]
        public object Compensacoes { get; set; }

        [Display(Name = "Ausências", Order = 14)]
        public object AusenciaFuncionarios { get; set; }

        [Display(Name = "Presenças", Order = 15)]
        public object FuncionarioHorasFuncionarios { get; set; }

        //[Display(Name = "Registo", Order = 17)]
        //public object TrackUsers { get; set; }

        [Display(Name = "Creation Date")]
        [UIHint("CalendarDateTimeRO")]
        [HideColumnIn(PageTemplate.List, PageTemplate.Insert)]
        public object CreationDate { get; set; }

        [ScaffoldColumn(false)]
        public object Departamentoes { get; set; }

        // FARDAMENTO

        [HideColumnIn(PageTemplate.List)]
        public object NrFardamento { get; set; }

        [HideColumnIn(PageTemplate.List)]
        public object Sapato { get; set; }

        [HideColumnIn(PageTemplate.List)]
        public object Calça { get; set; }

        [HideColumnIn(PageTemplate.List)]
        public object Polo { get; set; }

        [HideColumnIn(PageTemplate.List)]
        public object Casaco { get; set; }

        [HideColumnIn(PageTemplate.List)]
        public object Bata { get; set; }

        // INFO

        [HideColumnIn(PageTemplate.List)]
        [UIHint("JQueryCalendarDate", null, "PresentationMode", "Date", "Field", "FlightDate", "FieldClass", "Flight")]
        public object DataAdmissao { get; set; }

        [HideColumnIn(PageTemplate.List)]
        public object Morada { get; set; }

        [HideColumnIn(PageTemplate.List)]
        public object Telefone { get; set; }

        [HideColumnIn(PageTemplate.List)]
        public object Email { get; set; }

        [HideColumnIn(PageTemplate.List)]
        [UIHint("JQueryCalendarDate", null, "PresentationMode", "Date", "Field", "FlightDate", "FieldClass", "Flight")]
        public object DataNascimento { get; set; }

        [HideColumnIn(PageTemplate.List)]
        public object NIF { get; set; }

        // DOCS
        [Display(Name = "Doc. identificação", Order = 16)]
        [UIHint("Document")]
        [HideColumnIn(PageTemplate.List)]
        public object DocIdentificacao { get; set; }

    }
}