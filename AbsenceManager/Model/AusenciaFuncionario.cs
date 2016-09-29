using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

namespace AbsenceManager
{
    [MetadataType(typeof(AusenciaFuncionarioMetadata))]
    public partial class AusenciaFuncionario : EntityObject
    {
    }

    [DisplayName("Ausências")]
    public class AusenciaFuncionarioMetadata
    {
        [Display(Name = "Funcionário", Order = 1)]
        [UIHint("Autocomplete")]
        [FilterUIHint("Autocomplete")]
        public object User { get; set; }

        [Display(Name = "Data", Order = 2)]
        [UIHint("JQueryCalendarDate", null, "PresentationMode", "Date", "Field", "FlightDate", "FieldClass", "Flight")]
        [FilterUIHint("DateRange")]
        public object DataAusencia { get; set; }

        [Display(Name = "Tipo Ausência", Order = 3)]
        public object TipoAusencia { get; set; }

        [Display(Name = "Nr. Horas", Order = 4)]
        public object NrHoras { get; set; }

        [Display(Name = "Observações", Order = 5)]
        public object Observacoes { get; set; }

        [ScaffoldColumn(false)]
        public object Ano { get; set; }

        [ScaffoldColumn(false)]
        public object Mes { get; set; }

        [ScaffoldColumn(false)]
        public object Dia { get; set; }

    }
}