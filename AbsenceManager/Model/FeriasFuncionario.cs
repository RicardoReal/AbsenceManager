using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

namespace AbsenceManager
{
    [MetadataType(typeof(FeriasFuncionarioMetadata))]
    public partial class FeriasFuncionario : EntityObject
    {
    }

    [DisplayName("Férias")]
    public class FeriasFuncionarioMetadata
    {
        [Display(Name = "Funcionário", Order = 1)]
        [FilterUIHint("Autocomplete")]
        public object User { get; set; }

        [Display(Name = "Data", Order = 2)]
        [UIHint("JQueryCalendarDate", null, "PresentationMode", "Date", "Field", "FlightDate", "FieldClass", "Flight")]
        [FilterUIHint("DateRange")]
        public object DataFerias { get; set; }

        [Display(Name = "Nr. Horas", Order = 3)]
        public object NrHoras { get; set; }

        [Display(Name = "Aprovado", Order = 4)]
        public object Aprovado { get; set; }

        [Display(Name = "Pendente", Order = 5)]
        public object Pendente { get; set; }

        [Display(Name = "Observações", Order = 6)]
        [UIHint("MultilineText")]
        public object Observacoes { get; set; }

        [ScaffoldColumn(false)]
        public object Ano { get; set; }

        [ScaffoldColumn(false)]
        public object Mes { get; set; }

        [ScaffoldColumn(false)]
        public object Dia { get; set; }

    }
}