using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

namespace AbsenceManager
{
    [MetadataType(typeof(HorasFuncionarioMetadata))]
    public partial class HorasFuncionario : EntityObject
    {
    }

    [DisplayName("Presenças")]
    [DisplayColumn("Data", "Data", true)]
    public class HorasFuncionarioMetadata
    {
        [Display(Name = "Data", Order = 1)]
        
        [UIHint("JQueryCalendarDate", null, "PresentationMode", "Date", "Field", "FlightDate", "FieldClass", "Flight")]
        [FilterUIHint("DateRange")]
        public object Data { get; set; }

        [Display(Name = "Departamento", Order = 2)]
        public object Departamento { get; set; }

        [Display(Name = "Nr. Presenças", Order = 3)]
        [HideColumnIn(PageTemplate.Insert)]
        public object NrPresencas { get; set; }

        [Display(Name = "Observações", Order = 4)]
        [UIHint("MultilineText")]
        public object Observacoes { get; set; }

        [Display(Name = "Funcionários", Order = 5)]
        [UIHint("GridViewHorasFuncionarios")]
        [HideColumnIn(PageTemplate.List)]
        public object FuncionarioHorasFuncionarios { get; set; }

        [ScaffoldColumn(false)]
        public object Ano { get; set; }

        [ScaffoldColumn(false)]
        public object Mes { get; set; }

        [ScaffoldColumn(false)]
        public object Dia { get; set; }
    }
}